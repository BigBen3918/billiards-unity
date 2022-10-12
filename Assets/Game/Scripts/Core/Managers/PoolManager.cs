//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using NonUnitySystem = System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum BallType {
	SOLID,
	STRIPED,
	EIGHT,
	CUE,
	UNKNOWN
}

public enum MoveType {
	WIN,
	LOSS,
	FOUL,
	SCORED,
	NOT_SCORED
}

public class PoolManager : NetworkBehaviour {

	[SerializeField] protected Table table;
	[SerializeField] protected GameObject cueBallPrefab;
	[SerializeField] protected List<GameObject> ballPrefabs;
	[SerializeField] protected PointGroup ballCollectorPoints;
	[SerializeField] protected GameUI gameUI;

	[SerializeField] private GameObject cueBallIndicatorPrefab;
	[SerializeField] private GameObject ballInHandIndicatorPrefab;

	[SerializeField] private AudioClip rackSound;
	[SerializeField] private AudioClip foulSound;

	protected List<Player> players = new List<Player>();

	public Table PoolTable {
		get {
			return table;
		}
	}

	public Ball CueBall {
		get {
			return GameManager.GetBall (0);
		}
	}

	public Turn CurrentTurn {
		get {
			if (turns == null || turns.Count == 0) {
				return null;
			}

			return turns [turns.Count - 1];
		}
	}

	public Player CurrentPlayer {
		get {
			return CurrentTurn.Player;
		}
	}

	public Player WaitingPlayer {
		get {
			
			if (CurrentPlayer.playerId.Equals (players[0].playerId)) {
				return players [1];
			}

			if (CurrentPlayer.playerId.Equals (players[1].playerId)) {
				return players [0];
			}

			return null;
		}
	}

	public List<int> PocketedBalls {
		get {
			List<int> pocketedBalls = new List<int> ();
			foreach (Turn turn in turns) {
				pocketedBalls.AddRange (turn.PocketedBalls);
			}

			if (pocketedBalls.Contains (CueBall.BallNumber)) {
				pocketedBalls.Remove (CueBall.BallNumber);
			}

			return pocketedBalls;
		}
	}

	public List<Transform> BallCollectorNodes {
		get {
			return ballCollectorPoints.nodes;
		}
	}

	protected List<Ball> balls;
	protected List<Ball> Balls {
		get {
			if (balls == null) {
				balls = new List<Ball> ();
				for (int i = 1; i < GameManager.BallsCount; i++) {
					balls.Add (GameManager.GetBall (i));
				}
			}

			return balls;
		}
	}

	public bool IsGamePaused {
		get;
		private set;
	}

	[SyncVar]
	public  int turnCount = 0;
	public int TurnCount {
		get {
			return turnCount;
		}
	}

	public bool IsBreakDone {
		get {
			return TurnCount > 1;
		}
	}

	public bool IsGameConcluded {
		get;
		private set;
	}

	protected List<Turn> turns = new List<Turn> ();

	protected Coroutine timerCo;

	private GameObject cueBallIndicator;
	private ObjectFollower cueBallIndicatorFollower;
	private SpriteRenderer cueBallIndicatorRenderer;
	private GameObject ballInHandIndicator;
	private ObjectFollower ballInHandIndicatorFollower;

	protected virtual void Awake() {
		CreateUtils ();
	}

	protected virtual void OnEnable() {
		
	}

	protected virtual void Start() {

	}

	protected virtual void OnDisable() {
		
	}

	public virtual void OnPlayerReady() {
		
	}

	protected virtual void StartGame() {
		PlayRackSound ();
	}

	protected virtual void CreateBalls() {
		
	}

