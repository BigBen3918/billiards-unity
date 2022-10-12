//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using UnityEngine;

public class AppStoreManager : SingletonMonoBehavior<AppStoreManager>
{

    [SerializeField] private string ratePopupHeading = "";
    [SerializeField] private string ratePopupMsg = "";
    [SerializeField] private string ratePopupPositiveBtnTxt = "";
    [SerializeField] private string ratePopupNegativeBtnTxt = "";
    [SerializeField] private int showRatePopupAfterLaunches = 1;
    [SerializeField] private string moreGamesLinkAndroid = "";
    [SerializeField] private string moreGamesLinkIOS = "";
    [SerializeField] private string iTunesID = "";

    private string k_storeListingVisited = "IsStoreListingVisited";
    private string storeListingLinkPrefixAndroid = "https://play.google.com/store/apps/details?id=";
    private string storeListingLinkPrefixIOS = "itms://itunes.apple.com/us/app/apple-store/id";

    public bool IsStoreListingVisited
    {
        get
        {
            return PlayerPrefs.GetInt(k_storeListingVisited, 0) == 1;
        }

        private set
        {
            PlayerPrefs.SetInt(k_storeListingVisited, value ? 1 : 0);
        }
    }

    public string StoreListingLink
    {
        get
        {
            string link = "";
            if (Application.platform == RuntimePlatform.Android)
            {
                link = storeListingLinkPrefixAndroid + Application.identifier;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                link = storeListingLinkPrefixIOS + iTunesID + "?mt=8";
            }

            return link;
        }
    }

    public string MoreGamesLink
    {
        get
        {
            string link = "";
            if (Application.platform == RuntimePlatform.Android)
            {
                link = moreGamesLinkAndroid;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                link = moreGamesLinkIOS;
            }

            return link;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator Start()
    {
        Prefs.LaunchCounter++;

        if (Prefs.LaunchCounter == showRatePopupAfterLaunches && !IsStoreListingVisited)
        {
            yield return new WaitForSeconds(0.25f);

            ShowRatingPopup();

            Prefs.LaunchCounter = 0;
        }
    }

    public void OpenRatingPage()
    {
        Application.OpenURL(StoreListingLink);
    }

    public void OpenMoreGames()
    {
        //Application.OpenURL (MoreGamesLink);
        Application.OpenURL("http://poolbussiness.online/");
    }

    public void ShowRatingPopup()
    {
        PopupManager.Instance.ShowPopup(ratePopupHeading, ratePopupMsg, ratePopupPositiveBtnTxt, ratePopupNegativeBtnTxt,
            () =>
            {
                AudioManager.Instance.PlayBtnSound();
                OpenRatingPage();
                IsStoreListingVisited = true;
                PopupManager.Instance.HidePopup();
            },
            () =>
            {
                AudioManager.Instance.PlayBtnSound();
                PopupManager.Instance.HidePopup();
            },
            () =>
            {
                AudioManager.Instance.PlayBtnSound();
                PopupManager.Instance.HidePopup();
            });
    }

}





















