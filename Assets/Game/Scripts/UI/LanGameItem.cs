//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System;
using UnityEngine;
using UnityEngine.UI;

public class LanGameItem : MonoBehaviour {

	[SerializeField] private Text nameTxt;

	private LanConnectionInfo connectionInfo;

	private Action<LanConnectionInfo> joinBtnAction;

	public void JoinBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();

		if (joinBtnAction != null) {
			joinBtnAction (connectionInfo);
		}
	}

	public void Init(LanConnectionInfo connectionInfo, Action<LanConnectionInfo> joinBtnAction) {
		this.connectionInfo = connectionInfo;

		SetName (Formatter.FormatName(connectionInfo.name));
		SetClickAction (joinBtnAction);
	}

	public void SetName(string name) {
		nameTxt.text = name;
	}

	public void SetClickAction(Action<LanConnectionInfo> action) {
		joinBtnAction = action;
	}

}