	protected void RackBalls() {
		List<Ball> ballsToSkip = new List<Ball> ();
		ballsToSkip.Add (GetBall (8));

		Ball topCornerBall = GetRandomBall ();
		Ball bottomCornerBall;
		if (topCornerBall.Type == BallType.SOLID) {
			bottomCornerBall = GetRandomBall (BallType.STRIPED);
		}
		else {
			bottomCornerBall = GetRandomBall (BallType.SOLID);
		}

		ballsToSkip.Add (topCornerBall);
		ballsToSkip.Add (bottomCornerBall);

		List<Ball> shuffledBalls = RandomizeBalls (Balls, ballsToSkip);

		Vector3 ballPosition = new Vector3 ();
		int k = 0;
		for (int i = 0; i < 5; i++) {
			ballPosition.y = table.FootSpot.position.y;

			float deltaX = Balls [i].Radius * 1.8f;
			ballPosition.x = table.FootSpot.position.x + (deltaX * i);

			float deltaZ = Balls [i].Radius * 1.05f;
			ballPosition.z = table.FootSpot.position.z + (deltaZ * i);

			for (int j = 0; j < i + 1; j++) {
				Ball ballToPlace;
				// Place 8-ball
				if (i == 2 && j == 1) {
					ballToPlace = GetBall (8);
				}
				// Place top corner ball
				else if (i == 4 && j == 0) {
					ballToPlace = topCornerBall;
				}
				// Place bottom corner ball
				else if (i == 4 && j == 4) {
					ballToPlace = bottomCornerBall;
				}
				// Place other balls
				else {
					ballToPlace = shuffledBalls [k];
					k++;
				}

				ballToPlace.transform.position = ballPosition;
				ballPosition.z -= (deltaZ * 2);
			}
		}
	}

	protected virtual void TakeTurn(Player player, bool ballInHand, bool isBreak) {
		Turn turn = new Turn (player, isBreak);
		turns.Add (turn);

		string msg = Formatter.FormatName(player.playerName) + "'s turn.";
		ShowMsg (msg);

		player.SetTurnState ();
		WaitingPlayer.SetWaitState ();

		turnCount++;
	}

	public virtual void OnTurnTaken(Player player, bool timeFoul) {
		player.StopTimerSound ();
	}

	protected void OnFoul(string reason) {
		ShowMsg (reason);
		PlayFoulSound ();
	}

	protected void ProcessTurn(bool timeFoul) {
		StartCoroutine (ProcessTurnCo (timeFoul));
	}

	private IEnumerator ProcessTurnCo(bool timeFoul) {
		yield return StartCoroutine (WaitForBallsCo (0.25f));

		if (timeFoul) {
			PlayFoulSound ();

			string msg = Formatter.FormatName(CurrentPlayer.playerName) + " ran out of time.";
			ShowMsg (msg);

			msg = Formatter.FormatName(WaitingPlayer.playerName) + " has the ball in hand.";
			ShowMsg (msg);

			SwapTurns (true);

			yield break;
		}

		MoveType move = CheckMove (CurrentTurn);
		CurrentTurn.SetMoveType (move);

		if (move == MoveType.WIN) {
			ConcludeGame (CurrentPlayer);
		}
		else if (move == MoveType.LOSS) {
			ConcludeGame (WaitingPlayer);
		}
		else if (move == MoveType.FOUL) {
			string msg = Formatter.FormatName(WaitingPlayer.playerName) + " has the ball in hand.";
			ShowMsg (msg);

			SwapTurns (true);
		}
		else if (move == MoveType.SCORED) {
			ContinueTurn ();
		}
		else if (move == MoveType.NOT_SCORED) {
			SwapTurns ();
		}

		UpdateRemainingBalls ();
	}

	protected IEnumerator WaitForBallsCo(float checkInterval) {
		yield return new WaitForFixedUpdate ();

		List<Ball> ballsToCheck = new List<Ball> ();
		for (int i = 0; i <= Balls.Count; i++) {
			Ball ball = GetBall (i);
			if (!IsBallPocketed(ball)) {
				ballsToCheck.Add (ball);
			}
		}

		ballsToCheck.Add (GetBall (CueBall.BallNumber));

		while (!HaveBallsStopped(ballsToCheck)) {
			yield return new WaitForSeconds (checkInterval);
		}
	}

	protected void ContinueTurn() {
		TakeTurn (CurrentPlayer, false, false);
	}

	protected void SwapTurns(bool ballInHand = false) {
		TakeTurn (WaitingPlayer, ballInHand, false);
	}

	public Ball GetBall(int ballNumber) {
		if (ballNumber < 0 || ballNumber > Balls.Count) {
			return null;
		}

		if (ballNumber == 0) {
			return CueBall;
		}

		return Balls [ballNumber - 1];
	}

	public Ball GetRandomBall(BallType type) {
		if (type == BallType.CUE) {
			return CueBall;
		}

		if (type == BallType.EIGHT) {
			return GetBall (8);
		}

		if (type == BallType.SOLID) {
			int randomBallNumber = Random.Range (1, 8);
			return GetBall (randomBallNumber);
		}

		if (type == BallType.STRIPED) {
			int randomBallNumber = Random.Range (9, 16);
			return GetBall (randomBallNumber);
		}

		return null;
	}

