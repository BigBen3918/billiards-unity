//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : SingletonMonoBehavior<PlayerInfo> {

	[SerializeField] private AvatarsList avatarsList;
	[SerializeField] private CuesList cuesList;
	[SerializeField] private TablesList tablesList;

	public delegate void CoinBalanceChangeDelegate();
	public event CoinBalanceChangeDelegate CoinBalanceChanged;

	private List<CueStats> cues;
	private List<TableProperties> tables;

	protected override void Awake() {
		base.Awake ();
		DontDestroyOnLoad (this.gameObject);

		cues = cuesList.Cues;
		tables = tablesList.Tables;
	}

	public string PlayerId {
		get {
			return "";
		}
	}

	public string PlayerName {
		get {
			return Prefs.PlayerName;
		}

		set {
			Prefs.PlayerName = value;
		}
	}

	public PlayerAvatar SelectedAvatar {
		get {
			return avatarsList.GetAvatar (PlayerAvatarId);
		}
	}

	public PlayerAvatar DefaultAvatar {
		get {
			return avatarsList.DefaultAvatar;
		}
	}

	public PlayerAvatar BotAvatar {
		get {
			return avatarsList.BotAvatar;
		}
	}

	public float CoinBalance {
		get {
			return Prefs.CoinBalance;
		}

		set {
			Prefs.CoinBalance = value;

			if (CoinBalanceChanged != null) {
				CoinBalanceChanged ();
			}
		}
	}

	public bool IsNoAdsPurchased {
		get {
			return Prefs.IsNoAdsPurchased;
		}

		set {
			Prefs.IsNoAdsPurchased = value;
		}
	}

	public string PlayerAvatarId {
		get {
			return Prefs.PlayerAvatarId;
		}

		set {
			Prefs.PlayerAvatarId = value;
		}
	}

	public List<string> OwnedCues {
		get {
			List<string> ownedCues = new List<string> ();
			for (int i = 0; i < cues.Count; i++) {
				string cueId = cues [i].CueId;
				if (Prefs.IsCueOwned (cueId)) {
					ownedCues.Add (cueId);
				}
			}

			return ownedCues;
		}
	}

	public string SelectedCue {
		get {
			return Prefs.SelectedCue;
		}

		set {
			Prefs.SelectedCue = value;
		}
	}

	public CueStats GetCue(string id) {
		return cuesList.GetCue (id);
	}

	public PlayerAvatar GetAvatar(string id) {
		return avatarsList.GetAvatar (id);
	}

	public CueStats GetSelectedCue() {
		return cuesList.GetCue (SelectedCue);
	}

	public bool IsCueOwned(string cueId) {
		return OwnedCues.Contains (cueId);
	}

	public void AddToOwnedCues(string cueId) {
		Prefs.SetCueOwnStatus (cueId, true);
	}

	public void RemoveFromOwnedCues(string cueId) {
		Prefs.SetCueOwnStatus (cueId, false);
	}

	public List<string> OwnedTables {
		get {
			List<string> ownedTables = new List<string> ();
			for (int i = 0; i < tables.Count; i++) {
				string tableId = tables [i].TableId;
				if (Prefs.IsTableOwned (tableId)) {
					ownedTables.Add (tableId);
				}
			}

			return ownedTables;
		}
	}

	public string SelectedTable {
		get {
			return Prefs.SelectedTable;
		}

		set {
			Prefs.SelectedTable = value;
		}
	}

	public TableProperties GetSelectedTable() {
		return tablesList.GetTable (SelectedTable);
	}

	public bool IsTableOwned(string tableId) {
		return OwnedTables.Contains (tableId);
	}

	public void AddToOwnedTables(string tableId) {
		Prefs.SetTableOwnStatus (tableId, true);
	}

	public void RemoveFromOwnedTables(string tableId) {
		Prefs.SetTableOwnStatus (tableId, false);
	}

	public bool IsInfoSetup {
		get {
			return PlayerName != "";
		}
	}

}
