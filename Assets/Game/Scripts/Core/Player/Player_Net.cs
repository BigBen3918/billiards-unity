//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Net : Player {

	private PoolManager_Net pmNet;
	private MatchMakerPlayerUI mmPlayerUI;

	protected override void Awake() {
		base.Awake ();

		pmNet = (PoolManager_Net)poolManager;

		ui = gameUI.player2UI;
		mmPlayerUI = pmNet.preGameUI.otherPlayerUI;
	}

	public override void OnStartLocalPlayer () {
		base.OnStartLocalPlayer ();

		ui = gameUI.player1UI;
		mmPlayerUI = pmNet.preGameUI.thisPlayerUI;
	}

	protected override void Start() {
		base.Start ();

		poolManager.AddPlayer (this);

		if (isLocalPlayer) {
			CmdSpawnCue (PlayerInfo.Instance.SelectedCue);

			string id = netId.ToString ();
			string name = PlayerInfo.Instance.PlayerName;
			string avatarId = PlayerInfo.Instance.PlayerAvatarId;

			CmdSetProfile (id, name, avatarId);
		}
		else {
			mmPlayerUI.StopFindAnim ();

			ui.SetNameTxt (playerName);
			mmPlayerUI.SetName (playerName);

			PlayerAvatar playerAvatar = PlayerInfo.Instance.GetAvatar (playerAvatarId);
			if (playerAvatar != null) {
				ui.SetAvatar (playerAvatar.AvatarSprite);
				mmPlayerUI.SetAvatar (playerAvatar.AvatarSprite);
			}
		}
	}

	protected override void HandleNameChange (string name) {
		base.HandleNameChange (name);

		mmPlayerUI.SetName (name);
	}

	protected override void HandleAvatarUpdate (string avatarId) {
		base.HandleAvatarUpdate (avatarId);

		mmPlayerUI.StopFindAnim ();

		PlayerAvatar playerAvatar = PlayerInfo.Instance.GetAvatar (avatarId);
		if (playerAvatar != null) {
			mmPlayerUI.SetAvatar (playerAvatar.AvatarSprite);
		}
	}

	public override void ReportReady() {
		if (isLocalPlayer) {
			CmdReportReady ();
		}
	}

	public override void TakeBallInHand(bool behindHeadString = false) {
		if (isLocalPlayer) {
			base.TakeBallInHand (behindHeadString);
		}
	}

	public override void EndBallInHand() {
		if (isLocalPlayer) {
			base.EndBallInHand ();
		}
	}

	public override void EndTurn(bool timeFoul = false) {
		if (isLocalPlayer) {
			base.EndTurn (timeFoul);
		}
	}

	protected override void ReportTurnEnd (bool timeFoul) {
		if (isLocalPlayer) {
			CmdReportTurnEnd (timeFoul);
		}
	}

	protected override void DimUI () {
		RpcDimUI ();
	}

	protected override void UnDimUI() {
		RpcUnDimUI ();
	}

	protected override void ResetTimer() {
		RpcResetTimer ();
	}

	public override void UpdateBallImages(int[] ballNumbers) {
		RpcUpdateBallImages (ballNumbers);
	}

	public override void StopTimerSound() {
		RpcStopTimerSound ();
	}

	private void EndGame(Player winner) {
		if (!poolManager.IsBreakDone) {
			poolManager.ConcludeGame (null);
		}
		else {
			poolManager.ConcludeGame (winner);
		}
	}

	#region COMMANDS

	[Command]
	private void CmdSpawnCue(string cueId) {
		cue = Instantiate (cuePrefab);
		cue.GetComponent<CueController> ().ownerNetId = this.netId;
		cue.GetComponent<CueController_Net> ().cueId = cueId;

		NetworkServer.SpawnWithClientAuthority (cue, connectionToClient);
	}

	[Command]
	private void CmdSetProfile(string id, string name, string avatarId) {
		playerId = id;
		playerName = name;
		playerAvatarId = avatarId;
	}

	[Command]
	private void CmdReportTurnEnd(bool timeFoul) {
		base.ReportTurnEnd (timeFoul);
	}

	[Command]
	public void CmdReportReady() {
		base.ReportReady ();
	}

	[Command]
	private void CmdSetCueBallPosition(Vector3 position) {
		base.SetCueBallPosition (position);
	}

	#endregion

	#region RPCS

	[ClientRpc]
	private void RpcDimUI() {
		base.DimUI ();
	}

	[ClientRpc]
	private void RpcUnDimUI() {
		base.UnDimUI ();
	}

	[ClientRpc]
	public void RpcResetTimer() {
		base.ResetTimer ();
	}

	[ClientRpc]
	public void RpcUpdateBallImages(int[] ballNumbers) {
		base.UpdateBallImages (ballNumbers);
	}

	[ClientRpc]
	private void RpcStopTimerSound() {
		base.StopTimerSound ();
	}

	#endregion

}
