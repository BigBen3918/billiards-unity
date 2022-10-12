//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour {

	[SerializeField] private GameObject holder;
	[SerializeField] private Text text;

	private bool isShowing = false;

	void Awake() {
		holder.SetActive (false);
	}

	public void ShowToast(string txt, float duration) {
		StartCoroutine (ToastCo (txt, duration));
	}

	public void ShowToast(string txt) {
		holder.SetActive (true);
		text.text = txt;

		isShowing = true;
	}

	public void HideToast() {
		holder.SetActive (false);

		isShowing = false;
	}

	private IEnumerator ToastCo(string txt, float delay) {
		while (isShowing) {
			yield return new WaitForEndOfFrame ();
		}

		ShowToast (txt);

		yield return new WaitForSeconds (delay);

		HideToast ();
	}

}
