//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(AudioSource))]
public class Timer : MonoBehaviour {

	public Color normalColor = Color.green;
	public Color mediumColor = Color.yellow;
	public Color lowColor = Color.red;

	private Image timerImg;
	public Image TimerImage {
		get {
			return timerImg;
		}
	}

	private AudioSource audioSrc;

	private bool isPlayingSound = false;

	void Awake() {
		timerImg = GetComponent<Image> ();
		audioSrc = GetComponent<AudioSource> ();
	}

	public void StartTickSound() {
		if (isPlayingSound) {
			return;
		}

		audioSrc.Play ();
		isPlayingSound = true;
	}

	public void StopTickSound() {
		if (!isPlayingSound) {
			return;
		}

		audioSrc.Stop ();
		isPlayingSound = false;
	}

}
