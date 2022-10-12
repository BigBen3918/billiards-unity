//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Api;

public class AdmobHelper : MonoBehaviour
{

    [SerializeField] private string androidAppId;
    [SerializeField] private string androidBannerId;
    [SerializeField] private string androidInterstitialId;

    [SerializeField] private string iosAppId;
    [SerializeField] private string iosBannerId;
    [SerializeField] private string iosInterstitialId;

    private InterstitialAd interstitial;

    void Start()
    {
        Initialize();

        RequestInterstitial();
    }

    private void Initialize()
    {
#if UNITY_ANDROID
        string appId = androidAppId;
#elif UNITY_IPHONE
		string appId = iosAppId;
#else
		string appId = "unexpected_platform";
#endif

        MobileAds.Initialize(appId);
    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = androidInterstitialId;
#elif UNITY_IPHONE
		string adUnitId = iosInterstitialId;
#else
		string adUnitId = "unexpected_platform";
#endif

        CleanInterstitial();

        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    public bool ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            RequestInterstitial();

            return true;
        }

        return false;
    }

    private void CleanInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
    }

}
