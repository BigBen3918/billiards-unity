//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;

[CreateAssetMenu(fileName = "CueStats", menuName = "Cues/CueStatsObject", order = 1)]
public class CueStats : ScriptableObject {

	[SerializeField] private string cueId = "";
	[SerializeField] private string cueName = "";
	[SerializeField] private Sprite cueSprite;
	[SerializeField] private float price = 0;

	[SerializeField] private float minStrength = 0;
	[SerializeField] private float maxStrength = 0;
	[SerializeField] private float maxSpin = 0;
	[SerializeField] private float aimLength = 0;
	[SerializeField] private float timePerMove = 0;

	public string CueId {
		get {
			return cueId;
		}
	}

	public string CueName {
		get {
			return cueName;
		}
	}

	public Sprite CueSprite {
		get {
			return cueSprite;
		}
	}

	public float Price {
		get {
			return price;
		}
	}

	public float MinStrength {
		get {
			return minStrength;
		}
	}

	public float MaxStrength {
		get {
			return maxStrength;
		}
	}

	public float MaxSpin {
		get {
			return maxSpin;
		}
	}

	public float AimLength {
		get {
			return aimLength;
		}
	}

	public float TimePerMove {
		get {
			return timePerMove;
		}
	}

}