//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CuesList", menuName = "Cues/CuesList", order = 1)]
public class CuesList : ScriptableObject {

	[SerializeField] private string collectionName = "";
	[SerializeField] private List<CueStats> cues;

	[SerializeField] private float maxPower = 0;
	[SerializeField] private float maxSpin = 0;
	[SerializeField] private float maxAim = 0;
	[SerializeField] private float maxTime = 0;

	public string CollectionName {
		get {
			return collectionName;
		}
	}

	public List<CueStats> Cues {
		get {
			return cues;
		}
	}

	public CueStats GetCue(string cueId) {
		foreach (CueStats cue in Cues) {
			if (cue.CueId.Equals (cueId)) {
				return cue;
			}
		}

		return null;
	}

	public float GetCueRelativePower(CueStats cue) {
		return cue.MaxStrength / maxPower;
	}

	public float GetCueRelativeSpin(CueStats cue) {
		return cue.MaxSpin / maxSpin;
	}

	public float GetCueRelativeAim(CueStats cue) {
		return cue.AimLength / maxAim;
	}

	public float GetCueRelativeTime(CueStats cue) {
		return cue.TimePerMove / maxTime;
	}

}