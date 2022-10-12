//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupManager : SingletonMonoBehavior<PopupManager> {

	[SerializeField] private GameObject popupPrefab;

	private GameObject popup;
	private PopupHandler popupHandler;

	protected override void Awake() {
		base.Awake ();
		DontDestroyOnLoad (this.gameObject);
	}

	public void ShowPopup(string heading, string msg, string positiveBtnTxt, string negativeBtnTxt,
		UnityAction positiveBtnAction, UnityAction negativeBtnAction, UnityAction closeBtnAction) {

		if (popup == null) {
			popup = Instantiate (popupPrefab);
			popupHandler = popup.GetComponent<PopupHandler> ();
		}

		if (popupHandler != null) {
			popupHandler.SetHeading (heading);
			popupHandler.SetMessage (msg);
			popupHandler.SetPositiveBtnTxt (positiveBtnTxt);
			popupHandler.SetNegativeBtnTxt (negativeBtnTxt);

			if (positiveBtnAction == null) {
				popupHandler.HidePositiveBtn ();
			}
			else {
				popupHandler.SetPositiveBtnAction (positiveBtnAction);
			}

			if (negativeBtnAction == null) {
				popupHandler.HideNegativeBtn ();
			}
			else {
				popupHandler.SetNegativeBtnAction (negativeBtnAction);
			}

			if (closeBtnAction == null) {
				popupHandler.HideCloseBtn ();
			}
			else {
				popupHandler.SetCloseBtnAction (closeBtnAction);
			}

			popupHandler.ShowPopup ();
		}
	}

	public void HidePopup() {
		if (popupHandler != null) {
			popupHandler.HidePopup ();
		}
	}

}
