//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour {
	
	[SerializeField] private SpriteRenderer tableRenderer;
	[SerializeField] private Transform headSpot;
	[SerializeField] private Transform footSpot;
	[SerializeField] private Transform centerPoint;
	[SerializeField] private MeshRenderer playingAreaMesh;
	[SerializeField] private MeshRenderer headStringAreaMesh;
	[SerializeField] private GameObject headStringArea;

	public Transform HeadSpot {
		get {
			return headSpot;
		}
	}

	public Transform FootSpot {
		get {
			return footSpot;
		}
	}

	public Transform CenterPoint {
		get {
			return centerPoint;
		}
	}

	public Bounds PlayingAreaBounds {
		get {
			return playingAreaMesh.bounds;
		}
	}

	public Bounds HeadStringAreaBounds {
		get {
			return headStringAreaMesh.bounds;
		}
	}

	void Awake() {
		HideHeadStringArea ();
	}

	void Start() {
		SetProperties (PlayerInfo.Instance.GetSelectedTable());
	}

	public void ShowHeadStringArea() {
		headStringArea.SetActive (true);
	}

	public void HideHeadStringArea() {
		headStringArea.SetActive (false);
	}

	private void SetProperties(TableProperties properties) {
		SetTableSprite (properties.TableSprite);
	}

	private void SetTableSprite(Sprite sprite) {
		if (tableRenderer == null) {
			return;
		}

		tableRenderer.sprite = sprite;
	}

}
