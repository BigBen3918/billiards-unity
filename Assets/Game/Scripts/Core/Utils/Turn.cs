//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn {

	public Turn(Player player, bool isBreak) {
		SetPlayer (player);
		IsBreak = isBreak;
		BallsHitByCueBall = new List<int> ();
		CushionedBalls = new List<int> ();
		PocketedBalls = new List<int> ();
	}

	public Player Player {
		get;
		private set;
	}

	public bool IsBreak {
		get;
		private set;
	}

	public List<int> BallsHitByCueBall {
		get;
		private set;
	}

	public List<int> CushionedBalls {
		get;
		private set;
	}

	public List<int> PocketedBalls {
		get;
		private set;
	}

	public MoveType MoveType {
		get;
		private set;
	}

	public bool IsCueBallPocketed {
		get {
			return PocketedBalls.Contains (0);
		}
	}

	public bool IsEightBallPocketed {
		get {
			return PocketedBalls.Contains (8);
		}
	}

	public bool BallsHitRailsAfterContact {
		get;
		set;
	}

	public void SetPlayer(Player player) {
		Player = player;
	}

	public void AddToBallsHitByCueBall(Ball ball) {
		if (!BallsHitByCueBall.Contains (ball.BallNumber)) {
			BallsHitByCueBall.Add (ball.BallNumber);
		}
	}

	public void AddToCushionedBalls(Ball ball) {
		if (!CushionedBalls.Contains (ball.BallNumber)) {
			CushionedBalls.Add (ball.BallNumber);
		}
	}

	public void AddToPocketedBalls(Ball ball) {
		if (!PocketedBalls.Contains (ball.BallNumber)) {
			PocketedBalls.Add (ball.BallNumber);
		}
	}

	public void SetMoveType(MoveType type) {
		MoveType = type;
	}

}
