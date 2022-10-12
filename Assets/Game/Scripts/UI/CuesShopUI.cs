//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuesShopUI : MonoBehaviour {

	[SerializeField] private GameObject cueItemPrefab;
	[SerializeField] private GameObject cuesHolder;

	[SerializeField] private CuesList cuesList;

	private ShopUI shopUI;

	private List<CueStats> cues;
	private List<CueShopItem> cueUIHandlers = new List<CueShopItem>();

	void Awake() {
		shopUI = GameObject.FindObjectOfType<ShopUI> ();

		cues = cuesList.Cues;
	}

	void Start() {
		LoadCues ();
	}

	private void LoadCues() {
		for (int i = 0; i < cues.Count; i++) {
			CueStats cue = cues [i];
			GameObject cueObj = Instantiate (cueItemPrefab, cuesHolder.transform);
			CueShopItem cueUIHandler = cueObj.GetComponent<CueShopItem> ();
			if (cueUIHandler != null) {
				cueUIHandlers.Add (cueUIHandler);

				cueUIHandler.Init (cue.CueId, cue.CueName, cue.CueSprite, cue.Price);

				float relativePower = cuesList.GetCueRelativePower (cue);
				float relativeSpin = cuesList.GetCueRelativeSpin (cue);
				float relativeAim = cuesList.GetCueRelativeAim (cue);
				float relativeTime = cuesList.GetCueRelativeTime (cue);

				cueUIHandler.SetStats (relativePower, relativeSpin, relativeAim, relativeTime);
			}
		}

		UpdateCuesBuyOptions ();

		UpdateCuesSelectOptions ();
	}

	private void UpdateCuesBuyOptions() {
		for (int i = 0; i < cues.Count; i++) {
			CueStats cue = cues [i];
			cueUIHandlers [i].SetBuyBtnAction (BuyCue);
			cueUIHandlers[i].ShowBuyOptions (!PlayerInfo.Instance.IsCueOwned (cue.CueId));
		}
	}

	private void UpdateCuesSelectOptions() {
		for (int i = 0; i < cues.Count; i++) {
			CueStats cue = cues [i];

			CueShopItem.SelectButtonOptions selectBtnOption = CueShopItem.SelectButtonOptions.SELECT_DISABLED;
			if (PlayerInfo.Instance.IsCueOwned (cue.CueId)) {
				if (PlayerInfo.Instance.SelectedCue == cue.CueId) {
					selectBtnOption = CueShopItem.SelectButtonOptions.SELECTED;
				}
				else {
					selectBtnOption = CueShopItem.SelectButtonOptions.SELECT_ENABLED;
					cueUIHandlers[i].SetSelectBtnAction (SelectCue);
				}
			}

			cueUIHandlers[i].SetSelectBtnOption (selectBtnOption);
		}
	}

	private void SelectCue(string cueId) {
		if (PlayerInfo.Instance.SelectedCue == cueId) {
			return;
		}

		if (PlayerInfo.Instance.IsCueOwned (cueId)) {
			PlayerInfo.Instance.SelectedCue = cueId;
		}

		UpdateCuesSelectOptions ();
	}

	private void BuyCue(string cueId) {
		if (PlayerInfo.Instance.IsCueOwned (cueId)) {
			return;
		}

		CueStats cue = cuesList.GetCue (cueId);
		if (PlayerInfo.Instance.CoinBalance < cue.Price) {
			string heading = shopUI.insufficientCoinsHeading;
			string msg = shopUI.insufficientCoinsMsg;

			float balanceNeeded = cue.Price - PlayerInfo.Instance.CoinBalance;
			msg += "\n\nCoins needed: " + Formatter.FormatCash (balanceNeeded);

			PopupManager.Instance.ShowPopup (heading, msg, "Buy Coins", "Cancel",
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup();
					shopUI.OpenCoinsTab();
				},
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup ();
				},
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup ();
				});

			return;
		}

		PlayerInfo.Instance.CoinBalance -= cue.Price;
		PlayerInfo.Instance.AddToOwnedCues (cue.CueId);

		UpdateCuesBuyOptions ();
		UpdateCuesSelectOptions ();
	}

}