	public Ball GetRandomBall() {
		int randomType = Random.Range (0, 2);
		if (randomType == 1) {
			return GetRandomBall (BallType.SOLID);
		}
		else {
			return GetRandomBall (BallType.STRIPED);
		}
	}

	public List<Ball> GetBallsOfType(List<int> balls, BallType type) {
		List<Ball> matchingBalls = new List<Ball> ();

		for (int i = 0; i < balls.Count; i++) {
			Ball ball = GetBall (balls [i]);
			if (ball.Type == type) {
				matchingBalls.Add (ball);
			}
		}

		return matchingBalls;
	}

	private List<Ball> RandomizeBalls(List<Ball> balls, List<Ball> except = null) {
		List<Ball> randomizedBalls = PoolUtils.Shuffle (balls);

		if (except != null) {
			foreach (Ball ball in except) {
				randomizedBalls.Remove (ball);
			}
		}

		return randomizedBalls;
	}

	public bool IsBallPocketed(Ball ball) {
		return PocketedBalls.Contains (ball.BallNumber);
	}

	private bool HaveBallsStopped(List<Ball> balls) {
		foreach (Ball ball in balls) {
			if (!ball.IsAtRest) {
				return false;
			}
		}

		return true;
	}

	private MoveType CheckMove(Turn turn) {
		// **************************** CHECK FOR FOUL ****************************
		bool isFoul = false;
		string foul = "";

		// Cue ball potted
		if (turn.IsCueBallPocketed) {
			if (foul.Equals ("")) {
				foul = turn.Player.playerName + " potted the cue ball.";
				OnFoul (foul);
			}
		}

		// Illegal break
		if (turn.IsBreak) {
			int cushionedBalls = turn.CushionedBalls.Count;
			if (turn.CushionedBalls.Contains (CueBall.BallNumber)) {
				cushionedBalls--;
			}

			bool requiredBallsHitCushions = cushionedBalls >= 4;

			if(!requiredBallsHitCushions && turn.PocketedBalls.Count == 0) {
				if (foul.Equals ("")) {
					foul = turn.Player.playerName + " made an illegal break.";
					OnFoul (foul);
				}
			}
		}

		// Cue ball did not hit any ball
		if (turn.BallsHitByCueBall.Count == 0) {
			if (foul.Equals ("")) {
				foul = "Cue ball did not strike another ball.";
				OnFoul (foul);
			}
		}
		// Player did not hit one of his group's balls
		else if (turn.Player.AssignedBalls != BallType.UNKNOWN) {
			if (GetBall (turn.BallsHitByCueBall [0]).Type != turn.Player.AssignedBalls) {
				if (foul.Equals ("")) {
					foul = turn.Player.playerName + " failed to hit ";

					if (turn.Player.AssignedBalls == BallType.SOLID) {
						foul += "a SOLID ball.";
					}
					if (turn.Player.AssignedBalls == BallType.STRIPED) {
						foul += "a STRIPED ball.";
					}
					if (turn.Player.AssignedBalls == BallType.EIGHT) {
						foul += "the EIGHT ball.";
					}

					OnFoul (foul);
				}
			}
		}

		// No ball hit the rails after first contact
		if (turn.Player.AssignedBalls != BallType.UNKNOWN && turn.PocketedBalls.Count == 0 &&
			!turn.BallsHitRailsAfterContact) {

			if (foul.Equals ("")) {
				foul = "No ball hit the rails after initial contact.";
				OnFoul (foul);
			}
		}

		isFoul = !foul.Equals ("");

		// **************************** CHECK FOR WIN/LOSS ****************************

		// 8-ball pocketed
		if (turn.IsEightBallPocketed) {
			if (isFoul) {
				return MoveType.LOSS;
			}

			if (turn.Player.AssignedBalls == BallType.EIGHT) {
				return MoveType.WIN;
			}

			return MoveType.LOSS;
		}

		if (isFoul) {
			if (turn.Player.AssignedBalls != BallType.UNKNOWN) {
				if (GetRemainingBalls (turn.Player).Count == 0) {
					if (turn.Player.AssignedBalls != BallType.EIGHT) {
						turn.Player.AssignBalls (BallType.EIGHT);
					}
				}

				if (GetRemainingBalls (WaitingPlayer).Count == 0) {
					if (WaitingPlayer.AssignedBalls != BallType.EIGHT) {
						WaitingPlayer.AssignBalls (BallType.EIGHT);
					}
				}
			}

			return MoveType.FOUL;
		}

		if (turn.PocketedBalls.Count == 0) {
			return MoveType.NOT_SCORED;
		}

		if (turn.Player.AssignedBalls == BallType.UNKNOWN) {
			if (!turn.IsBreak) {
				BallType currentPlayerBalls = BallType.UNKNOWN;
				BallType waitingPlayerBalls = BallType.UNKNOWN;

				if (GetBallsOfType (turn.PocketedBalls, BallType.SOLID).Count >
				   GetBallsOfType (turn.PocketedBalls, BallType.STRIPED).Count) {

					currentPlayerBalls = BallType.SOLID;
					waitingPlayerBalls = BallType.STRIPED;
				}
				else if (GetBallsOfType (turn.PocketedBalls, BallType.STRIPED).Count >
				        GetBallsOfType (turn.PocketedBalls, BallType.SOLID).Count) {

					currentPlayerBalls = BallType.STRIPED;
					waitingPlayerBalls = BallType.SOLID;
				}
				else {
					Ball firstPocketedBall = GetBall (turn.PocketedBalls [0]);
					if (firstPocketedBall.Type == BallType.SOLID) {
						currentPlayerBalls = BallType.SOLID;
						waitingPlayerBalls = BallType.STRIPED;
					}
					else if (firstPocketedBall.Type == BallType.STRIPED) {
						currentPlayerBalls = BallType.STRIPED;
						waitingPlayerBalls = BallType.SOLID;
					}
				}

				if(currentPlayerBalls != BallType.UNKNOWN && waitingPlayerBalls != BallType.UNKNOWN) {
					turn.Player.AssignBalls (currentPlayerBalls);
					WaitingPlayer.AssignBalls (waitingPlayerBalls);

					string currentPlayerBallType = "";
					if (CurrentPlayer.AssignedBalls == BallType.SOLID) {
						currentPlayerBallType = "SOLIDS";
					}
					else if (CurrentPlayer.AssignedBalls == BallType.STRIPED) {
						currentPlayerBallType = "STRIPES";
					}

					string msg = CurrentPlayer.playerName + " takes " + currentPlayerBallType + ".";
					ShowMsg (msg);
				}
			}

			return MoveType.SCORED;
		}
		else {
			bool scored = false;
			if (GetBallsOfType (turn.PocketedBalls, turn.Player.AssignedBalls).Count == 0) {
				scored = false;
			}
			else {
				scored = true;
			}

			if (GetRemainingBalls (turn.Player).Count == 0) {
				if (turn.Player.AssignedBalls != BallType.EIGHT) {
					turn.Player.AssignBalls (BallType.EIGHT);
				}
			}

			if (GetRemainingBalls (WaitingPlayer).Count == 0) {
				if (WaitingPlayer.AssignedBalls != BallType.EIGHT) {
					WaitingPlayer.AssignBalls (BallType.EIGHT);
				}
			}

			if (scored) {
				return MoveType.SCORED;
			}
			else {
				return MoveType.NOT_SCORED;
			}
		}
	}

