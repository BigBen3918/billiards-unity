//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.UI;

public class InteractableMonoBehavior : MonoBehaviour {

	protected bool isInteractable = true;

	[SerializeField]
	protected Color normalColor = Color.white;

	[SerializeField]
	protected Color disabledColor = Color.white;

	public virtual void SetInteraction(bool interactable) {
		isInteractable = interactable;
	}

}
