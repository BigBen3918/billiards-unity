//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditProfileUI : MonoBehaviour {

	[SerializeField] private AvatarsList avatarsList;

	[SerializeField] private ScrollRect avatarsScrollRect;
	[SerializeField] private GameObject avatarsHolder;
	[SerializeField] private GameObject avatarItemPrefab;
	[SerializeField] private GameObject avatarItemEmptyPrefab;
	[SerializeField] private InputField nameField;

	private List<PlayerAvatar> avatars;
	private List<AvatarItem> avatarHandlers = new List<AvatarItem>();

	private Coroutine lerpCo;

	void Awake() {
		avatars = avatarsList.Avatars;
	}

	void Start() {
		LoadAvatars ();
		LerpToSelectedAvatar ();

		nameField.text = Formatter.FormatName (PlayerInfo.Instance.PlayerName);
	}

	public void AvatarsScrollRect_OnValueChanged() {
		UpdateAvatarItems ();
	}

	public void AvatarsScrollRect_OnPointerDown() {
		if (lerpCo != null) {
			StopCoroutine (lerpCo);
		}
	}

	public void AvatarsScrollRect_OnPointerUp() {
		LerpToAvatar (GetClosestAvatar());
	}

	public void NextAvatarBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		LerpToAvatar (GetClosestAvatar () + 1);
	}

	public void PreviousAvatarBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		LerpToAvatar (GetClosestAvatar () - 1);
	}

	public void OkBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		string playerName = nameField.text;
		PlayerInfo.Instance.PlayerName = playerName;

		string avatarId = avatarsList.Avatars [GetClosestAvatar ()].AvatarId;
		PlayerInfo.Instance.PlayerAvatarId = avatarId;

		//PoolSceneManager.Instance.GoToMainMenu ();
		PoolSceneManager.Instance.MyLoadScene("MainScreen");
	}

	public void CancelBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		//PoolSceneManager.Instance.GoToMainMenu ();
		PoolSceneManager.Instance.MyLoadScene("MainScreen");
	}

	private void LoadAvatars() {
		AddEmptyAvatar ();

		for (int i = 0; i < avatars.Count; i++) {
			GameObject avatar = Instantiate (avatarItemPrefab, avatarsHolder.transform);
			AvatarItem avatarHandler = avatar.GetComponent<AvatarItem> ();
			if (avatarHandler != null) {
				avatarHandler.Init (avatars [i].AvatarSprite);

				avatarHandlers.Add (avatarHandler);
			}
		}

		AddEmptyAvatar ();
	}

	private void UpdateAvatarItems() {
		for (int i = 0; i < avatarHandlers.Count; i++) {
			AvatarItem handler = avatarHandlers [i];

			float scrollValue = avatarsScrollRect.horizontalNormalizedPosition;
			float currentAvatarScrollValue = (i * 1.0f) / (avatars.Count - 1);
			float distance = (scrollValue - currentAvatarScrollValue) * (avatars.Count - 1);

			float sizeMultiplier = Mathf.Lerp (1, 0, Mathf.Abs(distance));
			handler.SetSize (sizeMultiplier);

			float alpha = Mathf.Lerp (0, 1, Mathf.Abs (distance));
			if (distance > 0) {
				handler.SetLeftAlpha (alpha);
				handler.SetRightAlpha (0);
			}
			else if (distance < 0) {
				handler.SetRightAlpha (alpha);
				handler.SetLeftAlpha (0);
			}
		}
	}

	private void LerpToAvatar(int avatarIdx) {
		if (lerpCo != null) {
			StopCoroutine (lerpCo);
		}

		avatarIdx = Mathf.Clamp (avatarIdx, 0, avatars.Count - 1);

		float finalValue = (avatarIdx * 1.0f) / (avatars.Count - 1);

		lerpCo = StartCoroutine (LerpCo (finalValue, 5));
	}

	private IEnumerator LerpCo(float finalValue, float lerpSpeed) {
		yield return new WaitForEndOfFrame ();

		float reachThreshold = 0.01f;
		float startValue = avatarsScrollRect.horizontalNormalizedPosition;

		if (Mathf.Abs (startValue - finalValue) > reachThreshold) {
			bool keepLerping = true;
			while (keepLerping) {
				if (finalValue > startValue) {
					if (finalValue - avatarsScrollRect.horizontalNormalizedPosition <= reachThreshold) {
						keepLerping = false;
					}
				}
				else if (finalValue < startValue) {
					if (avatarsScrollRect.horizontalNormalizedPosition - finalValue <= reachThreshold) {
						keepLerping = false;
					}
				}

				float t = Mathf.Lerp (avatarsScrollRect.horizontalNormalizedPosition,
					finalValue, Time.deltaTime * lerpSpeed);

				avatarsScrollRect.horizontalNormalizedPosition = t;

				yield return new WaitForEndOfFrame ();
			}
		}

		avatarsScrollRect.horizontalNormalizedPosition = finalValue;
	}

	private void LerpToSelectedAvatar() {
		LerpToAvatar (avatarsList.GetAvatarIndex (PlayerInfo.Instance.PlayerAvatarId));
	}

	private int GetClosestAvatar() {
		return Mathf.RoundToInt (avatarsScrollRect.horizontalNormalizedPosition * (avatars.Count - 1));
	}

	private void AddEmptyAvatar() {
		Instantiate (avatarItemEmptyPrefab, avatarsHolder.transform);
	}

}