	public bool DoBallsOverlap(Vector3 ball1Pos, float ball1Radius,
		Vector3 ball2Pos, float ball2Radius, float offset = 0) {

		float threshold = ball1Radius + ball2Radius;
		threshold = threshold + (threshold * offset);

		if (Vector3.Distance (ball1Pos, ball2Pos) < threshold) {
			return true;
		}

		return false;
	}

	public bool BallOverlapsOtherBalls(Ball ball, Vector3 ballPos, float offset = 0) {
		for (int i = 0; i <= Balls.Count; i++) {
			Ball otherBall = GetBall (i);
			if (ball != otherBall) {
				if (DoBallsOverlap (ballPos, ball.Radius, otherBall.transform.position,
					otherBall.Radius, offset)) {

					return true;
				}
			}
		}

		return false;
	}

	public virtual void ShowMsg(string msg) {
		ShowMsgLocal (msg);
	}

	public void ShowMsgLocal(string msg) {
		gameUI.toastHandler.ShowToast (msg, 2);
	}

	protected void StartTimer(Player player) {
		timerCo = StartCoroutine (TimerCo (player));
	}

	private IEnumerator TimerCo(Player player) {
		float totalTime = player.CueController.MaxTimePerMove;
		Timer playerTimer = player.ui.timer;

		float timer = 0;
		while (timer < totalTime) {
			float fill = Mathf.Lerp (1, 0, timer / totalTime);
			playerTimer.TimerImage.fillAmount = fill;

			if (timer < 0.5f) {
				playerTimer.TimerImage.color = Color.white;
			}
			else {
				if (fill > 0.35f) {
					playerTimer.TimerImage.color = playerTimer.normalColor;
				}
				else if (fill > 0.15f) {
					playerTimer.TimerImage.color = playerTimer.mediumColor;
				}
				else {
					playerTimer.TimerImage.color = playerTimer.lowColor;
					playerTimer.StartTickSound ();
				}
			}

			timer += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}

		player.EndTurn (true);
	}

