//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpinKnob : InteractableMonoBehavior, IDragHandler, IPointerDownHandler, IPointerUpHandler {

	[SerializeField] private Image bgImg;
	[SerializeField] private Image knobImg;
	[SerializeField] private float margin = 0;

	private Vector2 inputVector;
	public Vector2 Input {
		get {
			return inputVector;
		}
	}

	private int pointerId = -1;
	public int PointerId {
		get {
			return pointerId;
		}
	}

	public delegate void PressDelegate ();
	public PressDelegate Pressed;

	public delegate void DragDelegate();
	public DragDelegate Dragged;

	public delegate void ReleaseDelegate ();
	public ReleaseDelegate Released;

	public virtual void OnPointerDown(PointerEventData eventData) {
		if (!isInteractable) {
			return;
		}

		OnDrag (eventData);

		if (Pressed != null) {
			Pressed ();
		}
	}

	public virtual void OnDrag(PointerEventData eventData) {
		if (!isInteractable) {
			return;
		}

		Vector2 position;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (bgImg.rectTransform, eventData.position,
			   eventData.pressEventCamera, out position)) {

			position.x /= bgImg.rectTransform.sizeDelta.x;
			position.y /= bgImg.rectTransform.sizeDelta.y;

			inputVector = new Vector2 (position.x * 2, position.y * 2);
			inputVector = inputVector.magnitude > 1 ? inputVector.normalized : inputVector;

			// Update knob position if needed...

			// Save pointer id for later use
			pointerId = eventData.pointerId;
		}

		if (Dragged != null) {
			Dragged ();
		}
	}

	public virtual void OnPointerUp(PointerEventData eventData) {
		if (!isInteractable) {
			return;
		}

		if (Released != null) {
			Released ();
		}

		// Reset pointer id
		pointerId = -1;
	}

	public void SetKnobPosition(Vector2 position) {
		// Move knob
		knobImg.rectTransform.anchoredPosition = new Vector2 (
			position.x * ((bgImg.rectTransform.sizeDelta.x / 2.0f) - (bgImg.rectTransform.sizeDelta.x * margin)),
			position.y * ((bgImg.rectTransform.sizeDelta.y / 2.0f) - (bgImg.rectTransform.sizeDelta.y * margin)));
	}

	public void Reset() {
		inputVector = Vector2.zero;
		knobImg.rectTransform.anchoredPosition = Vector2.zero;

		pointerId = -1;
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
