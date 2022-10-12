//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpinKnobButton : InteractableMonoBehavior, IPointerDownHandler, IPointerUpHandler {

	[SerializeField] private Image bgImg;
	[SerializeField] private Image knobImg;
	[SerializeField] private float margin = 0;

	public delegate void ReleaseDelegate ();
	public ReleaseDelegate Released;

	public virtual void OnPointerDown(PointerEventData eventData) {
		if (!isInteractable) {
			return;
		}
	}

	public virtual void OnPointerUp(PointerEventData eventData) {
		if (!isInteractable) {
			return;
		}

		if (Released != null) {
			Released ();
		}
	}

	public void SetKnobPosition(Vector2 position) {
		// Move knob
		knobImg.rectTransform.anchoredPosition = new Vector2 (
			position.x * ((bgImg.rectTransform.sizeDelta.x / 2.0f) - (bgImg.rectTransform.sizeDelta.x * margin)),
			position.y * ((bgImg.rectTransform.sizeDelta.y / 2.0f) - (bgImg.rectTransform.sizeDelta.y * margin)));
	}

	public override void SetInteraction(bool interactable) {
		base.SetInteraction (interactable);

		Color clr = normalColor;
		if (!isInteractable) {
			clr = disabledColor;
		}

		bgImg.color = clr;
		knobImg.color = clr;
	}

}
