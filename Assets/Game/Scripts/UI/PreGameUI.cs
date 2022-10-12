//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PreGameUI : MonoBehaviour {

	public MatchMakerPlayerUI thisPlayerUI;
	public MatchMakerPlayerUI otherPlayerUI;

	void Start() {
		thisPlayerUI.SetName (PlayerInfo.Instance.PlayerName);
		thisPlayerUI.SetAvatar (PlayerInfo.Instance.SelectedAvatar.AvatarSprite);

		otherPlayerUI.StartFindAnim ();
	}

	public void ShowUI() {
		this.gameObject.SetActive (true);
	}

	public void HideUI() {
		this.gameObject.SetActive (false);
	}

	public void BackBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		CustomNetworkManager networkManager = GameObject.FindObjectOfType<CustomNetworkManager> ();
		if (networkManager != null) {
			networkManager.CleanAndStopHost ();
		}

		if (PoolManager_Net.isOnline) {
			//PoolSceneManager.Instance.GoToMainMenu ();
			PoolSceneManager.Instance.MyLoadScene("OnlineMatchMode");
		}
		else {
			PoolSceneManager.Instance.GoToLanLobby ();
		}
	}

}
