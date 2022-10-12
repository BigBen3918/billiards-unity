//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;

public class ObjectEnabler : MonoBehaviour {

	[SerializeField] private List<GameObject> objects;

	private void Start() {
		foreach (GameObject go in objects) {
			go.SetActive (true);
		}
	}

}
