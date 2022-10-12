//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CueController_Local : CueController {

	protected override void Start() {
		base.Start ();

		LookAt (poolManager.PoolTable.FootSpot.position);
		Deactivate ();

	

		owner.ReportReady ();
	}

	protected override void Update() {
		if (!isActive) {
			return;
		}

		DrawGuideLine ();

		base.Update ();
	}

}
