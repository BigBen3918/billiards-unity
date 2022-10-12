//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AvatarsList", menuName = "Avatars/AvatarsList", order = 1)]
public class AvatarsList : ScriptableObject {

	[SerializeField] private string collectionName = "";
	[SerializeField] private List<PlayerAvatar> avatars;

	[SerializeField] private PlayerAvatar defaultAvatar;
	[SerializeField] private PlayerAvatar botAvatar;

	public string CollectionName {
		get {
			return collectionName;
		}
	}

	public List<PlayerAvatar> Avatars {
		get {
			return avatars;
		}
	}

	public int GetAvatarIndex(string id) {
		for (int i = 0; i < avatars.Count; i++) {
			if (avatars [i].AvatarId.Equals (id)) {
				return i;
			}
		}

		return -1;
	}

	public PlayerAvatar GetAvatar(string id) {
		for (int i = 0; i < avatars.Count; i++) {
			if (avatars [i].AvatarId.Equals (id)) {
				return avatars [i];
			}
		}

		return null;
	}

	public PlayerAvatar DefaultAvatar {
		get {
			return defaultAvatar;
		}
	}

	public PlayerAvatar BotAvatar {
		get {
			return botAvatar;
		}
	}

}