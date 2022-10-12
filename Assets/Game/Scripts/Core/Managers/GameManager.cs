//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private const string BALL_PREFIX = "BALL ";

	private static Dictionary<int, Ball> ballsDictionary = new Dictionary<int, Ball>();

	public static int BallsCount {
		get {
			return ballsDictionary.Count;
		}
	}

	public static void AddBall(int ballNumber, Ball ball) {
		ballsDictionary.Add (ballNumber, ball);

		ball.transform.name = BALL_PREFIX + ballNumber;
	}

	public static void RemoveBall(int ballNumber) {
		ballsDictionary.Remove (ballNumber);
	}

	public static Ball GetBall(int ballNumber) {
		return ballsDictionary [ballNumber];
	}

}
