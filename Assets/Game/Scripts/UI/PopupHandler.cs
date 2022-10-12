//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PopupHandler : MonoBehaviour {

	[SerializeField] private Text headingTxt;
	[SerializeField] private Text msgTxt;
	[SerializeField] private Button positiveBtn;
	[SerializeField] private Text positiveBtnTxt;
	[SerializeField] private Button negativeBtn;
	[SerializeField] private Text negativeBtnTxt;
	[SerializeField] private Button closeBtn;
	[SerializeField] private AnimationClip showClip;
	[SerializeField] private AnimationClip hideClip;

	private Animator anim;

	void Awake() {
		anim = GetComponent<Animator> ();
	}

	public void SetupPopup(string heading, string msg, UnityAction positiveAction, UnityAction negativeAction) {
		SetHeading (heading);
		SetMessage (msg);

		if (positiveAction == null) {
			HidePositiveBtn ();
		}
		else {
			SetPositiveBtnAction (positiveAction);
		}
		if (negativeAction == null) {
			HideNegativeBtn ();
		}
		else {
			SetNegativeBtnAction (negativeAction);
		}
	}

	public void SetHeading(string heading) {
		headingTxt.text = heading;
	}

	public void SetMessage(string msg) {
		msgTxt.text = msg;
	}

	public void SetPositiveBtnTxt(string txt) {
		positiveBtnTxt.text = txt;
	}

	public void SetNegativeBtnTxt(string txt) {
		negativeBtnTxt.text = txt;
	}

	public void SetPositiveBtnAction(UnityAction action) {
		positiveBtn.onClick.RemoveAllListeners ();
		positiveBtn.onClick.AddListener (action);
	}

	public void SetNegativeBtnAction(UnityAction action) {
		negativeBtn.onClick.RemoveAllListeners ();
		negativeBtn.onClick.AddListener (action);
	}

	public void SetCloseBtnAction(UnityAction action) {
		closeBtn.onClick.RemoveAllListeners ();
		closeBtn.onClick.AddListener (action);
	}

	public void SetPopupVisibility(bool visible) {
		if (visible) {
			ShowPopup ();
		}
		else {
			HidePopup ();
		}
	}

	public void ShowPopup() {
		this.gameObject.SetActive (true);
		anim.Play (showClip.name);
	}

	public void HidePopup() {
		anim.Play (hideClip.name);
	}

	public void HideAnim_OnEnded() {
		Destroy (this.gameObject);
	}

	public void ShowPositiveBtn() {
		positiveBtn.gameObject.SetActive (true);
	}

	public void HidePositiveBtn() {
		positiveBtn.gameObject.SetActive (false);

		Vector2 negativeBtnPos = negativeBtn.GetComponent<RectTransform> ().anchoredPosition;
		negativeBtn.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, negativeBtnPos.y);
	}

	public void ShowNegativeBtn() {
		negativeBtn.gameObject.SetActive (true);
	}

	public void HideNegativeBtn() {
		negativeBtn.gameObject.SetActive (false);

		Vector2 positiveBtnPos = positiveBtn.GetComponent<RectTransform> ().anchoredPosition;
		positiveBtn.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, positiveBtnPos.y);
	}

	public void ShowCloseBtn() {
		closeBtn.gameObject.SetActive (true);
	}

	public void HideCloseBtn() {
		closeBtn.gameObject.SetActive (false);
	}

	public void ResetBtnActions() {
		positiveBtn.onClick.RemoveAllListeners ();
		negativeBtn.onClick.RemoveAllListeners ();
		closeBtn.onClick.RemoveAllListeners ();
	}

	public void PlaySwooshSound() {
		AudioManager.Instance.PlaySwooshSound ();
	}

}