//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System;
using UnityEngine;
using UnityEngine.UI;

public class CueShopItem : MonoBehaviour {

	public enum SelectButtonOptions {
		SELECT_ENABLED,
		SELECT_DISABLED,
		SELECTED
	}

	[SerializeField] private Text nameTxt;
	[SerializeField] private Image image;
	[SerializeField] private Text priceTxt;

	[SerializeField] private Meter powerMeter;
	[SerializeField] private Meter spinMeter;
	[SerializeField] private Meter aimMeter;
	[SerializeField] private Meter timeMeter;

	[SerializeField] private GameObject buyOptions;
	[SerializeField] private GameObject boughtDetails;

	[SerializeField] private Button selectBtn;
	[SerializeField] private Sprite selectSpriteEnabled;
	[SerializeField] private Sprite selectSpriteDisabled;
	[SerializeField] private Sprite selectedSprite;

	private string cueId = "";

	private Action<string> buyBtnAction;
	private Action<string> selectBtnAction;

	public void BuyBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		if (buyBtnAction != null) {
			buyBtnAction (cueId);
		}
	}

	public void SelectBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		if (selectBtnAction != null) {
			selectBtnAction (cueId);
		}
	}

	public void Init(string id, string name, Sprite sprite, float price) {
		cueId = id;
		SetName (name);
		SetImage (sprite);
		SetPrice (price);
	}

	public void SetName(string name) {
		nameTxt.text = name;
	}

	public void SetImage(Sprite sprite) {
		image.sprite = sprite;
	}

	public void SetPrice(float price) {
		priceTxt.text = Formatter.FormatCash (price);
	}

	public void SetStats(float power, float spin, float aim, float time) {
		powerMeter.SetFillAmount (power);
		spinMeter.SetFillAmount (spin);
		aimMeter.SetFillAmount (aim);
		timeMeter.SetFillAmount (time);
	}

	public void ShowBuyOptions(bool show) {
		buyOptions.SetActive (show);
		boughtDetails.SetActive (!show);
	}

	public void SetSelectBtnOption(SelectButtonOptions option) {
		Image btnImg = selectBtn.GetComponent<Image> ();

		if (option == SelectButtonOptions.SELECT_ENABLED) {
			btnImg.sprite = selectSpriteEnabled;
			selectBtn.interactable = true;
		}
		else if (option == SelectButtonOptions.SELECT_DISABLED) {
			btnImg.sprite = selectSpriteDisabled;
			selectBtn.interactable = false;
		}
		else if (option == SelectButtonOptions.SELECTED) {
			btnImg.sprite = selectedSprite;
			selectBtn.interactable = false;
		}
	}

	public void SetBuyBtnAction(Action<string> action) {
		buyBtnAction = action;
	}

	public void SetSelectBtnAction(Action<string> action) {
		selectBtnAction = action;
	}

}
