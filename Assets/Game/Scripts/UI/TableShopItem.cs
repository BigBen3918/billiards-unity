//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System;
using UnityEngine;
using UnityEngine.UI;

public class TableShopItem : MonoBehaviour {

	[SerializeField] private Text nameTxt;
	[SerializeField] private Image image;
	[SerializeField] private Image bgImage;

	[SerializeField] private Color bgClrSelected = Color.white;
	[SerializeField] private Color bgClrNormal = Color.white;

	public string tableId = "";

	private Action<string> clickAction;

	public void Item_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		if (clickAction != null) {
			clickAction (tableId);
		}
	}

	public void Init(string id, string name, Sprite sprite) {
		tableId = id;
		SetName (name);
		SetImage (sprite);
	}

	public void SetName(string name) {
		nameTxt.text = name;
	}

	public void SetImage(Sprite sprite) {
		image.sprite = sprite;
	}

	public void SetClickAction(Action<string> action) {
		clickAction = action;
	}

	public void ShowSelection(bool show) {
		if (show) {
			bgImage.color = bgClrSelected;
		}
		else {
			bgImage.color = bgClrNormal;
		}
	}

}
