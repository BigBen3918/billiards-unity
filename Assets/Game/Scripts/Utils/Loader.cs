//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{

    [SerializeField] private GameObject sceneManagerPrefab;
    [SerializeField] private GameObject networkManagerPrefab;
    [SerializeField] private GameObject gameFinderPrefab;
    [SerializeField] private GameObject audioManagerPrefab;
    [SerializeField] private GameObject playerInfoPrefab;
    [SerializeField] private GameObject popupManagerPrefab;
    [SerializeField] private GameObject appStoreManager;
    [SerializeField] private GameObject adsManager;

    void Awake()
    {
        if (PoolSceneManager.Instance == null)
        {
            Instantiate(sceneManagerPrefab);
        }

        if (NetworkManager.singleton == null)
        {
            Instantiate(networkManagerPrefab);
        }

        if (GameFinder.Instance == null)
        {
            Instantiate(gameFinderPrefab);
        }

        if (AudioManager.Instance == null)
        {
            Instantiate(audioManagerPrefab);
        }

        if (PlayerInfo.Instance == null)
        {
            Instantiate(playerInfoPrefab);
        }

        if (PopupManager.Instance == null)
        {
            Instantiate(popupManagerPrefab);
        }

        if (AppStoreManager.Instance == null)
        {
            Instantiate(appStoreManager);
        }

        if (AdsManager.Instance == null)
        {
            Instantiate(adsManager);
        }
    }

}
