//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyUI_LAN : MonoBehaviour {

	[SerializeField] private NetworkDiscovery_LAN networkDiscovery;

	[SerializeField] private GameObject gameItemPrefab;
	[SerializeField] private GameObject gamesHolder;
	[SerializeField] private Text msgTxt;

	[TextArea()]
	[SerializeField] private string noGamesMsg = "No games found.";

	private NetworkManager networkManager;

	void OnEnable() {
		ShowMsg (noGamesMsg);

		networkDiscovery.GamesListUpdated += OnGamesListUpdated;
	}

	void Start() {
		networkManager = NetworkManager.singleton;
	}

	void OnDisable() {
		networkDiscovery.GamesListUpdated -= OnGamesListUpdated;
	}

	public void HostBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		networkDiscovery.StartBroadcast ();

		PoolSceneManager.Instance.StartLanMatch (() => {
			networkManager.StartHost ();
		});
	}

	public void BackBtn_OnClick() {
		networkDiscovery.StopServer ();

		AudioManager.Instance.PlayBtnSound ();
		PoolSceneManager.Instance.GoToMainMenu ();
	}

	private void OnGamesListUpdated(List<LanConnectionInfo> gamesList) {
		if (gamesList.Count == 0) {
			ShowMsg (noGamesMsg);
		}
		else {
			HideMsg ();
		}

		UpdateGamesList (gamesList);
	}

	private void UpdateGamesList(List<LanConnectionInfo> gamesList) {
		for (int i = 0; i < gamesHolder.transform.childCount; i++) {
			Destroy (gamesHolder.transform.GetChild (i).gameObject);
		}

		foreach (LanConnectionInfo gameInfo in gamesList) {
			GameObject gameItemObj = Instantiate (gameItemPrefab, gamesHolder.transform);
			LanGameItem gameItemHandler = gameItemObj.GetComponent<LanGameItem> ();

			if (gameItemHandler != null) {
				gameItemHandler.Init (gameInfo, OnJoinBtnClicked);
			}
		}
	}

	private void OnJoinBtnClicked(LanConnectionInfo gameInfo) {
		AudioManager.Instance.PlayBtnSound ();

		PoolSceneManager.Instance.StartLanMatch (() => {
			networkManager.networkAddress = gameInfo.ipAddress;
			networkManager.networkPort = gameInfo.port;
			networkManager.StartClient ();
		});
	}

	private void ShowMsg(string msg) {
		msgTxt.gameObject.SetActive (true);
		msgTxt.text = msg;
	}

	private void HideMsg() {
		msgTxt.gameObject.SetActive (false);
	}

}
