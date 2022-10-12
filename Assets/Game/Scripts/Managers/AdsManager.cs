//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdsManager : SingletonMonoBehavior<AdsManager> {

	private AdmobHelper admobHelper;

	protected override void Awake() {
		base.Awake ();
		DontDestroyOnLoad (this.gameObject);

		admobHelper = GetComponent<AdmobHelper> ();
	}

	public bool ShowInterstitial() {
		if (PlayerInfo.Instance.IsNoAdsPurchased) {
			return true;
		}

		return admobHelper.ShowInterstitial ();
	}

	public bool ShowVideoAd() {
		if (PlayerInfo.Instance.IsNoAdsPurchased) {
			return true;
		}

		#if UNITY_ADS
		if (Advertisement.IsReady ()) {
			Advertisement.Show ();

			return true;
		}
		#endif

		return false;
	}

}
