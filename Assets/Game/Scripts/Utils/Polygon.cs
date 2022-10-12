//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon {

	public Vector2[] Points {
		get;
		private set;
	}

	public Polygon(Vector2[] points) {
		Points = points;
	}

	public bool ContainsPoint(Vector2 point) {
		bool result = false;
		int j = Points.Length - 1;
		for (int i = 0; i < Points.Length; i++) {
			if (Points[i].y < point.y && Points[j].y >= point.y || Points[j].y < point.y && Points[i].y >= point.y) {
				if (Points[i].x + (point.y - Points[i].y) / (Points[j].y - Points[i].y) * (Points[j].x - Points[i].x) < point.x) {
					result = !result;
				}
			}
			j = i;
		}

		return result;
	}

}
