//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLineController : MonoBehaviour {

	[SerializeField] private GameObject linePrefab;
	[SerializeField] private GameObject ghostBallPrefab;
	[SerializeField] private Sprite allowedGhostBallSprite;
	[SerializeField] private Sprite restrictedGhostBallSprite;

	[HideInInspector]
	public GameObject lineMain;
	[HideInInspector]
	public GameObject lineSource;
	[HideInInspector]
	public GameObject lineTarget;
	[HideInInspector]
	public GameObject ghostBall;

	private SpriteRenderer lineMainRenderer;
	private SpriteRenderer lineSourceRenderer;
	private SpriteRenderer lineTargetRenderer;
	private SpriteRenderer ghostBallRenderer;

	private void Awake() {
		if (lineMain == null) {
			lineMain = Instantiate (linePrefab);
		}

		if (lineSource == null) {
			lineSource = Instantiate (linePrefab);
		}

		if (lineTarget == null) {
			lineTarget = Instantiate (linePrefab);
		}

		if (ghostBall == null) {
			ghostBall = Instantiate (ghostBallPrefab);
			ghostBall.SetActive (false);
		}

		lineMainRenderer = lineMain.GetComponent<SpriteRenderer> ();
		lineSourceRenderer = lineSource.GetComponent<SpriteRenderer> ();
		lineTargetRenderer = lineTarget.GetComponent<SpriteRenderer> ();
		ghostBallRenderer = ghostBall.GetComponent<SpriteRenderer> ();
	}

	void Start() {
		
	}

	public void DrawLineMain(Vector3 point1, Vector3 point2) {
		DrawLine (lineMain, lineMainRenderer, point1, point2);
	}

	public void DrawLineSource(Vector3 point1, Vector3 point2) {
		DrawLine (lineSource, lineSourceRenderer, point1, point2);
	}

	public void DrawLineTarget(Vector3 point1, Vector3 point2) {
		DrawLine (lineTarget, lineTargetRenderer, point1, point2);
	}

	public void DrawGhostBall(Vector3 center, bool allowed) {
		if (ghostBall == null) {
			return;
		}

		ghostBall.SetActive (true);
		ghostBall.transform.position = center;

		if (ghostBallRenderer != null) {
			if (allowed) {
				ghostBallRenderer.sprite = allowedGhostBallSprite;
			}
			else {
				ghostBallRenderer.sprite = restrictedGhostBallSprite;
			}
		}
	}

	public void HideGuideLine() {
		HideLineMain ();
		HideLineSource ();
		HideLineTarget ();

		if (ghostBall != null) {
			ghostBall.SetActive (false);
		}
	}

	public void SetGuideLineOpacity(float opacity) {
		Color clr = lineMainRenderer.color;
		clr.a = opacity;

		lineMainRenderer.color = clr;
		lineSourceRenderer.color = clr;
		lineTargetRenderer.color = clr;
		ghostBallRenderer.color = clr;
	}

	public void HideLineMain() {
		if (lineMainRenderer != null) {
			lineMainRenderer.enabled = false;
		}
	}

	public void HideLineSource() {
		if (lineSourceRenderer != null) {
			lineSourceRenderer.enabled = false;
		}
	}

	public void HideLineTarget() {
		if (lineTargetRenderer != null) {
			lineTargetRenderer.enabled = false;
		}
	}

	private void DrawLine(GameObject line, SpriteRenderer lineRenderer, Vector3 point1, Vector3 point2) {
		lineRenderer.enabled = true;

		Vector3 dir = (point2 - point1).normalized;
		float distance = Vector3.Distance (point1, point2);

		Vector3 position = point1 + (dir * (distance / 2.0f));
		line.transform.position = position;

		if (dir.magnitude != 0) {
			Vector3 rotation = Quaternion.LookRotation (dir).eulerAngles;
			rotation.z = -rotation.y;
			rotation.x = 90;
			rotation.y = 0;
			line.transform.rotation = Quaternion.Euler (rotation);
		}

		Vector2 size = lineRenderer.size;
		size.y = distance;
		lineRenderer.size = size;
	}

}
