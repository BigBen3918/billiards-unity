//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.Networking;

public class Ball_Net : Ball {

	public override void OnStartClient () {
		base.OnStartClient ();

		GameManager.AddBall (BallNumber, this);
	}

	protected override void Update() {
		if (!isServer) {
			return;
		}

		base.Update ();
	}

	void OnCollisionEnter(Collision col) {
		HandleCollisionEnter (col, isServer);
	}

	void OnTriggerEnter(Collider col) {
		HandleTriggerEnter (col, isServer);
	}

	void OnDisable() {
		GameManager.RemoveBall (BallNumber);
	}

	protected override void SetLayer (int layerNumber) {
		RpcSetLayer (layerNumber);
	}

	protected override void SetGravity (bool gravityState) {
		RpcSetGravity (gravityState);
	}

	protected override void Sleep() {
		RpcSleep ();
	}

	protected override void StartCollectorSound() {
		RpcStartCollectorSound ();
	}

	protected override void StopCollectorSound () {
		RpcStopCollectorSound ();
	}

	protected override void PlayBallReachedSound() {
		RpcPlayBallReachedSound ();
	}

	#region RPCS

	[ClientRpc]
	public void RpcSetLayer(int layerNumber) {
		base.SetLayer (layerNumber);
	}

	[ClientRpc]
	public void RpcSetGravity(bool gravityState) {
		base.SetGravity (gravityState);
	}

	[ClientRpc]
	public void RpcSleep() {
		base.Sleep ();
	}

	[ClientRpc]
	private void RpcPlayBallReachedSound() {
		base.PlayBallReachedSound ();
	}

	[ClientRpc]
	private void RpcStartCollectorSound() {
		base.StartCollectorSound ();
	}

	[ClientRpc]
	private void RpcStopCollectorSound() {
		base.StopCollectorSound ();
	}

	#endregion

}
