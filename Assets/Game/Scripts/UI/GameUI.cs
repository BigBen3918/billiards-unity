//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	[SerializeField] private PauseUI pauseUI;

	public PlayerUI player1UI;
	public PlayerUI player2UI;

	[SerializeField] private Text prizeTxt;

	public Slider cueSlider;
	public SpinKnobButton spinKnobBtn;
	public SpinMenu spinMenu;
	public SpinKnob spinKnob;

	public Toast toastHandler;

	[SerializeField] private Sprite emptyBallSprite;
	[SerializeField] private List<Sprite> ballSprites;

	[SerializeField] private PoolManager poolManager;

	void Start() {
		EnableControls (false);
	}

	public void PauseBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		poolManager.PauseGame ();
		pauseUI.ShowPauseUI ();
	}

	public Sprite GetEmptyBallSprite() {
		return emptyBallSprite;
	}

	public Sprite GetBallSprite(int ballNumber) {
		if (ballNumber < 0 || ballNumber >= ballSprites.Count) {
			return null;
		}

		return ballSprites [ballNumber];
	}

	public void EnableControls(bool enable) {
		if (enable) {
			cueSlider.ShowSlider ();
		}
		else {
			cueSlider.HideSlider ();
		}

		spinKnobBtn.SetInteraction (enable);
		spinMenu.SetInteraction (enable);
		spinKnob.SetInteraction (enable);
	}

	public void ResetSpin() {
		spinKnob.Reset ();
		spinKnobBtn.SetKnobPosition (spinKnob.Input);

		spinMenu.HideMenu ();
	}

	public void SetPrize(float prizeAmount) {
		prizeTxt.text = Formatter.FormatCash (prizeAmount);
	}

}
