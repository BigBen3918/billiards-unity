//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;

[CreateAssetMenu(fileName = "Avatar", menuName = "Avatars/Avatar", order = 1)]
public class PlayerAvatar : ScriptableObject {

	[SerializeField] private string avatarId = "";
	[SerializeField] private string avatarName = "";
	[SerializeField] private Sprite avatarSprite;

	[SerializeField] private float price = 0;

	public string AvatarId {
		get {
			return avatarId;
		}
	}

	public string AvatarName {
		get {
			return avatarName;
		}
	}

	public Sprite AvatarSprite {
		get {
			return avatarSprite;
		}
	}

	public float Price {
		get {
			return price;
		}
	}

}