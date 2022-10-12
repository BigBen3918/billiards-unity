//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "InAppProduct", menuName = "IAP/InAppProduct", order = 1)]
public class InAppProduct : ScriptableObject {

	[SerializeField] private string productId = "";
	[SerializeField] private Sprite productImage;

	[SerializeField] private float coinsToAdd = 0;
	[SerializeField] private bool removeAds = false;

	[SerializeField] private ProductType productType;

	public string ProductId {
		get {
			return productId;
		}
	}

	public Sprite ProductImage {
		get {
			return productImage;
		}
	}

	public float CoinsToAdd {
		get {
			return coinsToAdd;
		}
	}

	public bool RemoveAds {
		get {
			return removeAds;
		}
	}

	public ProductType ProductType {
		get {
			return productType;
		}
	}

}