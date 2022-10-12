//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

public class CustomNetworkManager : NetworkManager {

	private PoolManager poolManager;

	void OnApplicationPause(bool paused) {
		if (paused) {
			PoolManager_Net pm = GameObject.FindObjectOfType<PoolManager_Net> ();
			if (pm != null && pm.TurnCount > 0) {
				CleanAndStopHost ();

				PoolSceneManager.Instance.GoToMainMenu ();
			}
		}
	}

	#region SERVER_CALLBACKS

	public override void OnServerDisconnect (NetworkConnection conn) {
		HandleDisconnection (() => {
			base.OnServerDisconnect (conn);
		});
	}

	#endregion

	#region CLIENT_CALLBACKS

	public override void OnClientDisconnect (NetworkConnection conn) {
		HandleDisconnection (() => {
			base.OnClientDisconnect (conn);
		});
	}

	#endregion

	public void CleanAndStopHost() {
		if (PoolManager_Net.isOnline) {
			StopHost();
		}
		else {
			NetworkDiscovery_LAN networkDiscovery = GameObject.FindObjectOfType<NetworkDiscovery_LAN> ();
			if (networkDiscovery != null) {
				networkDiscovery.StopServer ();
			}

			StopHost();
		}
	}

	public void ShowDisconnectPopup() {
		string popupHeading = "Disconnected";
		string popupMsg = "You were disconnected. This might have happened because:\n" +
			"\n - Your opponent left the match." +
			"\n - There was an issue with your network.";

		PopupManager.Instance.ShowPopup (popupHeading, popupMsg, "OK", "",
			() => {
				CleanAndStopHost();

				AudioManager.Instance.PlayBtnSound ();

				if (PoolManager_Net.isOnline) {
					PoolSceneManager.Instance.GoToMainMenu ();
				}
				else {
					PoolSceneManager.Instance.GoToLanLobby ();
				}

			},
			null, null);
	}

	private void HandleDisconnection(Action callback) {
		poolManager = GameObject.FindObjectOfType<PoolManager> ();
		if (poolManager == null) {
			callback ();

			return;
		}

		poolManager.StopAllCoroutines ();

		if (PoolManager_Net.isOnline) {
			if (!poolManager.IsBreakDone) {
				poolManager.ConcludeGame (null);

				callback ();
			}
			else {
				CheckInternetConnection ((bool available) => {

					Player winner = null;

					if (available) {
						winner = poolManager.GetLocalPlayer ();
					}
					else {
						winner = poolManager.GetOtherPlayer (poolManager.GetLocalPlayer ());
					}

					poolManager.ConcludeGame (winner);

					callback();
				});
			}
		}
		else {
			ShowDisconnectPopup ();

			callback ();
		}
	}

	private void CheckInternetConnection(Action<bool> checkCompleteCallback) {
		StartCoroutine (CheckInternetCo (checkCompleteCallback));
	}

	private IEnumerator CheckInternetCo(Action<bool> checkCompleteCallback) {
		string testUrl = "www.google.com";
		WWW www = new WWW (testUrl);

		yield return www;

		if (string.IsNullOrEmpty(www.error)) {
			checkCompleteCallback (true);
		}
		else {
			checkCompleteCallback (false);
		}

	}

}
