//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkDiscovery_LAN : NetworkDiscovery {

	[SerializeField] private int broadcastDataMaxSize = 20;

	public delegate void GamesListUpdateDelegate (List<LanConnectionInfo> gamesList);
	public event GamesListUpdateDelegate GamesListUpdated;

	private Dictionary<LanConnectionInfo, float> connections = new Dictionary<LanConnectionInfo, float>();

	private float timeout = 3.0f;

	void Awake() {
		Initialize ();
		StartAsClient ();

		StartCoroutine (CleanUpExpiredEntriesCo (timeout));
	}

	void Start() {
		broadcastData = NetworkManager.singleton.networkPort + "," + PlayerInfo.Instance.PlayerName;
		if (broadcastData.Length > broadcastDataMaxSize) {
			broadcastData = broadcastData.Substring (0, broadcastDataMaxSize);
		}
		else if (broadcastData.Length < broadcastDataMaxSize) {
			string strToAppend = new string (' ', broadcastDataMaxSize - broadcastData.Length);
			broadcastData += strToAppend;
		}
	}

	public void StartBroadcast() {
		StopBroadcast ();
		Initialize ();
		StartAsServer ();
	}

	public void StopServer() {
		if (isServer) {
			Destroy (this.gameObject);
		}
	}

	public override void OnReceivedBroadcast (string fromAddress, string data) {
		base.OnReceivedBroadcast (fromAddress, data);

		string filteredData = data.Substring (0, broadcastDataMaxSize);

		string ipAddress = fromAddress.Substring (fromAddress.LastIndexOf (":") + 1);

		int port = 0;
		string name = "";
		string[] dataArray = filteredData.Split (",".ToCharArray(), 2);
		if (dataArray != null && dataArray.Length == 2) {
			port = int.Parse (dataArray[0]);
			name = dataArray [1];
		}

		LanConnectionInfo lanConnection = new LanConnectionInfo (ipAddress, port, name);

		if (!connections.ContainsKey (lanConnection)) {
			connections.Add (lanConnection, Time.time + timeout);

			ReportListUpdate ();
		}
		else {
			connections [lanConnection] = Time.time + timeout;
		}
	}

	private IEnumerator CleanUpExpiredEntriesCo(float interval) {
		while (true) {
			List<LanConnectionInfo> entries = new List<LanConnectionInfo> (connections.Keys);
			foreach (LanConnectionInfo con in entries) {
				if (connections [con] <= Time.time) {
					connections.Remove (con);

					ReportListUpdate ();
				}
			}

			yield return new WaitForSeconds (timeout);
		}
	}

	private void ReportListUpdate() {
		if (GamesListUpdated != null) {
			List<LanConnectionInfo> gamesList = new List<LanConnectionInfo> (connections.Keys);
			GamesListUpdated (gamesList);
		}
	}

}

public struct LanConnectionInfo {

	public string ipAddress;
	public int port;
	public string name;

	public LanConnectionInfo(string ip, int port, string name) {
		this.ipAddress = ip;
		this.port = port;
		this.name = name;
	}

	public override string ToString () {
		return "IP address: " + ipAddress + ", Port: " + port + ", Name: " + name;
	}

}
