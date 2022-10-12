//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class DimmableGraphic : MonoBehaviour {

	[SerializeField] private Color dimColor = Color.gray;

	private Color normalColor;

	private Graphic graphic;

	void Awake() {
		graphic = GetComponent<Graphic> ();
		normalColor = graphic.color;
	}

	public void Dim() {
		graphic.color = dimColor;
	}

	public void UnDim() {
		graphic.color = normalColor;
	}

}
