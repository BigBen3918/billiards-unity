//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakerPlayerUI : MonoBehaviour {

	[SerializeField] private Image avatarImg;
	[SerializeField] private Text nameTxt;

	[SerializeField] private List<GameObject> winningElements;
	[SerializeField] private GameObject prizeHolder;
	[SerializeField] private Text prizeValueTxt;

	[SerializeField] private AvatarsList avatarsList;

	[SerializeField] private AudioClip tickSound;
	[SerializeField] private AudioClip matchFoundSound;

	private AudioSource audioSrc;

	private List<PlayerAvatar> availableAvatars;

	private bool animRunning = false;

	void Awake() {
		availableAvatars = avatarsList.Avatars;
		audioSrc = GetComponent<AudioSource> ();

		EnableWinningElements (false);
	}

	public void Init(string name, Sprite avatar) {
		SetName (name);
		SetAvatar (avatar);
	}

	public void SetAvatar(Sprite avatar) {
		avatarImg.sprite = avatar;
	}

	public void SetName(string name) {
		nameTxt.text = Formatter.FormatName (name);
	}

	public void StartFindAnim() {
		if (animRunning) {
			return;
		}

		SetName ("");

		animRunning = true;
		StartCoroutine (FindMatchAnimCo (0.1f, 0.2f));
	}

	public void StopFindAnim() {
		if (!animRunning) {
			return;
		}

		animRunning = false;
		PlayMatchFoundSound ();
	}

	public void MarkWinner(string prizeTxt) {
		PlayMatchFoundSound ();

		EnableWinningElements (true);

		if (string.IsNullOrEmpty (prizeTxt)) {
			prizeHolder.SetActive (false);
		}
		else {
			SetPrizeText (prizeTxt);
		}
	}

	public void SetPrizeText(string txt) {
		prizeValueTxt.text = txt;
	}

	private IEnumerator FindMatchAnimCo(float minInterval, float maxInterval) {
		int idx = GetRandomAvatarIdx ();

		while (animRunning) {
			avatarImg.sprite = availableAvatars [idx].AvatarSprite;

			PlayTickSound ();

			int newIdx = GetRandomAvatarIdx ();
			if (newIdx != idx) {
				idx = newIdx;
			}
			else {
				idx = (newIdx + 1) % availableAvatars.Count;
			}

			yield return new WaitForSeconds (Random.Range (minInterval, maxInterval));
		}
	}

	private int GetRandomAvatarIdx() {
		return Random.Range(0, availableAvatars.Count);
	}

	private void PlayTickSound() {
		audioSrc.clip = tickSound;
		audioSrc.Play ();
	}

	private void PlayMatchFoundSound() {
		audioSrc.clip = matchFoundSound;
		audioSrc.Play ();
	}

	private void EnableWinningElements(bool enable) {
		foreach (GameObject element in winningElements) {
			element.SetActive (enable);
		}
	}

}
