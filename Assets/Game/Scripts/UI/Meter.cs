//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour {

	[SerializeField] private Image[] pointImages;
	[SerializeField] private Sprite pointSpriteEmpty;
	[SerializeField] private Sprite pointSpriteFilled;

	public void SetFillAmount(float amount) {
		amount = Mathf.Clamp01 (amount);

		int pointsToFill = Mathf.RoundToInt (amount * pointImages.Length);

		for (int i = 0; i < pointImages.Length; i++) {
			if (i < pointsToFill) {
				pointImages [i].sprite = pointSpriteFilled;
			}
			else {
				pointImages [i].sprite = pointSpriteEmpty;
			}
		}
	}

}
