//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;

public class Player_Local : Player {

	protected override void Start() {
		base.Start ();

		poolManager.AddPlayer (this);

		if (playerId == "1") {
			ui = gameUI.player1UI;
		}
		else if (playerId == "2") {
			ui = gameUI.player2UI;
		}

		PlayerAvatar avatar;
		if (!PoolManager_Local.isAgainstAI) {
			playerName = "PLAYER " + playerId;
			avatar = PlayerInfo.Instance.DefaultAvatar;
		}
		else {
			playerName = PlayerInfo.Instance.PlayerName;
			avatar = PlayerInfo.Instance.SelectedAvatar;
		}

		ui.SetNameTxt (playerName);
		ui.SetAvatar (avatar.AvatarSprite);

		CreateCue ();
	}

	public override void TakeTurn () {
		base.TakeTurn ();

		inputManager.Unlock ();
	}

	public override void EndTurn (bool timeFoul = false) {
		base.EndTurn (timeFoul);

		inputManager.Lock ();
		gameUI.cueSlider.ResetSlider ();
	}

	private void CreateCue() {
		cue = Instantiate (cuePrefab);
		CueController = cue.GetComponent<CueController> ();
		CueController.owner = this;

		CueController.SetStats (PlayerInfo.Instance.SelectedCue);
	}

}
