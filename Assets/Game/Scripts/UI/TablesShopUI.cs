//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TablesShopUI : MonoBehaviour {

	[SerializeField] private GameObject tableItemPrefab;
	[SerializeField] private GameObject tablesHolder;
	[SerializeField] private Image tablePreviewImage;

	[SerializeField] private GameObject buyOptions;
	[SerializeField] private GameObject boughtOptions;
	[SerializeField] private Button selectBtn;
	[SerializeField] private Sprite selectSprite;
	[SerializeField] private Sprite selectedSprite;

	[SerializeField] private Text priceTxt;

	[SerializeField] private TablesList tablesList;

	private ShopUI shopUI;

	private List<TableProperties> tables;
	private List<TableShopItem> tableUIHandlers = new List<TableShopItem>();

	private string previewedTableId = "";

	void Awake() {
		shopUI = GameObject.FindObjectOfType<ShopUI> ();

		tables = tablesList.Tables;
	}

	void Start() {
		LoadTables ();
		ShowTable (PlayerInfo.Instance.SelectedTable);
	}

	public void BuyBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		BuyTable (previewedTableId);
	}

	public void SelectBtn_OnClick() {
		AudioManager.Instance.PlayBtnSound ();
		SelectTable (previewedTableId);
	}

	private void LoadTables() {
		for (int i = 0; i < tables.Count; i++) {
			TableProperties table = tables [i];
			GameObject tableObj = Instantiate (tableItemPrefab, tablesHolder.transform);
			TableShopItem tableUIHandler = tableObj.GetComponent<TableShopItem> ();
			if (tableUIHandler != null) {
				tableUIHandlers.Add (tableUIHandler);

				tableUIHandler.Init (table.TableId, table.TableName, table.ThumbnailSprite);

				tableUIHandler.SetClickAction (TableItem_OnClick);
			}
		}
	}

	private void TableItem_OnClick(string tableId) {
		ShowTable (tableId);
	}

	private void ShowTable(string tableId) {
		previewedTableId = tableId;

		foreach (TableShopItem uiHandler in tableUIHandlers) {
			uiHandler.ShowSelection (uiHandler.tableId.Equals (tableId));
		}

		tablePreviewImage.sprite = tablesList.GetTable (tableId).TableSprite;

		UpdateTableOptions ();
	}

	private void UpdateTableOptions() {
		priceTxt.text = Formatter.FormatCash (tablesList.GetTable (previewedTableId).Price);

		if (!PlayerInfo.Instance.IsTableOwned (previewedTableId)) {
			buyOptions.SetActive (true);
			boughtOptions.SetActive (false);
		}
		else {
			boughtOptions.SetActive (true);
			buyOptions.SetActive (false);

			Image selectBtnImg = selectBtn.GetComponent<Image> ();
			if (PlayerInfo.Instance.SelectedTable == previewedTableId) {
				selectBtnImg.sprite = selectedSprite;
				selectBtn.interactable = false;
			}
			else {
				selectBtnImg.sprite = selectSprite;
				selectBtn.interactable = true;
			}
		}
	}

	private void BuyTable(string tableId) {
		if (PlayerInfo.Instance.IsTableOwned (tableId)) {
			return;
		}

		TableProperties table = tablesList.GetTable (tableId);
		if (PlayerInfo.Instance.CoinBalance < table.Price) {
			string heading = shopUI.insufficientCoinsHeading;
			string msg = shopUI.insufficientCoinsMsg;

			float balanceNeeded = table.Price - PlayerInfo.Instance.CoinBalance;
			msg += "\n\nCoins needed: " + Formatter.FormatCash (balanceNeeded);

			PopupManager.Instance.ShowPopup (heading, msg, "Buy Coins", "Cancel",
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup();
					shopUI.OpenCoinsTab();
				},
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup ();
				},
				() => {
					AudioManager.Instance.PlayBtnSound ();
					PopupManager.Instance.HidePopup ();
				});

			return;
		}

		PlayerInfo.Instance.CoinBalance -= table.Price;
		PlayerInfo.Instance.AddToOwnedTables (table.TableId);

		UpdateTableOptions ();
	}

	private void SelectTable(string tableId) {
		if (PlayerInfo.Instance.SelectedTable == tableId) {
			return;
		}

		if (PlayerInfo.Instance.IsTableOwned (tableId)) {
			PlayerInfo.Instance.SelectedTable = tableId;
		}

		UpdateTableOptions ();
	}

}
