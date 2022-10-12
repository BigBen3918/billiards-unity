//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {

	[SerializeField] private GameObject[] tabs;
	[SerializeField] private Image[] tabBtnImages;
	[SerializeField] private Sprite tabSpriteActive;
	[SerializeField] private Sprite tabSpriteInactive;

	public string insufficientCoinsHeading = "";
	public string insufficientCoinsMsg = "";

	private int selectedTabIdx = 0;

	void Start() {
		ActivateTab (1);
	}

	public void Tab_OnClick(int tabNumber) {
		AudioManager.Instance.PlayBtnSound ();
		ActivateTab (tabNumber);
	}

	public void NextBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		ActivateTab (GetNextTabIdx () + 1);
	}

	public void PreviousBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		ActivateTab (GetPreviousTabIdx () + 1);
	}

	public void CloseBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		PoolSceneManager.Instance.GoToMainMenu ();
	}

	public void OpenCoinsTab() {
		ActivateTab (3);
	}

	private void ActivateTab(int tabNumber) {
		int tabIndex = tabNumber - 1;
		if (tabIndex < 0 || tabIndex >= tabs.Length) {
			return;
		}

		for (int i = 0; i < tabs.Length; i++) {
			if (i == tabIndex) {
				tabs [i].SetActive (true);
				tabBtnImages [i].sprite = tabSpriteActive;
			}
			else {
				tabs [i].SetActive (false);
				tabBtnImages [i].sprite = tabSpriteInactive;
			}
		}

		selectedTabIdx = tabIndex;
	}

	private int GetNextTabIdx() {
		if (selectedTabIdx == tabs.Length - 1) {
			return selectedTabIdx;
		}

		return selectedTabIdx + 1;
	}

	private int GetPreviousTabIdx() {
		if (selectedTabIdx == 0) {
			return selectedTabIdx;
		}

		return selectedTabIdx - 1;
	}

}
