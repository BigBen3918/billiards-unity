//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slider : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

	[SerializeField] private GameObject holder;
	[SerializeField] private Image bgImg;
	[SerializeField] private Image sliderImg;
	[SerializeField] private Image meterImg;
	[SerializeField] private Vector2 cueOffset;

	public float ignoreBelowValue = 0;
	[Range(0, 1)]
	public float draggableHeight = 1;
	public float dragSpeed = 1;

	[Range(0, 1)]
	[SerializeField] private float meterAlphaDeactivated = 0;
	[Range(0, 1)]
	[SerializeField] private float meterMinAlphaActivated = 0.4f;

	[SerializeField] private AnimationClip sliderShowClip;
	[SerializeField] private AnimationClip sliderHideClip;

	private float value;
	public float Value {
		get {
			return value;
		}
	}

	private int pointerId = -1;
	public int PointerId {
		get {
			return pointerId;
		}
	}

	private bool touchStarted = false;

	public delegate void PressDelegate ();
	public PressDelegate Pressed;

	public delegate void DragDelegate();
	public DragDelegate Dragged;

	public delegate void ReleaseDelegate ();
	public ReleaseDelegate Released;

	private Animator anim;

	private bool isEnabled = true;

	void Awake() {
		anim = GetComponent<Animator> ();
	}

	void Start() {
		
	}

	public virtual void OnPointerDown(PointerEventData eventData) {
		touchStarted = true;

		if (Pressed != null) {
			Pressed ();
		}
	}

	public virtual void OnDrag(PointerEventData eventData) {
		if (!touchStarted) {
			return;
		}

		value -= (eventData.delta.y * dragSpeed) / bgImg.rectTransform.sizeDelta.y;
		value = Mathf.Clamp01 (value);

		// Move slider
		sliderImg.rectTransform.anchoredPosition = new Vector2 (sliderImg.rectTransform.anchoredPosition.x,
			-value * (bgImg.rectTransform.sizeDelta.y * draggableHeight)) + cueOffset;

		// Update meter color
		Color meterClr = meterImg.color;
		if (Value < ignoreBelowValue) {
			meterClr.a = meterAlphaDeactivated;
		}
		else {
			meterClr.a = Mathf.Lerp (meterMinAlphaActivated, 1, Value);
		}

		meterImg.color = meterClr;

		// Save pointer id for later use
		pointerId = eventData.pointerId;

		if (Dragged != null) {
			Dragged ();
		}
	}

	public virtual void OnPointerUp(PointerEventData eventData) {
		touchStarted = false;

		if (Released != null) {
			Released ();
		}

		ResetSlider ();
	}

	public void ResetSlider() {
		// Reset slider and input
		value = 0;
		sliderImg.rectTransform.anchoredPosition = Vector2.zero + cueOffset;

		// Reset meter color
		Color meterClr = meterImg.color;
		meterClr.a = meterAlphaDeactivated;
		meterImg.color = meterClr;

		// Reset pointer id
		pointerId = -1;

		touchStarted = false;
	}

	public void ShowSlider() {
		if (isEnabled) {
			return;
		}

		anim.Play (sliderShowClip.name);

		isEnabled = true;
	}

	public void HideSlider() {
		if (!isEnabled) {
			return;
		}

		anim.Play (sliderHideClip.name);

		isEnabled = false;
	}

}
