//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.UI;

public class CoinBalanceUI : MonoBehaviour {

	[SerializeField] private Text balanceTxt;

	void OnEnable() {
		PlayerInfo.Instance.CoinBalanceChanged += OnCoinBalanceChanged;
	}

	void Start() {
		UpdateBalanceTxt ();
	}

	void OnDisable() {
		PlayerInfo.Instance.CoinBalanceChanged -= OnCoinBalanceChanged;
	}

	private void OnCoinBalanceChanged() {
		UpdateBalanceTxt ();
	}

	private void UpdateBalanceTxt() {
		balanceTxt.text = Formatter.FormatCash (PlayerInfo.Instance.CoinBalance);
	}

}
