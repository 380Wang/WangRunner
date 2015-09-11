using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 3.0f;
	public float jumpForce = 200.0f;
	public float jetpackForce = 100.0f;
	public Transform groundCheckTransform;
	public LayerMask groundCheckLayerMask;
	
	private bool grounded = false;
	private Rigidbody2D rb;
	private float clickDelay;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		clickDelay = 0.05f;
	}

	
	// Update is called once per frame
	void Update () {

		/*for (int i = 0; i < platforms.Count; i++) {
			if(rb.IsTouching(platforms[i]))
			{
				isJumping = false;
			}
		}

		if(Input.GetKeyDown(KeyCode.Space) && rb.IsTouchingLayers())
		{
			rb.AddForce (new Vector2 (0, 200));
			transform.Translate(new Vector3(0, 1, 0));
			isJumping = true;
		}*/

	}

	void FixedUpdate(){

		// Constantly moves player to the right
		Vector2 newVelocity = rb.velocity;
		newVelocity.x = movementSpeed;
		rb.velocity = newVelocity;

		// Click delay hack to prevent accidental double jumps
		clickDelay -= Time.fixedDeltaTime;
		bool pressedJump = false;

		//== FOR DESKTOP: CONTROLS ==//
		if ( Input.GetKeyDown(KeyCode.RightArrow) )
			pressedJump = (clickDelay <= 0);
		if (Input.GetKey (KeyCode.LeftArrow))
			rb.AddForce (new Vector2 (0, 200));
		//=============================//

		// Will jump when right side of screen is tapped
		int tCount = Input.touchCount;
		if (tCount > 0){
			foreach( var touch in Input.touches ){
				if( touch.position.x > Screen.width / 2 )
					pressedJump = (clickDelay <= 0) && ScreenIsTapped();
				else{
					rb.AddForce( new Vector2( 0, 10 ) );
				}
			}
		}
		if(pressedJump && grounded){
			clickDelay = 0.05f;
			rb.AddForce (new Vector2 (0, jumpForce));
		}

		UpdateGroundedStatus ();
	}

	void UpdateGroundedStatus(){
		grounded = Physics2D.OverlapCircle (groundCheckTransform.position, 0.1f, groundCheckLayerMask);
	}

	bool ScreenIsTapped(){
		return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
	}
}
