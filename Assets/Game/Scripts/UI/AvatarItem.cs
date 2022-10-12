//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour {

	[SerializeField] private Image avatarImg;
	[SerializeField] private Image alphaRight;
	[SerializeField] private Image alphaLeft;

	[SerializeField] private float minSize = 1;
	[SerializeField] private float maxSize = 1;

	private RectTransform rectTransform;
	private Vector2 startingSize;

	void Awake() {
		rectTransform = GetComponent<RectTransform> ();
		startingSize = rectTransform.sizeDelta;
	}

	public void Init(Sprite image) {
		SetImage (image);
		SetSize (0);
		SetRightAlpha (0);
		SetLeftAlpha (0);
	}

	public void SetImage(Sprite img) {
		avatarImg.sprite = img;
	}

	public void SetRightAlpha(float alpha) {
		SetAlpha (alphaRight, alpha);
	}

	public void SetLeftAlpha(float alpha) {
		SetAlpha (alphaLeft, alpha);
	}

	private void SetAlpha(Image img, float alpha) {
		if (alpha < 0 || alpha > 1) {
			return;
		}

		Color clr = img.color;
		clr.a = alpha;

		img.color = clr;
	}

	public void SetSize(float multiplier) {
		if (multiplier < 0 || multiplier > 1) {
			return;
		}

		float sizeMultiplier = Mathf.Lerp (minSize, maxSize, multiplier);

		Vector2 size = startingSize;
		size *= sizeMultiplier;

		rectTransform.sizeDelta = size;
	}

}
