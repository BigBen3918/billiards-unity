//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

    [SyncVar]
    public string playerId = "";

    [SyncVar(hook = "OnNameChanged")]
    public string playerName = "";

    [SyncVar(hook = "OnAvatarUpdated")]
    public string playerAvatarId = "";

    [SerializeField]
    protected GameObject cuePrefab;

    [HideInInspector]
    public PlayerUI ui;

    protected PoolManager poolManager;
    protected InputManager inputManager;
    protected GameUI gameUI;
    protected GameObject cue;
    protected GameObject cueBall;

    public CueController CueController
    {
        get;
        set;
    }

    [SyncVar]
    protected BallType ballType = BallType.UNKNOWN;
    public BallType AssignedBalls
    {
        get
        {
            return ballType;
        }
    }

    public bool HasBallInHand
    {
        get;
        protected set;
    }

    protected bool isReady = false;
    public bool IsReady
    {
        get
        {
            return isReady;
        }
    }

    private float collisionCheckOffset = 0.15f;

    protected virtual void Awake()
    {
        poolManager = GameObject.FindObjectOfType<PoolManager>();
        inputManager = GameObject.FindObjectOfType<InputManager>();
        gameUI = GameObject.FindObjectOfType<GameUI>();
    }

    protected virtual void Start()
    {
        cueBall = poolManager.CueBall.gameObject;
    }

    private void OnNameChanged(string name)
    {
        HandleNameChange(name);
    }

    private void OnAvatarUpdated(string avatarId)
    {
        HandleAvatarUpdate(avatarId);
    }

    protected virtual void HandleNameChange(string name)
    {
        playerName = name;
        ui.SetNameTxt(playerName);
    }

    protected virtual void HandleAvatarUpdate(string avatarId)
    {
        playerAvatarId = avatarId;
        ui.SetAvatar(PlayerInfo.Instance.GetAvatar(playerAvatarId).AvatarSprite);
    }

    public virtual void TakeBallInHand(bool behindHeadString = false)
    {
        HasBallInHand = true;
        StartCoroutine(BallInHandCo(behindHeadString));
    }

    public virtual void EndBallInHand()
    {
        HasBallInHand = false;

        poolManager.DisableBallInHandIndicator();
        poolManager.DisableCueBallIndicator();
    }

    protected virtual IEnumerator BallInHandCo(bool behindHeadString)
    {
        poolManager.EnableBallInHandIndicator();

        Transform cueBall = poolManager.CueBall.transform;

        bool isBallSelected = false;
        Bounds allowedArea = poolManager.PoolTable.PlayingAreaBounds;

        if (behindHeadString)
        {
            allowedArea = poolManager.PoolTable.HeadStringAreaBounds;
        }

        PlaceCueBallAfterFoul(behindHeadString);

        while (HasBallInHand)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(inputManager.TouchPosition.x,
                inputManager.TouchPosition.y, Camera.main.transform.position.y -
                cueBall.transform.position.y));

            Vector3 transformedPoint = mousePosition;
            transformedPoint.y = cueBall.position.y;

            if (inputManager.State == InputManager.TouchState.START)
            {
                if (poolManager.IsCueBallSelected(transformedPoint))
                {
                    isBallSelected = true;
                    CueController.Deactivate();
                    gameUI.EnableControls(false);

                    poolManager.EnableCueBallIndicator();

                    if (behindHeadString)
                    {
                        poolManager.PoolTable.ShowHeadStringArea();
                    }
                }
            }

            if (isBallSelected)
            {
                bool ballOverlapsOtherBalls = poolManager.BallOverlapsOtherBalls(poolManager.CueBall,
                    transformedPoint, collisionCheckOffset);

                if (!ballOverlapsOtherBalls)
                {
                    transformedPoint.y = allowedArea.center.y;
                    Vector3 cueBallPosition;

                    if (PoolUtils.IsPointInBounds(allowedArea, transformedPoint))
                    {
                        cueBallPosition = new Vector3(transformedPoint.x, cueBall.position.y,
                            transformedPoint.z);
                    }
                    else
                    {
                        Vector3 nearestPoint = PoolUtils.NearestPointOnBounds(allowedArea, transformedPoint);
                        cueBallPosition = new Vector3(nearestPoint.x, cueBall.position.y,
                            nearestPoint.z);
                    }

                    ballOverlapsOtherBalls = poolManager.BallOverlapsOtherBalls(poolManager.CueBall,
                        cueBallPosition, collisionCheckOffset);

                    if (!ballOverlapsOtherBalls)
                    {
                        SetCueBallPosition(cueBallPosition);
                    }
                }
            }

            if (inputManager.State == InputManager.TouchState.END)
            {
                isBallSelected = false;
                CueController.Activate();
                gameUI.EnableControls(true);

                poolManager.DisableCueBallIndicator();

                if (behindHeadString)
                {
                    poolManager.PoolTable.HideHeadStringArea();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                poolManager.DisableBallInHandIndicator();
            }

            yield return null;
        }
    }

    public virtual void ReportReady()
    {
        isReady = true;
        poolManager.OnPlayerReady();
    }

    public virtual void TakeTurn()
    {
        CueController.Activate();
    }

    public void AssignBalls(BallType type)
    {
        if (type != BallType.SOLID && type != BallType.STRIPED && type != BallType.EIGHT)
        {
            return;
        }

        ballType = type;
    }

    public virtual void EndTurn(bool timeFoul = false)
    {
        EndBallInHand();
        ReportTurnEnd(timeFoul);

        CueController.Deactivate();
        gameUI.EnableControls(false);
    }

    protected virtual void ReportTurnEnd(bool timeFoul)
    {
        poolManager.OnTurnTaken(this, timeFoul);
    }

    protected virtual void SetCueBallPosition(Vector3 position)
    {
        cueBall.transform.position = position;
    }

    public void SetTurnState()
    {
        UnDimUI();
    }

    public void SetWaitState()
    {
        DimUI();
        ResetTimer();
    }

    protected virtual void DimUI()
    {
        ui.DimUI();
    }

    protected virtual void UnDimUI()
    {
        ui.UnDimUI();
    }

    protected virtual void ResetTimer()
    {
        ui.timer.TimerImage.fillAmount = 0;
    }

    public virtual void UpdateBallImages(int[] ballNumbers)
    {
        System.Array.Sort(ballNumbers);

        for (int i = 0; i < ui.ballImages.Count; i++)
        {
            if (i < ballNumbers.Length)
            {
                ui.ballImages[i].sprite = gameUI.GetBallSprite(ballNumbers[i]);
            }
            else
            {
                ui.ballImages[i].sprite = gameUI.GetEmptyBallSprite();
            }
        }
    }

    protected void PlaceCueBallAfterFoul(bool behindHeadString)
    {
        Vector3 cueBallPosition = poolManager.PoolTable.CenterPoint.position;
        if (behindHeadString)
        {
            cueBallPosition = poolManager.PoolTable.HeadSpot.position;
        }

        while (poolManager.BallOverlapsOtherBalls(poolManager.CueBall, cueBallPosition, collisionCheckOffset))
        {
            float deltaZ = poolManager.CueBall.Radius * 2;
            deltaZ = deltaZ + (deltaZ * collisionCheckOffset);

            cueBallPosition.z += deltaZ;
        }

        SetCueBallPosition(cueBallPosition);
    }

    public virtual void StopTimerSound()
    {
        ui.timer.StopTickSound();
    }

}
