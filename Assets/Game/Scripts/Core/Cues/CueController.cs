//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CueController : NetworkBehaviour {

	[SerializeField] protected SpriteRenderer cueSpriteRenderer;
	public float disabledOpacity = 0.6f;
	[SerializeField] protected float offsetFromCueBall = 0.7f;
	[SerializeField] protected GameObject guideLinePrefab;

	[SerializeField] private float accuracyModeCueRotSpeed = 1;
	[SerializeField] private float roateSpeed = 1f;
	[SerializeField] protected AudioClip hitSound;

	[SyncVar]
	public NetworkInstanceId ownerNetId;
	[HideInInspector]
	public Player owner;

	public bool isTutorial = false;
	public float MinStrength {
		get;
		set;
	}

	public float MaxStrength {
		get;
		set;
	}

	public float MaxSpin {
		get;
		set;
	}

	public float AimLength {
		get;
		set;
	}

	public float MaxTimePerMove {
		get;
		set;
	}

	protected PoolManager poolManager;
	protected InputManager inputManager;
	protected GuideLineController guideLine;
	protected GameUI gameUI;
	protected Ball cueBall;
	protected Rigidbody cueBallRB;
	protected Slider cueSlider;
	protected SpinKnobButton spinKnobBtn;
	protected SpinMenu spinMenu;
	protected SpinKnob spinKnob;

	protected bool isActive = false;
	protected Vector3 initialTouchPos;

	// Guide line
	protected Vector3 origin;
	protected Vector3 castDirection;
	protected float radius = 0;
	protected Ray ray;
	protected RaycastHit hit;

	private float cueRotationSpeed = 0.7f;
	private float deltaAngle = 0f;
	protected virtual void Awake() {
		poolManager = GameObject.FindObjectOfType<PoolManager> ();
		inputManager = GameObject.FindObjectOfType<InputManager> ();
		inputManager.IgnoreUI (true);
		gameUI = GameObject.FindObjectOfType<GameUI> ();

		if (gameUI != null) {
			cueSlider = gameUI.cueSlider;
			spinKnobBtn = gameUI.spinKnobBtn;
			spinMenu = gameUI.spinMenu;
			spinKnob = spinMenu.spinKnob;
		}

		GameObject guideLineObj = Instantiate (guideLinePrefab);
		guideLine = guideLineObj.GetComponent<GuideLineController> ();
	}
	public bool isReal = false;
	protected virtual void Start() {
		
		
		
		cueBall = poolManager.CueBall;
		cueBallRB = cueBall.GetComponent<Rigidbody> ();
	

		guideLine.HideGuideLine ();
	}

	Vector3 targetpos = Vector3.zero;
	Vector3 target_vetor = Vector3.zero;
	float delta_angle = 0f;
	protected virtual void Update() {
	
		if (inputManager.PointerId != cueSlider.PointerId &&
		   inputManager.PointerId != spinKnob.PointerId) {

			
			if (inputManager.State == InputManager.TouchState.START) {


			
								initialTouchPos = inputManager.TouchPosition;
							

			
								
								
				//deltaAngle = Mathf.Atan(initialTouchPos.y / initialTouchPos.x);

			
				initialTouchPos = Camera.main.ScreenToWorldPoint (new Vector3 (inputManager.TouchPosition.x,
					inputManager.TouchPosition.y, Camera.main.transform.position.y -
					cueBall.transform.position.y));
				target_vetor = initialTouchPos - cueBall.transform.position;
			
			}
			else if (inputManager.State == InputManager.TouchState.STAY) {

				initialTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(inputManager.TouchPosition.x,
					inputManager.TouchPosition.y, Camera.main.transform.position.y -
					cueBall.transform.position.y));


				delta_angle = Vector3.Angle((initialTouchPos - cueBall.transform.position), target_vetor);
				target_vetor = (initialTouchPos - cueBall.transform.position);

				//Debug.Log("xxx=" + delta_angle.ToString());



				Vector2 deltaTouch = inputManager.DeltaTouchPosition;
				

				if (initialTouchPos.x > cueBall.transform.position.x) {
					deltaTouch.y *= -1;
				}

				if (initialTouchPos.z < cueBall.transform.position.z) {
					deltaTouch.x *= -1;
				}

				float angle = deltaTouch.x * delta_angle;
				if (Mathf.Abs (deltaTouch.y) > Mathf.Abs (deltaTouch.x)) {
					angle = deltaTouch.y * delta_angle;
				}

				angle *= cueRotationSpeed;
				RotateCue (angle);
			}
		}

		SetCuePosition ();
	}

	protected virtual void OnDisable() {
		guideLine.HideGuideLine ();
	}

	public virtual void Activate() {
		this.gameObject.SetActive (true);
		isActive = true;

		cueSlider.Pressed = OnSliderPressed;
		cueSlider.Dragged = OnSliderDragged;
		cueSlider.Released = OnSliderReleased;

		spinKnobBtn.Released = OnSpinKnobBtnReleased;

		spinKnob.Dragged = OnSpinKnobDragged;
		spinKnob.Released = OnSpinKnobReleased;

		spinMenu.Released = OnSpinMenuReleased;
	}

	public virtual void Deactivate() {
		this.gameObject.SetActive (false);
		isActive = false;

		guideLine.HideGuideLine ();
	}

	protected void OnSliderPressed() {
		// Take action for slider pressed
	}

	protected void OnSliderDragged() {
		
	}

	protected virtual void OnSliderReleased() {
		Debug.Log("release");
		if (cueSlider.Value <= cueSlider.ignoreBelowValue) {
			return;
		}

		float t = Mathf.InverseLerp (cueSlider.ignoreBelowValue, 1, cueSlider.Value);

		Vector3 direction = transform.forward;
		float magnitude = MaxStrength * t;
		magnitude = Mathf.Clamp (magnitude, MinStrength, MaxStrength);

		Vector3 force = direction * magnitude;

		// For left & right spin ==> y axis
		// For top & bottom spin ==> z axis
		Vector2 spin = spinKnob.Input;

		float spinMagnitude = Mathf.Lerp (0, 1, cueSlider.Value / 1);
		Vector3 horizontalSpin = new Vector3 (0, -spin.x * MaxSpin * spinMagnitude, 0);
		Vector3 verticalSpin = transform.right * spin.y * MaxSpin * spinMagnitude * 5;

		Vector3 angularVelocity = new Vector3 (verticalSpin.x, horizontalSpin.y, verticalSpin.z);

		HitCueBall (force, angularVelocity);

		owner.EndTurn (false);

		if (isTutorial)
			(poolManager as TutorialController).ToGame();
		

	}



	protected virtual void OnSpinKnobBtnReleased() {
		ShowSpinMenu ();
	}

	protected virtual void OnSpinKnobDragged() {
		SetKnobPosition (spinKnob.Input);
	}

	protected virtual void OnSpinKnobReleased() {
		HideSpinMenu ();
	}

	protected virtual void OnSpinMenuReleased() {
		HideSpinMenu ();
	}

	protected void DrawGuideLine() {
		origin = cueBall.transform.position;
		castDirection = transform.forward;
		radius = cueBall.Radius;
		ray = new Ray (origin, castDirection);

		if (Physics.SphereCast (ray, radius, out hit, 100)) {
			Vector3 collisionCentroid = cueBall.transform.position + (castDirection.normalized * hit.distance);
			guideLine.DrawLineMain (cueBall.transform.position, collisionCentroid);

			bool ballAllowed = true;

			Ball targetBall = hit.collider.GetComponent<Ball> ();
			if (targetBall != null) {
				ballAllowed = poolManager.IsBallAllowed (owner, targetBall);

				if (ballAllowed) {
					Vector3 cueBallDirection = Vector3.ProjectOnPlane (ray.direction, hit.normal);
					Vector3 cueBallPoint2 = collisionCentroid + (cueBallDirection * AimLength);
					guideLine.DrawLineSource (collisionCentroid, cueBallPoint2);

					Vector3 targetBallDirection = hit.normal;
					Vector3 targetBallPoint2 = targetBall.transform.position - (targetBallDirection * AimLength);
					guideLine.DrawLineTarget (targetBall.transform.position, targetBallPoint2);

					//cueRotationSpeed = accuracyModeCueRotSpeed;



					cueRotationSpeed = 1f;
					//cueRotationSpeed = 1.5f;
					//cueRotationSpeed = 2f;
					//cueRotationSpeed = 2.5f;
				}
			}
			else {

				cueRotationSpeed = 1f;
				//cueRotationSpeed = 1.5f;
				//cueRotationSpeed = 2f;
				//cueRotationSpeed = 2.5f;

				guideLine.HideLineSource ();
				guideLine.HideLineTarget ();
			}

			guideLine.DrawGhostBall (collisionCentroid, ballAllowed);

			if (!ballAllowed) {
				guideLine.HideLineSource ();
				guideLine.HideLineTarget ();
			}
		}
		else {
			guideLine.HideGuideLine ();
		}
	}

	protected float GetAngle(Vector3 position) {
		Vector3 dir = (position - cueBall.transform.position).normalized;
		Quaternion targetRotation = Quaternion.LookRotation (dir);
		float angle = Quaternion.Angle (transform.rotation, targetRotation);

		return angle;
	}

	public void RotateCue(float angle) {
		transform.RotateAround (cueBall.transform.position, Vector3.up, angle);
	}

	protected void SetCuePosition() {
		if (cueBall == null) {
			return;
		}

		Vector3 cueDirection = (cueBall.transform.position - transform.position).normalized;
		float offset = offsetFromCueBall + (cueSlider.Value / 1.5f);
		Vector3 cuePosition = cueBall.transform.position - (cueDirection * offset);
		cuePosition.y = cueBall.transform.position.y;
		transform.position = cuePosition;

		transform.LookAt (cueBall.transform);
	}

	public void LookAt(Vector3 point) {
		SetCuePosition ();

		float angle = GetAngle (point);
		if (Mathf.Abs(angle) > 90) {
			angle *= -1;
		}

		transform.RotateAround (cueBall.transform.position, Vector3.up, angle);
	}

	public void SetCueOpacity(float opacity) {
		Color clr = cueSpriteRenderer.color;
		clr.a = opacity;

		cueSpriteRenderer.color = clr;

		guideLine.SetGuideLineOpacity (opacity);
	}

	protected virtual void ShowSpinMenu() {
		spinMenu.ShowMenu ();
	}

	protected virtual void HideSpinMenu() {
		spinMenu.HideMenu ();
	}

	protected virtual void SetKnobPosition(Vector2 position) {
		spinKnob.SetKnobPosition (position);
		spinKnobBtn.SetKnobPosition (position);
	}

	public virtual void HitCueBall(Vector3 force, Vector3 angularVelocity) {
		cueBallRB.AddForce (force, ForceMode.VelocityChange);
		cueBallRB.angularVelocity = angularVelocity;

		PlayHitSound ();
	}

	protected virtual void PlayHitSound() {
		AudioSource.PlayClipAtPoint (hitSound, Camera.main.transform.position);
	}

	public void SetStats(string cueId) {
		CueStats cueStats = PlayerInfo.Instance.GetCue (cueId);

		SetCueSprite (cueStats.CueSprite);

		MinStrength = cueStats.MinStrength;
		MaxStrength = cueStats.MaxStrength;
		MaxSpin = cueStats.MaxSpin;
		AimLength = cueStats.AimLength;
		MaxTimePerMove = cueStats.TimePerMove;
	}

	private void SetCueSprite(Sprite sprite) {
		if (cueSpriteRenderer == null) {
			return;
		}

		cueSpriteRenderer.sprite = sprite;
	}

}
