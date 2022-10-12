//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;

public class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour {

	private static T instance = null;
	public static T Instance {
		get {
			return instance;
		}
	}

	protected virtual void Awake() {
		if (instance == null) {
			instance = this as T;
		}
		else if (instance != this) {
			Destroy (this.gameObject);
		}
	}

}
