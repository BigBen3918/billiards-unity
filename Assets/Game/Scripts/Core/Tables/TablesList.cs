//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TablesList", menuName = "Tables/TablesList", order = 1)]
public class TablesList : ScriptableObject {

	[SerializeField] private string collectionName = "";
	[SerializeField] private List<TableProperties> tables;

	public string CollectionName {
		get {
			return collectionName;
		}
	}

	public List<TableProperties> Tables {
		get {
			return tables;
		}
	}

	public TableProperties GetTable(string tableId) {
		foreach (TableProperties table in Tables) {
			if (table.TableId.Equals (tableId)) {
				return table;
			}
		}

		return null;
	}

}