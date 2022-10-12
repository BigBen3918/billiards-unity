//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;

public class Prefs {
	
	private static string k_playerName = "PlayerName";
	private static string k_playerAvatarId = "PlayerAvatarId";
	private static string k_soundState = "SoundState";

	private static string k_cueOwnStatus = "CueOwnStatus";
	private static string k_selectedCue = "SelectedCue";

	private static string k_tableOwnStatus = "TableOwnStatus";
	private static string k_selectedTable = "SelectedTable";

	private static string k_coinBalance = "CoinBalance";

	private static string k_noAds = "NoAdsPurchasedState";

	private static string k_launchCounter = "LaunchCounter";

	public static string PlayerName {
		get {
			string defaultName = "";
			return PlayerPrefs.GetString (k_playerName, defaultName);
		}

		set {
			PlayerPrefs.SetString (k_playerName, value);
		}
	}

	public static string PlayerAvatarId {
		get {
			return PlayerPrefs.GetString (k_playerAvatarId, "");
		}

		set {
			PlayerPrefs.SetString (k_playerAvatarId, value);
		}
	}

	public static bool IsSoundOn {
		get {
			return PlayerPrefs.GetInt (k_soundState, 1) == 1;
		}

		set {
			PlayerPrefs.SetInt (k_soundState, value ? 1 : 0);
		}
	}

	public static string SelectedCue {
		get {
			return PlayerPrefs.GetString (k_selectedCue, "");
		}

		set {
			PlayerPrefs.SetString (k_selectedCue, value);
		}
	}

	public static string SelectedTable {
		get {
			return PlayerPrefs.GetString (k_selectedTable, "");
		}

		set {
			PlayerPrefs.SetString (k_selectedTable, value);
		}
	}

	public static float CoinBalance {
		get {
			return PlayerPrefs.GetFloat (k_coinBalance, 0);
		}

		set {
			PlayerPrefs.SetFloat (k_coinBalance, value);
		}
	}

	public static bool IsNoAdsPurchased {
		get {
			return PlayerPrefs.GetInt (k_noAds, 0) == 1;
		}

		set {
			PlayerPrefs.SetInt (k_noAds, value ? 1 : 0);
		}
	}

	public static int LaunchCounter {
		get {
			return PlayerPrefs.GetInt (k_launchCounter, 0);
		}

		set {
			PlayerPrefs.SetInt (k_launchCounter, value);
		}
	}

	public static bool IsCueOwned(string cueId) {
		return PlayerPrefs.GetInt (k_cueOwnStatus + cueId, 0) == 1;
	}

	public static void SetCueOwnStatus(string cueId, bool owned) {
		PlayerPrefs.SetInt (k_cueOwnStatus + cueId, owned ? 1 : 0);
	}

	public static bool IsTableOwned(string tableId) {
		return PlayerPrefs.GetInt (k_tableOwnStatus + tableId, 0) == 1;
	}

	public static void SetTableOwnStatus(string tableId, bool owned) {
		PlayerPrefs.SetInt (k_tableOwnStatus + tableId, owned ? 1 : 0);
	}

}
