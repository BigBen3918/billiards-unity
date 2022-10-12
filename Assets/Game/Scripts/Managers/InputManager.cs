//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	public enum TouchState
	{
		UNKNOWN,
		START,
		STAY,
		END
	}

	public float sensitivityMouse = 1;
	public float sensitivityTouch = 1;

	public Vector2 TouchPosition {
		get;
		private set;
	}

	public Vector2 DeltaTouchPosition {
		get;
		private set;
	}

	private TouchState touchState = TouchState.UNKNOWN;
	public TouchState State {
		get {
			return touchState;
		}

		private set {
			touchState = value;
		}
	}

	public int PointerId {
		get;
		private set;
	}

	private bool ignoreUI = false;
	private bool locked = false;
	private bool touchStarted = false;

	void Update() {
		if (locked) {
			return;
		}

		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		if (ignoreUI && EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		if(Input.GetMouseButtonDown(0)) {
			touchStarted = true;
			State = TouchState.START;

			StartCoroutine(ResetTouchStateCo());
		}
		else if(Input.GetMouseButton(0)) {
			if(touchStarted) {
				State = TouchState.STAY;
			}
		}
		else if(Input.GetMouseButtonUp(0)) {
			touchStarted = false;
			State = TouchState.END;

			StartCoroutine(ResetTouchStateCo());
		}

		TouchPosition = Input.mousePosition;

		float x = Input.GetAxis("Mouse X");
		float y = Input.GetAxis("Mouse Y");

		DeltaTouchPosition = new Vector2(x, y) * sensitivityMouse;

		PointerId = 0;

		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		for(int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch(i);
			if (ignoreUI && EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {
				return;
			}

			if(touch.phase == TouchPhase.Began) {
				touchStarted = true;
				State = TouchState.START;
			}
			else if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
				if(touchStarted) {
					State = TouchState.STAY;
				}
			}
			else if(touch.phase == TouchPhase.Ended) {
				touchStarted = false;
				State = TouchState.END;
			}

			TouchPosition = touch.position;

			DeltaTouchPosition = touch.deltaPosition * sensitivityTouch;

			PointerId = touch.fingerId;
		}

		#endif
	}

	public void IgnoreUI(bool ignore) {
		ignoreUI = ignore;
	}

	public void Lock() {
		touchStarted = false;
		TouchPosition = Vector2.zero;
		DeltaTouchPosition = Vector2.zero;
		State = TouchState.UNKNOWN;
		PointerId = 0;

		locked = true;
	}

	public void Unlock() {
		locked = false;
	}

	private IEnumerator ResetTouchStateCo() {
		yield return new WaitForEndOfFrame ();

		State = TouchState.UNKNOWN;
	}

}
