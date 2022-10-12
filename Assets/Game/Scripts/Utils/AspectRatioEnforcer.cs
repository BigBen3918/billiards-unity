//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;

public class AspectRatioEnforcer : MonoBehaviour {

	[SerializeField] private List<Camera> targetCameras;

	[SerializeField] private Vector2 targetAspectRatio = new Vector2(16.0f, 9.0f);
	[SerializeField] private Color letterBoxCameraBGColor = Color.black;

	private Camera letterBoxCamera;

	void Awake() {
		CreateLetterBoxCamera ();

		foreach (Camera cam in targetCameras) {
			ForceCameraRatio (cam, targetAspectRatio);
		}
	}

	private void CreateLetterBoxCamera() {
		letterBoxCamera = this.gameObject.AddComponent<Camera> ();

		letterBoxCamera.depth = -2;
		letterBoxCamera.clearFlags = CameraClearFlags.SolidColor;
		letterBoxCamera.cullingMask = 0;
		letterBoxCamera.backgroundColor = letterBoxCameraBGColor;
		letterBoxCamera.allowHDR = false;
		letterBoxCamera.allowMSAA = false;
	}

	private void ForceCameraRatio(Camera cam, Vector2 ratio) {
		float currentScreenAspect = (float)Screen.width / (float)Screen.height;

		float targetAspect = ratio.x / ratio.y;

		float heightScaler = currentScreenAspect / targetAspect;

		if (heightScaler < 1.0f) {
			Rect camRect = cam.rect;

			camRect.width = 1.0f;
			camRect.height = heightScaler;
			camRect.x = 0;
			camRect.y = (1.0f - heightScaler) / 2.0f;

			cam.rect = camRect;
		}
		else {
			float widthScaler = 1.0f / heightScaler;

			Rect camRect = cam.rect;

			camRect.width = widthScaler;
			camRect.height = 1.0f;
			camRect.x = (1.0f - widthScaler) / 2.0f;
			camRect.y = 0;

			cam.rect = camRect;
		}
	}

}
