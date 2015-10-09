using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 3.0f;
	public float jumpForce = 200.0f;
	public float jetpackForce = 100.0f;
	public RectTransform titleBar;
	public Transform groundCheckTransform;
	public LayerMask groundCheckLayerMask;
	public Text currentScore;
    public JumpAbility CurrentJump = JumpAbility.None;
    public ActionAbility CurrentAcction = ActionAbility.None;
	
	private bool grounded = false;
	private Rigidbody2D player;
	private float clickDelay;
    Collider2D playerCollider;
	
	// Use this for initialization
	void Start () {
		player = GetComponent<Rigidbody2D> ();
        playerCollider = player.GetComponent<Collider2D>();
		clickDelay = 0.04f;
	}

	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Anchored position: " + (0.0f - titleBar.sizeDelta.y));
	}

	void FixedUpdate(){

		PushRight ();

		// Click delay hack to prevent accidental double jumps
		clickDelay -= Time.fixedDeltaTime;
		bool pressedRight = false;

		//== FOR DESKTOP: CONTROLS ==//
		if ( Input.GetKeyDown(KeyCode.RightArrow) )
			pressedRight = (clickDelay <= 0);
		if (Input.GetKey (KeyCode.LeftArrow))
			PlayerActivateJetpack ();
		//=============================//

		// Will jump when right side of screen is tapped
		int tCount = Input.touchCount;
		if (tCount > 0){
			foreach( var touch in Input.touches ){
				if( TouchedGameplay(touch) ){
					if( TouchedRightSide(touch) )
						pressedRight = (clickDelay <= 0) && ScreenIsTapped();
					else{
						PlayerActivateJetpack ();
					}
				}
			}
		}
		if(pressedRight && grounded){
			PlayerJump();
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
        grounded = Physics2D.IsTouchingLayers(playerCollider, groundCheckLayerMask.value);
	}

	void PlayerJump(){
		clickDelay = 0.04f;
		player.AddForce (new Vector2 (0, jumpForce));
	}

	void PlayerActivateJetpack(){
		player.AddForce (new Vector2 (0, jetpackForce));
	}

	/**
	 * Constantly moves player to the right
	 **/
	void PushRight(){
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
		return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
	}

	/**
	 * Check if game was touched and not UI
	 **/
	bool TouchedGameplay( Touch touch ){
		if (touch.position.y < (Screen.height - (Screen.height / 8)))
			return true;
		else
			return false;
	}

	bool TouchedRightSide(Touch touch){
		bool isOnScreen = false;

		if (touch.position.x > Screen.width / 2)
			isOnScreen = true;

		return isOnScreen;
	}
}
