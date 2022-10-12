//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AI : Player {

	[SerializeField] private float cueRotationSpeedNormal = 1;
	[SerializeField] private float cueRotationSpeedAccuracy = 1;

	private Coroutine turnCo;
	private bool shouldFindTarget = false;

	private float cueRotationSpeed = 0;

	protected override void Awake() {
		base.Awake ();

		cueRotationSpeed = cueRotationSpeedNormal;
	}

	protected override void Start() {
		base.Start ();

		poolManager.AddPlayer (this);

		if (playerId == "1") {
			ui = gameUI.player1UI;
		}
		else if (playerId == "2") {
			ui = gameUI.player2UI;
		}

		playerName = "BOT";
		ui.SetNameTxt (playerName);

		ui.SetAvatar (PlayerInfo.Instance.BotAvatar.AvatarSprite);

		CreateCue ();
		CueController.SetCueOpacity (CueController.disabledOpacity);
	}

	public override void TakeTurn () {
		base.TakeTurn ();

		inputManager.Lock ();
		gameUI.EnableControls (false);

		turnCo = StartCoroutine (TurnCo ());
	}

	public override void EndTurn (bool timeFoul = false) {
		shouldFindTarget = false;

		if (turnCo != null) {
			StopCoroutine (turnCo);
		}

		base.EndTurn (timeFoul);
	}

	public override void EndBallInHand () {
		HasBallInHand = false;
	}

	protected override IEnumerator BallInHandCo (bool behindHeadString) {
		base.PlaceCueBallAfterFoul (behindHeadString);

		yield return null;
	}

	private void CreateCue() {
		cue = Instantiate (cuePrefab);
		CueController = cue.GetComponent<CueController> ();
		CueController.owner = this;

		CueController.SetStats (PlayerInfo.Instance.SelectedCue);
	}

	private IEnumerator TurnCo() {
		if (poolManager.CurrentTurn.IsBreak) {
			yield return new WaitForSeconds (2.0f);

			float randomStrengthBreak = Random.Range (0.8f, 1.0f);
			TakeShot (randomStrengthBreak);

			yield break;
		}

		yield return new WaitForSeconds (1.0f);

		yield return StartCoroutine (FindTargetCo ());

		yield return new WaitForSeconds (1.0f);

		float randomStrength = Random.Range (0.4f, 0.6f);
		TakeShot (randomStrength);
	}

	private void TakeShot(float strength) {
		Vector3 direction = CueController.transform.forward;
		float magnitude = Mathf.Lerp (CueController.MinStrength, CueController.MaxStrength, strength);
		Vector3 force = direction * magnitude;

		CueController.HitCueBall (force, Vector3.zero);

		EndTurn (false);
	}

	private IEnumerator FindTargetCo() {
		Ball cueBall = poolManager.CueBall;

		Ray ray;
		float radius = cueBall.Radius;
		RaycastHit hit;

		float totalAngleRotated = 0;
		int roundTripCount = 0;

		int rotationDir = 1;
		if (Random.Range (0, 2) == 0) {
			rotationDir = -1;
		}

		shouldFindTarget = true;
		while (shouldFindTarget) {
			cueRotationSpeed = cueRotationSpeedNormal;

			ray = new Ray (cueBall.transform.position, CueController.transform.forward);

			if (Physics.SphereCast (ray, radius, out hit, 100)) {
				if (hit.collider.CompareTag ("Ball")) {
					if (roundTripCount == 2) {
						yield break;
					}

					Ball targetBall = hit.collider.GetComponent<Ball> ();

					bool isBallAllowed = poolManager.IsBallAllowed (this, targetBall);

					if (AssignedBalls == BallType.UNKNOWN) {
						int solidsPocketed = poolManager.GetBallsOfType (poolManager.PocketedBalls, BallType.SOLID).Count;
						int stripesPocketed = poolManager.GetBallsOfType (poolManager.PocketedBalls, BallType.STRIPED).Count;
						if (solidsPocketed > stripesPocketed) {
							isBallAllowed = targetBall.Type == BallType.SOLID;
						}
						else if (stripesPocketed > solidsPocketed) {
							isBallAllowed = targetBall.Type == BallType.STRIPED;
						}
					}

					if (isBallAllowed) {
						cueRotationSpeed = cueRotationSpeedAccuracy;

						if (roundTripCount == 1) {
							yield break;
						}

						RaycastHit pocketHit;
						if (Physics.SphereCast (hit.point, radius, -hit.normal, out pocketHit, 50)) {
							if (pocketHit.collider.CompareTag ("AIPocketDetector")) {
								yield break;
							}
						}
					}
				}
			}

			float angle = Time.deltaTime * cueRotationSpeed;
			CueController.RotateCue (angle * rotationDir);

			totalAngleRotated += angle;
			if (totalAngleRotated >= 360.0f) {
				roundTripCount++;
				totalAngleRotated = 0;
			}

			yield return new WaitForEndOfFrame ();
		}
	}

}
