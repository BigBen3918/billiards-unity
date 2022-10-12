using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCue : CueController
{
	protected override void Start()
	{
		base.Start();

		LookAt(poolManager.PoolTable.FootSpot.position);
		Deactivate();

		

		owner.ReportReady();
	}

	protected override void Update()
	{
		
		

		DrawGuideLine();

		base.Update();
	}
}
