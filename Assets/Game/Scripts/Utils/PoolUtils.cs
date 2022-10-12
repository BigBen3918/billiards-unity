//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolUtils {

	public static float ClampAngle(float angle, float min, float max) {
		if (min < 0 && max > 0 && (angle > max || angle < min)) {
			angle -= 360;
			if (angle > max || angle < min) {
				if (Mathf.Abs (Mathf.DeltaAngle (angle, min)) < Mathf.Abs (Mathf.DeltaAngle (angle, max))) {
					return min;
				}
				else {
					return max;
				}
			}
		}
		else if(min > 0 && (angle > max || angle < min)) {
			angle += 360;
			if (angle > max || angle < min) {
				if (Mathf.Abs (Mathf.DeltaAngle (angle, min)) < Mathf.Abs (Mathf.DeltaAngle (angle, max))) {
					return min;
				}
				else {
					return max;
				}
			}
		}

		if (angle < min) {
			return min;
		}
		else if (angle > max) {
			return max;
		}
		else {
			return angle;
		}
	}

	public static float SpeedMPH(Rigidbody body) {
		return body.velocity.magnitude * 2.23694f;
	}

	public static bool DoCirclesIntersect(float x1, float y1, float r1, float x2, float y2, float r2) {
		float dx = x1 - x2;
		float dy = y1 - y2;
		float radius = r1 + r2;

		return (dx * dx) + (dy * dy) <= radius * radius;
	}

	public static List<T> Shuffle<T>(List<T> list) {
		List<T> inputList = new List<T> (list);
		List<T> randomList = new List<T>();

		int randomIndex = 0;
		while (inputList.Count > 0) {
			randomIndex = Random.Range (0, inputList.Count);
			randomList.Add(inputList[randomIndex]);
			inputList.RemoveAt(randomIndex);
		}

		return randomList;
	}

	public static bool IsPointInBounds(Bounds bounds, Vector3 point) {
		return bounds.Contains (point);
	}

	public static Vector3 NearestPointOnBounds(Bounds bounds, Vector3 referencePoint) {
		return bounds.ClosestPoint (referencePoint);
	}

}
