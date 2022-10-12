//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;

public class ScreenSleepHandler : MonoBehaviour {

	void OnEnable() {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	void OnDisable() {
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
	}

}
