//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using UnityEngine;

public class Startup : MonoBehaviour {

	[SerializeField] private float initialCoinBalance = 0;
	[SerializeField] private string defaultAvatarId = "";
	[SerializeField] private string defaultCueId = "";
	[SerializeField] private string defaultTableId = "";

	IEnumerator Start() {
		yield return new WaitForEndOfFrame ();

		if (!PlayerInfo.Instance.IsInfoSetup) {
			PlayerInfo.Instance.PlayerName = "PLAYER " + Random.Range (0, 10000001);

			PlayerInfo.Instance.PlayerAvatarId = defaultAvatarId;

			PlayerInfo.Instance.CoinBalance = initialCoinBalance;

			PlayerInfo.Instance.AddToOwnedCues (defaultCueId);
			PlayerInfo.Instance.SelectedCue = defaultCueId;

			PlayerInfo.Instance.AddToOwnedTables (defaultTableId);
			PlayerInfo.Instance.SelectedTable = defaultTableId;

			PoolSceneManager.Instance.GoToEditProfile ();
		}
		else {
			PoolSceneManager.Instance.MyLoadScene ("MainScreen");
		}
	}

}
