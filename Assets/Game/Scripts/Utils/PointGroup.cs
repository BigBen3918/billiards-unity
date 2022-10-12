//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointGroup : MonoBehaviour {

	public Color lineColor;
	public bool drawGizmos = true;

	[HideInInspector]
	public List<Transform> nodes = new List<Transform>();
	
	void Start (){
		nodes.Clear();
		for (int i = 0; i < transform.childCount; i++){
			nodes.Add(transform.GetChild(i));
		}
	}

	void OnDrawGizmos() {
		if (!drawGizmos) {
			return;
		}

		Gizmos.color = lineColor;

		Transform[] pathTransforms = GetComponentsInChildren<Transform>();
		nodes = new List<Transform>();

		for(int i = 0; i < pathTransforms.Length; i++) {
			if(pathTransforms[i] != transform) {
				nodes.Add(pathTransforms[i]);
			}
		}

		for(int i = 0; i < nodes.Count; i++) {
			Vector3 currentNode = nodes[i].position;
			Vector3 previousNode = Vector3.zero;

			if (i > 0) {
				previousNode = nodes[i - 1].position;
			} else if(i == 0 && nodes.Count > 1) {
				previousNode = nodes[nodes.Count - 1].position;
			}

			Gizmos.DrawLine(previousNode, currentNode);
			Gizmos.DrawWireSphere(currentNode, 0.02f);
		}
	}

}
