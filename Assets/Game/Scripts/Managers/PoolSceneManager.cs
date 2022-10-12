//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class PoolSceneManager : SingletonMonoBehavior<PoolSceneManager> {

	[SerializeField] private string mainMenuSceneName = "MainMenu";
	[SerializeField] private string editProfileSceneName = "EditProfile";
	[SerializeField] private string shopSceneName = "Shop";
	[SerializeField] private string lanLobbySceneName = "Lobby_LAN";
	[SerializeField] private string localMatchSceneName = "Game_Local";
	[SerializeField] private string onlineMatchSceneName = "Game_Net";

	[SerializeField] private GameObject fader;
	[SerializeField] private AnimationClip fadeInClip;
	[SerializeField] private AnimationClip fadeOutClip;

	private Animator anim;

	protected override void Awake () {
		base.Awake ();
		DontDestroyOnLoad (this.gameObject);

		anim = GetComponent<Animator> ();

		fader.SetActive (false);
	}

	public void GoToMainMenu() {
		LoadScene (mainMenuSceneName);
	}

	public void GoToEditProfile() {
		LoadScene (editProfileSceneName);
	}

	public void GoToShop() {
		LoadScene (shopSceneName);
	}

	public void StartBotMatch() {
		PoolManager_Local.isAgainstAI = true;
		LoadScene (localMatchSceneName);
	}

	public void StartPassNPlayMatch() {
		PoolManager_Local.isAgainstAI = false;
		LoadScene (localMatchSceneName);
	}

	public void GoToLanLobby() {
		PoolManager_Net.isOnline = false;
		LoadScene (lanLobbySceneName);
	}

	public void StartOnlineMatch() {
		PoolManager_Net.isOnline = true;

		LoadScene (onlineMatchSceneName, () => {
			GameFinder.Instance.StartMatchMaking ();
		});
	}

	public void StartLanMatch(Action callback) {
		PoolManager_Net.isOnline = false;
		LoadScene (onlineMatchSceneName, callback);
	}

	public void RestartScene() {
		string currentSceneName = SceneManager.GetActiveScene ().name;
		LoadScene (currentSceneName);
	}

	private void LoadScene(string sceneName, Action callback = null) {
		fader.SetActive (true);

		StartCoroutine (LoadSceneCo (sceneName, callback));
	}
	public void MyLoadScene(string sceneName, Action callback = null)
	{
		fader.SetActive(true);

		StartCoroutine(LoadSceneCo(sceneName, callback));
	}

	private IEnumerator LoadSceneCo(string sceneName, Action callback) {
		Time.timeScale = 0;

		anim.Play (fadeInClip.name);

		yield return new WaitForSecondsRealtime (fadeInClip.length);

		SceneManager.LoadScene (sceneName);

		anim.Play (fadeOutClip.name);

		yield return new WaitForSecondsRealtime (fadeOutClip.length);

		fader.SetActive (false);

		Time.timeScale = 1;

		if (callback != null) {
			callback ();
		}
	}

}
