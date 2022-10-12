//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

	[SerializeField] private Image profileImg;
	[SerializeField] private Image soundBtnImg;
	[SerializeField] private Sprite soundOnSprite;
	[SerializeField] private Sprite soundOffSprite;
	[SerializeField] private ScrollRect optionsScrollRect;

	[SerializeField] private AvatarsList avatarsList;

	private List<PlayerAvatar> avatars;

	void Awake() {
		avatars = avatarsList.Avatars;
	}

	void Start() {
		UpdateProfileImg ();
		UpdateSoundBtn ();

		ScrollOptionsTo (0);
	}

	public void ProfileBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		PoolSceneManager.Instance.GoToEditProfile ();
	}

	public void ShopBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		PoolSceneManager.Instance.GoToShop ();
	}

	public void SoundBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		AudioManager.Instance.ToggleSound ();
		UpdateSoundBtn ();
	}

	public void OnlineMatchBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		//PoolSceneManager.Instance.StartOnlineMatch ();
		PoolSceneManager.Instance.MyLoadScene("OnlineMatchMode");
	}
	public void TourmentModeBtn_OnClick()
	{
		AudioManager.Instance.PlayBtnSound();
		//PoolSceneManager.Instance.StartOnlineMatch ();
		PoolSceneManager.Instance.MyLoadScene("TourmentMode");
	}
	public void LanMatchBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		PoolSceneManager.Instance.GoToLanLobby ();
	}

	public void BotMatchBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		//PoolSceneManager.Instance.StartBotMatch ();
		PoolSceneManager.Instance.MyLoadScene("DifficultyMode");
	}

	public void PassNPlayBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		PoolSceneManager.Instance.StartPassNPlayMatch ();
	}

	public void MoreGamesBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		AppStoreManager.Instance.OpenMoreGames ();
	}

	private void UpdateProfileImg() {
		profileImg.sprite = avatars [avatarsList.GetAvatarIndex (PlayerInfo.Instance.PlayerAvatarId)].AvatarSprite;
	}

	private void UpdateSoundBtn() {
		if (Prefs.IsSoundOn) {
			soundBtnImg.sprite = soundOnSprite;
		}
		else {
			soundBtnImg.sprite = soundOffSprite;
		}
	}

	private void ScrollOptionsTo(float value) {
		optionsScrollRect.horizontalNormalizedPosition = value;
	}

}
