//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System;
using UnityEngine;
using UnityEngine.UI;

public class IAPItemUI : MonoBehaviour {
	
	[SerializeField] private Image image;

	private string productId = "";

	private Action<string> buyBtnAction;

	public void BuyBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		if (buyBtnAction != null) {
			buyBtnAction (productId);
		}
	}

	public void Init(string id, Sprite sprite) {
		productId = id;
		SetImage (sprite);
	}

	public void SetImage(Sprite sprite) {
		image.sprite = sprite;
	}

	public void SetBuyBtnAction(Action<string> action) {
		buyBtnAction = action;
	}

}
