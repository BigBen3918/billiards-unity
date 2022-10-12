//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsShopUI : MonoBehaviour {

	[SerializeField] private GameObject iapItemPrefab;
	[SerializeField] private GameObject iapHolder;

	[SerializeField] private InAppProductsList iapList;

	private IAPManager iapManager;

	private List<InAppProduct> products;
	private List<IAPItemUI> productUIHandlers = new List<IAPItemUI>();

	void Awake() {
		iapManager = GameObject.FindObjectOfType<IAPManager> ();

		products = iapList.Products;
	}

	void Start() {
		LoadProducts ();
	}

	private void LoadProducts() {
		for (int i = 0; i < products.Count; i++) {
			InAppProduct product = products [i];
			GameObject productObj = Instantiate (iapItemPrefab, iapHolder.transform);
			IAPItemUI productUIHandler = productObj.GetComponent<IAPItemUI> ();
			if (productUIHandler != null) {
				productUIHandlers.Add (productUIHandler);

				productUIHandler.Init (product.ProductId, product.ProductImage);

				productUIHandler.SetBuyBtnAction (ProductBuyBtn_OnClick);
			}
		}
	}

	private void ProductBuyBtn_OnClick(string productId) {
		iapManager.BuyProduct (iapList.GetProduct (productId), OnProductPurchased);
	}

	private void OnProductPurchased(InAppProduct product) {
		string heading = "";
		string msg = "";

		if (product.CoinsToAdd > 0) {
			PlayerInfo.Instance.CoinBalance += product.CoinsToAdd;

			heading = "+ " + product.CoinsToAdd + " Coins!";
			msg = "You have successfully purchased " + product.CoinsToAdd + " coins.\n" +
			"\nNew balance: " + Formatter.FormatCash (PlayerInfo.Instance.CoinBalance);
		}

		if (product.RemoveAds) {
			PlayerInfo.Instance.IsNoAdsPurchased = true;

			heading = "Ads Removed!";
			msg = "You have successfully removed ads from the game.";
		}

		PopupManager.Instance.ShowPopup (heading, msg, "OK", "",
			() => {
				AudioManager.Instance.PlayBtnSound ();
				PopupManager.Instance.HidePopup ();
			},
			null, null);
	}

}
