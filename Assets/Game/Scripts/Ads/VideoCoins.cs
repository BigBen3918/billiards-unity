//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.UI;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class VideoCoins : MonoBehaviour {

	[SerializeField] private Button watchBtn;

	public float coinsToAdd = 0;

	public string videoNotRdyHeading = "";
	public string videoNotRdyMsg = "";
	public string videoSkippedHeading = "";
	public string videoSkippedMsg = "";
	public string videoErrorHeading = "";
	public string videoErrorMsg = "";

	void Awake() {
		#if !UNITY_ADS
		Destroy(this.gameObject);
		#endif
	}

	void OnEnable() {
		#if UNITY_ADS

		if (Advertisement.IsReady ("rewardedVideo")) {
			watchBtn.interactable = true;
		}
		else {
			watchBtn.interactable = false;
		}

		#endif
	}

	public void WatchBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		ShowRewardedVideo ();
	}

	private void ShowRewardedVideo() {
		#if UNITY_ADS

		if (Advertisement.IsReady ("rewardedVideo")) {
			var options = new ShowOptions {
				resultCallback = HandleRewardedVideo
			};
			Advertisement.Show ("rewardedVideo", options);
		}
		else {
			PopupManager.Instance.ShowPopup (videoNotRdyHeading, videoNotRdyMsg, "OK", "",
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup();
				}, null, null);
		}

		#endif
	}

	#if UNITY_ADS
	private void HandleRewardedVideo(ShowResult result) {
		if (result == ShowResult.Finished) {
			PlayerInfo.Instance.CoinBalance += coinsToAdd;

			string videoCompletedHeading = "+ " + Formatter.FormatCash(coinsToAdd) + " Coins!";
			string videoCompletedMsg = "You have received " + Formatter.FormatCash(coinsToAdd) + " coins.\n" +
			                           "\nNew balance: " + Formatter.FormatCash (PlayerInfo.Instance.CoinBalance);

			PopupManager.Instance.ShowPopup (videoCompletedHeading, videoCompletedMsg, "OK", "",
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup ();
				}, null, null);
		}
		else if (result == ShowResult.Skipped) {
			PopupManager.Instance.ShowPopup (videoSkippedHeading, videoSkippedMsg, "OK", "",
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup();
				}, null, null);
		}
		else if (result == ShowResult.Failed) {
			PopupManager.Instance.ShowPopup (videoErrorHeading, videoErrorMsg, "OK", "",
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup();
				}, null, null);
		}
	}
	#endif

}
