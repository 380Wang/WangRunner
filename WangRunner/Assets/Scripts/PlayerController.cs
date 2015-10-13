using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float boostForce = 3.0f;
	public float diveKickForce = 6.0f;
	public float movementSpeed = 3.0f;
	public float jumpForce = 200.0f;
	public float jetpackForce = 100.0f;
	public float uppercutForce = 6.0f;

	public RectTransform titleBar;
	public Transform groundCheckTransform;
	public LayerMask groundCheckLayerMask;
	public Text currentScore;
    public JumpAbility CurrentJump = JumpAbility.None;
    public ActionAbility CurrentAction = ActionAbility.None;
	
	private bool grounded = false;
	private bool isFirstTouch = true;
	private Rigidbody2D player;
	private float clickDelay;
	
	// Use this for initialization
	void Start () {
		player = GetComponent<Rigidbody2D> ();
		// Click delay hack
		clickDelay = 0.04f;
	}

	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Anchored position: " + (0.0f - titleBar.sizeDelta.y));
	}

	void FixedUpdate(){

		Run ();

		// Click delay hack to prevent accidental double jumps
		clickDelay -= Time.fixedDeltaTime;
		//bool pressedRight = false;

		//== FOR DESKTOP: CONTROLS ==//
			if (Input.GetKey (KeyCode.RightArrow)) {
				RegisterTouch (RIGHT);
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				RegisterTouch (LEFT);
			}
		//=============================//

		// Will jump when right side of screen is tapped
		int tCount = Input.touchCount;
		if (tCount > 0){
			foreach( var touch in Input.touches ){
				if( TouchedGameplay(touch) ){
					if( TouchedRightSide(touch) ){
							RegisterTouch( RIGHT );
					}
						//pressedRight = (clickDelay <= 0) && ScreenIsTapped();
					else{
							RegisterTouch( LEFT );
					}
				}
			}
		}

		UpdateGroundedStatus ();
	}

	void OnGUI(){
		if( currentScore != null )
			UpdateScore ();
	}

	void UpdateScore(){
		currentScore.text = "Distance: " + ((int)transform.position.x + 3) + "m";
	}

	void UpdateGroundedStatus(){
		grounded = Physics2D.OverlapCircle (groundCheckTransform.position, 0.1f, groundCheckLayerMask);
	}

	void Boost(){
		player.AddForce (new Vector2 (boostForce, 0));
	}

	void DiveKick(){
		if (isFirstTouch) {
			if (ScreenIsTapped ()) {
				isFirstTouch = false;
				Jump ();
			}
		} else {
			if( grounded )
				isFirstTouch = true;
			else{
				Vector3 diveVelo = player.velocity;
				diveVelo.x = diveKickForce;
				diveVelo.y = -1.0f * diveKickForce;
				player.velocity = diveVelo;
			}
		}
	}

	void Slide(){
		if (Input.touchCount > 0)
			player.transform.eulerAngles = new Vector3( 0, 0, 45 );
		else {
			player.transform.eulerAngles = new Vector3 ( 0, 0, 0 );
		}
	}

	void StabilizeY(){
		if (isFirstTouch) {
			if( ScreenIsTapped() ){
				isFirstTouch = false;
				Jump ();
			}
		} else {
			if(grounded)
				isFirstTouch = true;
			else{
				//Debug.Log("Stabilizing Y");
				Vector3 currVelo = player.velocity;
				currVelo.y = 0.6f;
				player.velocity = currVelo;
			}
		}
	}

	void Uppercut(){
		player.velocity = new Vector2 (uppercutForce, uppercutForce);
	}

	void Jump(){
		if (ScreenIsTapped ()) {
			if (grounded && clickDelay <= 0) {
				player.AddForce (new Vector2 (0, jumpForce));
				clickDelay = 0.02f;
			}
		}
	}

	void ActivateJetpack(){
		player.AddForce (new Vector2 (0, jetpackForce));
	}

	/**
	 * Constantly moves player to the right
	 **/
	void Run(){
		Vector2 newVelocity = player.velocity;
		newVelocity.x = movementSpeed;
		player.velocity = newVelocity;
	}

	/**
	 * Check for taps and not held down
	 **/
	bool ScreenIsTapped(){
		// In Android:
		// Single finger = MouseButtonDown(0)
		// Double finger = MouseButtonDown(1)
		return Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)
			|| Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow);
	}

	private void RegisterTouch( bool touch ){
		bool DEBUG_T = true;

		if ( touch == LEFT ) {
			// Left side was tapped
			if( DEBUG_T && ScreenIsTapped() ) Debug.Log( "" + CurrentAction );
			switch(CurrentAction){
			case ActionAbility.Attack:

				break;
				// DONE
			case ActionAbility.Boost:
				Boost();
				break;
			case ActionAbility.GrapplingHook:

				break;
				// NOT DONE
			case ActionAbility.Slide:
				Slide();
				break;
				// DONE
			case ActionAbility.Uppercut:
				Uppercut();
				break;
			default:
				if( DEBUG_T) Debug.Log ("LeftSide");
				break;
			}
		} else {
			if( DEBUG_T && ScreenIsTapped() ) Debug.Log( "" + CurrentJump );
			// Right side was tapped
			switch(CurrentJump){
				// DONE
			case JumpAbility.AirStabilizer:
				StabilizeY();
				break;
			case JumpAbility.DiveKick:
				DiveKick();
				break;
			case JumpAbility.DoubleJump:

				break;

			case JumpAbility.Glider:

				break;
				// DONE
			case JumpAbility.Jump:
				Jump();
				break;
				// DONE
			case JumpAbility.Jetpack:
				ActivateJetpack();
				break;
			default:
				Debug.Log("RightSide");
				break;
			}
		}
	}

	/**
	 * Check if game was touched and not UI
	 **/
	private bool TouchedGameplay( Touch touch ){
		if (touch.position.y < (Screen.height - (Screen.height / 8)))
			return true;
		else
			return false;
	}
	
	private bool TouchedRightSide(Touch touch){
		bool isOnScreen = false;
		
		if (touch.position.x > Screen.width / 2)
			isOnScreen = true;
		
		return isOnScreen;
	}

	private static bool LEFT = true;
	private static bool RIGHT = false;
}
