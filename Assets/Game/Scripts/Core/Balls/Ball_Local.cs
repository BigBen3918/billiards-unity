//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;

public class Ball_Local : Ball {

	protected override void Awake() {
		base.Awake ();

		GameManager.AddBall (BallNumber, this);
	}

	protected override void Update () {
		base.Update ();
	}

	void OnCollisionEnter(Collision col) {
		HandleCollisionEnter (col, true);
	}

	void OnTriggerEnter(Collider col) {
		HandleTriggerEnter (col, true);
	}

	void OnDisable() {
		GameManager.RemoveBall (BallNumber);
	}

}
