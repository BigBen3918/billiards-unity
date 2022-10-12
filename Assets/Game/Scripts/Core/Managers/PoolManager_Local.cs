//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using UnityEngine;

public class PoolManager_Local : PoolManager
{

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerPrefabAI;

    public static bool isAgainstAI = true;

    protected override void Start()
    {
        base.Start();

        gameUI.SetPrize(0);

        CreateBalls();
        RackBalls();

        CreatePlayers();
    }

    public override void OnPlayerReady()
    {
        if (players.Count < 2)
        {
            return;
        }

        bool allPlayersReady = true;
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].IsReady)
            {
                allPlayersReady = false;

                break;
            }
        }

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

    protected override void TakeTurn(Player player, bool ballInHand, bool isBreak)
    {
        base.TakeTurn(player, ballInHand, isBreak);

        CueBall.gameObject.layer = 8;
        CueBall.GetComponent<Rigidbody>().useGravity = true;

        StartTimer(player);

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
        ProcessTurn(timeFoul);
    }

    private void CreatePlayers()
    {
        for (int i = 0; i < 2; i++)
        {
            bool shouldCreateAIPlayer = i == 1 && isAgainstAI;

            GameObject prefab = playerPrefab;
            if (shouldCreateAIPlayer)
            {
                prefab = playerPrefabAI;
            }

            GameObject playerObj = Instantiate(prefab);
            Player player = playerObj.GetComponent<Player>();

            player.playerId = (i + 1).ToString();
        }
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
            () =>
            {
                AudioManager.Instance.PlayBtnSound();
                PoolSceneManager.Instance.RestartScene();
            },
            () =>
            {
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

        TakeTurn(players[firstPlayerIdx], true, true);
    }

}