	protected void StopTimer() {
		if (timerCo != null) {
			StopCoroutine (timerCo);
		}
	}

	public List<int> GetRemainingBalls(Player player) {
		List<int> remainingBalls = new List<int> ();
		for (int i = 0; i < Balls.Count; i++) {
			Ball ball = Balls [i];
			if (!PocketedBalls.Contains (ball.BallNumber) && player.AssignedBalls == ball.Type) {
				remainingBalls.Add (ball.BallNumber);
			}
		}

		return remainingBalls;
	}

	public void UpdateRemainingBalls() {
		CurrentPlayer.UpdateBallImages (GetRemainingBalls (CurrentPlayer).ToArray());
		WaitingPlayer.UpdateBallImages (GetRemainingBalls (WaitingPlayer).ToArray());
	}

	public void AddPlayer(Player player) {
		if (players.Count < 2) {
			players.Add (player);
		}
	}

	protected virtual void PlayRackSound() {
		AudioSource.PlayClipAtPoint (rackSound, Camera.main.transform.position);
	}

	protected virtual void PlayFoulSound() {
		AudioSource.PlayClipAtPoint (foulSound, Camera.main.transform.position);
	}

	public virtual void ConcludeGame(Player winner) {
		if (IsGameConcluded) {
			return;
		}

		string msg = "";
		if (winner == null) {
			msg = "Game is draw!";
		}
		else {
			msg = winner.playerName + " wins!";
		}

		ShowMsgLocal (msg);

		IsGameConcluded = true;
	}

	public virtual void PauseGame() {
		if (IsGamePaused) {
			return;
		}

		IsGamePaused = true;
	}

	public virtual void UnpauseGame() {
		if (!IsGamePaused) {
			return;
		}

		IsGamePaused = false;
	}

	public bool IsBallAllowed(Player player, Ball ball) {
		return player.AssignedBalls == BallType.UNKNOWN || player.AssignedBalls == ball.Type;
	}

	public void EnableCueBallIndicator() {
		cueBallIndicatorFollower.target = CueBall.transform;
		cueBallIndicator.SetActive (true);
	}

	public void DisableCueBallIndicator() {
		cueBallIndicatorFollower.target = null;
		cueBallIndicator.SetActive (false);
	}

	public void EnableBallInHandIndicator() {
		ballInHandIndicatorFollower.target = CueBall.transform;
		ballInHandIndicator.SetActive (true);
	}

	public void DisableBallInHandIndicator() {
		ballInHandIndicatorFollower.target = null;
		ballInHandIndicator.SetActive (false);
	}

	public bool IsCueBallSelected(Vector3 touchPosition) {
		if (cueBallIndicatorRenderer == null) {
			return false;
		}

		return Vector3.Distance (CueBall.transform.position, touchPosition) <=
		cueBallIndicatorRenderer.bounds.size.x / 2.7f;
	}

	public Player GetLocalPlayer() {
		if (players == null) {
			return null;
		}

		foreach (Player p in players) {
			if (p.isLocalPlayer) {
				return p;
			}
		}

		return null;
	}

	public Player GetOtherPlayer(Player currentPlayer) {
		if (players == null || players.Count < 2) {
			return null;
		}

		if (players [0].playerId.Equals (currentPlayer.playerId)) {
			return players [1];
		}

		return players [0];
	}

	private void CreateUtils() {
		cueBallIndicator = Instantiate (cueBallIndicatorPrefab);
		cueBallIndicatorFollower = cueBallIndicator.GetComponent<ObjectFollower> ();
		cueBallIndicatorRenderer = cueBallIndicator.GetComponent<SpriteRenderer> ();
		cueBallIndicator.SetActive (false);

		ballInHandIndicator = Instantiate (ballInHandIndicatorPrefab);
		ballInHandIndicatorFollower = ballInHandIndicator.GetComponent<ObjectFollower> ();
		ballInHandIndicator.SetActive (false);
	}

}
