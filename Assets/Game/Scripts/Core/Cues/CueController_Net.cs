//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CueController_Net : CueController {

	[SyncVar]
	public string cueId = "";

	protected override void Start() {
		base.Start ();

		owner = ClientScene.FindLocalObject (ownerNetId).GetComponent<Player> ();
		owner.CueController = this;

		SetStats (cueId);

		if (owner.isLocalPlayer) {
			LookAt (poolManager.PoolTable.FootSpot.position);
			Deactivate ();

			owner.ReportReady ();
		}
		else {
			SetCueOpacity (disabledOpacity);
		}
	}

	protected override void Update() {
		if (!isActive) {
			return;
		}

		DrawGuideLine ();

		if (!hasAuthority) {
			return;
		}

		base.Update ();
	}

	public override void Activate() {
		if (!hasAuthority) {
			return;
		}

		base.Activate ();
		CmdActivate ();
	}

	public override void Deactivate() {
		if (!hasAuthority) {
			return;
		}

		base.Deactivate ();
		CmdDeactivate ();
	}

	protected override void OnSliderReleased() {
		if (!hasAuthority) {
			return;
		}

		base.OnSliderReleased ();
	}

	protected override void OnSpinKnobBtnReleased() {
		if (!hasAuthority) {
			return;
		}

		ShowSpinMenu ();
	}

	protected override void OnSpinKnobDragged() {
		if (!hasAuthority) {
			return;
		}

		SetKnobPosition (spinKnob.Input);
	}

	protected override void OnSpinKnobReleased() {
		if (!hasAuthority) {
			return;
		}

		HideSpinMenu ();
	}

	protected override void OnSpinMenuReleased() {
		if (!hasAuthority) {
			return;
		}

		HideSpinMenu ();
	}

	protected override void HideSpinMenu() {
		base.HideSpinMenu ();
		CmdHideSpinMenu ();
	}

	protected override void SetKnobPosition(Vector2 position) {
		base.SetKnobPosition (position);
		CmdSetKnobPosition (position);
	}

	public override void HitCueBall (Vector3 force, Vector3 angularVelocity) {
		CmdHitCueBall (force, angularVelocity);
	}

	protected override void PlayHitSound() {
		RpcPlayHitSound ();
	}

	protected override void ShowSpinMenu() {
		base.ShowSpinMenu ();
		CmdShowSpinMenu ();
	}

	#region COMMANDS

	[Command]
	private void CmdActivate() {
		RpcActivate ();
	}

	[Command]
	private void CmdDeactivate() {
		RpcDeactivate ();
	}

	[Command]
	private void CmdShowSpinMenu() {
		RpcShowSpinMenu ();
	}

	[Command]
	private void CmdHideSpinMenu() {
		RpcHideSpinMenu ();
	}

	[Command]
	private void CmdSetKnobPosition(Vector2 position) {
		RpcSetKnobPosition (position);
	}

	[Command]
	private void CmdHitCueBall(Vector3 force, Vector3 angularVelocity) {
		NetworkIdentity cueBallNetIdentity = cueBall.GetComponent<NetworkIdentity> ();
		if (cueBallNetIdentity.clientAuthorityOwner == owner.connectionToClient) {
			cueBallNetIdentity.RemoveClientAuthority (owner.connectionToClient);
		}

		base.HitCueBall (force, angularVelocity);
	}

	#endregion

	#region RPCS

	[ClientRpc]
	private void RpcActivate() {
		if (hasAuthority) {
			return;
		}

		base.Activate ();
	}

	[ClientRpc]
	private void RpcDeactivate() {
		if (hasAuthority) {
			return;
		}

		base.Deactivate ();
	}

	[ClientRpc]
	private void RpcShowSpinMenu() {
		if (hasAuthority) {
			return;
		}

		base.ShowSpinMenu ();
	}

	[ClientRpc]
	private void RpcHideSpinMenu() {
		if (hasAuthority) {
			return;
		}

		base.HideSpinMenu ();
	}

	[ClientRpc]
	private void RpcSetKnobPosition(Vector2 position) {
		if (hasAuthority) {
			return;
		}

		base.SetKnobPosition (position);
	}

	[ClientRpc]
	private void RpcPlayHitSound() {
		base.PlayHitSound ();
	}

	#endregion

}
