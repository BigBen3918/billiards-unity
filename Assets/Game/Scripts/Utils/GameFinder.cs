//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class GameFinder : SingletonMonoBehavior<GameFinder> {

	private CustomNetworkManager networkManager;

	protected override void Awake () {
		base.Awake ();
		DontDestroyOnLoad (this.gameObject);
	}

	public void StartMatchMaking() {
		networkManager = GameObject.FindObjectOfType<CustomNetworkManager> ();
		if (networkManager == null) {
			return;
		}

		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker ();
		}

		ListMatches ();
	}

	private void ListMatches() {
		NetworkManager networkManager = NetworkManager.singleton;

		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker ();
		}

		networkManager.matchMaker.ListMatches (0, 10, "", false, 0, 0, OnMatchList);
	}

	private void HostMatch() {
		NetworkManager networkManager = NetworkManager.singleton;

		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker ();
		}

		networkManager.matchMaker.CreateMatch ("", 2, true, "", "", "", 0, 0, OnMatchCreate);
	}

	private void JoinMatch(MatchInfoSnapshot match) {
		NetworkManager networkManager = NetworkManager.singleton;

		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker ();
		}

		networkManager.matchMaker.JoinMatch (match.networkId, "", "", "", 0, 0, OnMatchJoined);
	}

	private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> responseData) {
		NetworkManager.singleton.OnMatchList (success, extendedInfo, responseData);

		if (!success) {
			return;
		}

		if (responseData == null || responseData.Count == 0) {
			HostMatch ();

			return;
		}

		MatchInfoSnapshot match = responseData [responseData.Count - 1];
		JoinMatch (match);
	}

	private void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
		NetworkManager.singleton.OnMatchCreate (success, extendedInfo, matchInfo);

		if (!success) {
			return;
		}
	}

	private void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo) {
		NetworkManager.singleton.OnMatchJoined (success, extendedInfo, matchInfo);

		if (!success) {
			return;
		}
	}

}
