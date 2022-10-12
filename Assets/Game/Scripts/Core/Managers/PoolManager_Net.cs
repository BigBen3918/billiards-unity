//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PoolManager_Net : PoolManager {

	public PreGameUI preGameUI;

	public float prizePerWin = 50;

	public static bool isOnline = true;

	protected override void Awake () {
		preGameUI.ShowUI ();

		base.Awake ();
	}

	protected override void Start() {
		base.Start ();

		if (isOnline) {
			gameUI.SetPrize (prizePerWin);
		}
		else {
			gameUI.SetPrize (0);
		}

		if (isServer) {
			CreateBalls ();
			RackBalls ();
		}
	}

	public override void OnPlayerReady() {
		if (!isServer) {
			return;
		}

		if (players.Count < 2) {
			return;
		}

		bool allPlayersReady = true;
		for(int i = 0; i < players.Count; i++) {
			if (!players [i].IsReady) {
				allPlayersReady = false;

				break;
			}
		}

		if (allPlayersReady) {
			StartGame ();
		}
	}

	protected override void StartGame() {
		NetworkDiscovery_LAN networkDiscovery = GameObject.FindObjectOfType<NetworkDiscovery_LAN> ();
		if (networkDiscovery != null) {
			networkDiscovery.StopServer ();
		}

		StartCoroutine (StartGameCo (1.0f));
	}

	protected override void CreateBalls() {
		GameObject cueBallObj = Instantiate (cueBallPrefab);
		cueBallObj.transform.position = table.HeadSpot.position;

		NetworkServer.Spawn (cueBallObj);

		for (int i = 0; i < ballPrefabs.Count; i++) {
			GameObject ballObj = Instantiate (ballPrefabs [i]);
			ballObj.transform.rotation = Random.rotation;

			NetworkServer.Spawn (ballObj);
		}
	}

	protected override void TakeTurn(Player player, bool ballInHand, bool isBreak) {
		base.TakeTurn (player, ballInHand, isBreak);

		StartCoroutine (TakeTurnCo (player, ballInHand, isBreak));
	}

	private IEnumerator TakeTurnCo(Player player, bool ballInHand, bool isBreak) {
		yield return null;

		NetworkIdentity cueBallNetIdentity = CueBall.GetComponent<NetworkIdentity> ();
		if (cueBallNetIdentity.clientAuthorityOwner == WaitingPlayer.connectionToClient) {
			cueBallNetIdentity.RemoveClientAuthority (WaitingPlayer.connectionToClient);
		}

		cueBallNetIdentity.AssignClientAuthority (player.connectionToClient);

		RpcTakeTurn (player.netId, ballInHand, isBreak);
	}

	public override void OnTurnTaken(Player player, bool timeFoul) {
		base.OnTurnTaken (player, timeFoul);

		if (isServer) {
			RpcStopTimer ();
			ProcessTurn (timeFoul);
		}
	}

	public override void ShowMsg (string msg) {
		RpcShowMsg (msg);
	}

	protected override void PlayRackSound() {
		RpcPlayRackSound ();
	}

	protected override void PlayFoulSound() {
		RpcPlayFoulSound ();
	}

	public override void ConcludeGame (Player winner) {
		if (isServer) {
			if (winner == null) {
				RpcConcludeGameDraw ();
			}
			else {
				RpcConcludeGame (winner.netId);
			}
		}
		else {
			if (winner == null) {
				ConcludeGameDrawLocal ();
			}
			else {
				ConcludeGameLocal (winner.netId);
			}
		}
	}

	private IEnumerator StartGameCo(float delay) {
		yield return new WaitForSeconds (delay);

		base.StartGame ();

		RpcHidePreGameUI ();

		int firstPlayerIdx = 0;
		TakeTurn (players[firstPlayerIdx], true, true);
	}

	private void ConcludeGameLocal(NetworkInstanceId winnerNetId) {
		if (IsGameConcluded) {
			return;
		}

		Player winner = ClientScene.FindLocalObject (winnerNetId).GetComponent<Player> ();
		base.ConcludeGame (winner);

		preGameUI.ShowUI ();

		if (isOnline) {
			if (winner.isLocalPlayer) {
				preGameUI.thisPlayerUI.MarkWinner ("+ " + prizePerWin);

				PlayerInfo.Instance.CoinBalance += prizePerWin;
			}
			else {
				preGameUI.otherPlayerUI.MarkWinner ("+ " + prizePerWin);
			}
		}
		else {
			if (winner.isLocalPlayer) {
				preGameUI.thisPlayerUI.MarkWinner ("");
			}
			else {
				preGameUI.otherPlayerUI.MarkWinner ("");
			}
		}
	}

	private void ConcludeGameDrawLocal() {
		if (IsGameConcluded) {
			return;
		}

		base.ConcludeGame (null);

		CustomNetworkManager nm = GameObject.FindObjectOfType<CustomNetworkManager> ();
		if (nm != null) {
			nm.ShowDisconnectPopup ();
		}
	}

	#region COMMANDS



	#endregion

	#region RPCS

	[ClientRpc]
	private void RpcHidePreGameUI() {
		preGameUI.HideUI ();
	}

	[ClientRpc]
	private void RpcTakeTurn(NetworkInstanceId playerNetId, bool ballInHand, bool isBreak) {
		CueBall.gameObject.layer = 8;
		CueBall.GetComponent<Rigidbody> ().useGravity = true;

		Player player = ClientScene.FindLocalObject (playerNetId).GetComponent<Player> ();

		StartTimer (player);

		if (player.isLocalPlayer) {
			gameUI.EnableControls (true);

			if (ballInHand && isBreak) {
				player.TakeBallInHand (true);
			}
			else if (ballInHand) {
				player.TakeBallInHand (false);
			}

			player.TakeTurn ();
		}

		gameUI.ResetSpin ();
	}

	[ClientRpc]
	private void RpcStopTimer() {
		StopTimer ();
	}

	[ClientRpc]
	public void RpcShowMsg(string msg) {
		base.ShowMsg (msg);
	}

	[ClientRpc]
	private void RpcPlayRackSound() {
		base.PlayRackSound ();
	}

	[ClientRpc]
	private void RpcPlayFoulSound() {
		base.PlayFoulSound ();
	}

	[ClientRpc]
	private void RpcConcludeGame(NetworkInstanceId winnerNetId) {
		ConcludeGameLocal (winnerNetId);
	}

	[ClientRpc]
	private void RpcConcludeGameDraw() {
		ConcludeGameDrawLocal ();
	}

	#endregion

}
