//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;

public class BallCollector : MonoBehaviour {

	private AudioSource audioSrc;

	[SerializeField] private AudioClip collectorSound;

	void Awake() {
		audioSrc = GetComponent<AudioSource> ();
	}

	public void StartCollectorSound() {
		audioSrc.loop = true;
		audioSrc.clip = collectorSound;
		audioSrc.Play ();
	}

	public void StopCollectorSound() {
		audioSrc.Stop ();
	}

}
