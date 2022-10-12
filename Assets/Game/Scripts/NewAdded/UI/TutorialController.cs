using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialController : PoolManager
{

	[SerializeField] private GameObject playerPrefab;

	
	public static bool isAgainstAI = true;
	public TutorialCue tuto_cue;
	public GameObject tutor_content;
	public GameObject hand_slide;
	public GameObject hand_cue;
	public Text content;
	private int cur_tuto = 0;
	protected override void Start()
	{
		base.Start();

		gameUI.SetPrize(0);

		CreateBalls();
		RackBalls();
		
		CreatePlayers();



		tutor_content.SetActive(false);
		hand_slide.SetActive(false);
		hand_cue.SetActive(false);
		content.text = "Welcome, I gonna teach you how to play this game.";
		StartCoroutine(iShow());
}

	public void OnClickNextBtn()
	{
		cur_tuto++;
		switch (cur_tuto)
		{
			case 1:
				hand_cue.SetActive(true);
				hand_slide.SetActive(false);
				content.text = "Drag to teak cue.";
				break;
			case 2:
				hand_cue.SetActive(false);
				hand_slide.SetActive(true);
				content.text = "Slide to stick ball";
				break;
			case 3:
				hand_cue.SetActive(false);
				hand_slide.SetActive(false);
				content.text = "Okay,Enjoy game.";

				break;
			case 4:
				tutor_content.SetActive(false);
				hand_slide.SetActive(false);
				hand_cue.SetActive(false);
				break;

			case 5:
				PoolSceneManager.Instance.MyLoadScene("MainMenu");
				break;

		}
	
	}

	IEnumerator iShow()
	{
		print("333");
		yield return new WaitForSeconds(1f);
		tutor_content.SetActive(true);
		print("555");
	}
	public override void OnPlayerReady()
	{
		/*
		if (players.Count < 2)
		{
			return;
		}
		*/
		bool allPlayersReady = true;
		/*  
		 *  for (int i = 0; i < players.Count; i++)
		{
			if (!players[i].IsReady)
			{
				allPlayersReady = false;

				break;
			}
		}
		*/


		if (allPlayersReady)
		{
			StartGame();
		}
	}

	protected override void StartGame()
	{
		StartCoroutine(StartGameCo(0.5f));
	}

	protected override void CreateBalls()
	{
		GameObject cueBallObj = Instantiate(cueBallPrefab);
		cueBallObj.transform.position = table.HeadSpot.position;

		for (int i = 0; i < ballPrefabs.Count; i++)
		{
			GameObject ballObj = Instantiate(ballPrefabs[i]);
			ballObj.transform.rotation = Random.rotation;
		}
	}
	public void ToGame()
	{
		
		StartCoroutine(iToGame());

	}
	IEnumerator iToGame()
	{
		yield return new WaitForSeconds(3f);
		tutor_content.SetActive(true);
		content.text = "Thank you";
	}
	public void My_TakeTurn(Player player, bool ballInHand, bool isBreak)
	{

		Turn turn = new Turn(player, isBreak);
		turns.Add(turn);

		string msg = Formatter.FormatName(player.playerName) + "'s turn.";
		ShowMsg(msg);

		player.SetTurnState();
	//	WaitingPlayer.SetWaitState();

		turnCount++;
	}
	protected override void TakeTurn(Player player, bool ballInHand, bool isBreak)
	{
		Debug.Log("abc");
		//base.TakeTurn(player, ballInHand, isBreak);
		My_TakeTurn(player, ballInHand, isBreak);
		CueBall.gameObject.layer = 8;
		CueBall.GetComponent<Rigidbody>().useGravity = true;

		//StartTimer(player);
		
		if (player.GetType() != typeof(Player_AI))
		{
			gameUI.EnableControls(true);
		}

		if (ballInHand && isBreak)
		{
			player.TakeBallInHand(true);
		}
		else if (ballInHand)
		{
			player.TakeBallInHand(false);
		}

		player.TakeTurn();

		gameUI.ResetSpin();
	}

	public override void OnTurnTaken(Player player, bool timeFoul)
	{
		base.OnTurnTaken(player, timeFoul);

		StopTimer();
		
	//	ProcessTurn(timeFoul);
	}

	private void CreatePlayers()
	{
	

		GameObject prefab = playerPrefab;
		GameObject playerObj = Instantiate(prefab);
		Player player = playerObj.GetComponent<Player>();

		player.playerId = "1";
	}

	public override void ConcludeGame(Player winner)
	{
		if (IsGameConcluded)
		{
			return;
		}

		base.ConcludeGame(winner);

		string popupHeading = Formatter.FormatName(winner.playerName) + " wins!";
		string popupMsg = Formatter.FormatName(winner.playerName) + " has won this match." +
						  "\n\nWant a re-match?";

		PopupManager.Instance.ShowPopup(popupHeading, popupMsg, "Re-match", "Quit",
			() => {
				AudioManager.Instance.PlayBtnSound();
				PoolSceneManager.Instance.RestartScene();
			},
			() => {
				AudioManager.Instance.PlayBtnSound();
				PoolSceneManager.Instance.GoToMainMenu();
			},
			null);

		AdsManager.Instance.ShowVideoAd();
	}

	public override void PauseGame()
	{
		base.PauseGame();

		Time.timeScale = 0;
	}

	public override void UnpauseGame()
	{
		base.UnpauseGame();

		Time.timeScale = 1;
	}

	private IEnumerator StartGameCo(float delay)
	{
		yield return new WaitForSeconds(delay);
		
		base.StartGame();

		int firstPlayerIdx = Random.Range(0, 2);
		if (!isAgainstAI)
		{
			firstPlayerIdx = 0;
		}

		TakeTurn(players[0], true, true);
	}


	public void OnClickNextTutorial()
	{
		gameUI.EnableControls(true);
		
	
	}
}
