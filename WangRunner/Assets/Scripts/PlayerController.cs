using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 3.0f;
	public float jumpForce = 200.0f;
	public Transform groundCheckTransform;
	public LayerMask groundCheckLayerMask;
	
	private bool grounded = false;
	private Rigidbody2D rb;
	private float clickDelay;

	//public List<BoxCollider2D> platforms = new List<BoxCollider2D>();
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		//ground = GetComponent<BoxCollider2D> ();

		/*GameObject[] groundStuff = GameObject.FindGameObjectsWithTag ("Ground");

		for (int i = 0; i < groundStuff.Length; i++) {
			platforms.Add(groundStuff[i].GetComponent<BoxCollider2D>());
		}*/
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
		Vector2 newVelocity = rb.velocity;

		newVelocity.x = movementSpeed;
		rb.velocity = newVelocity;

		clickDelay -= Time.fixedDeltaTime;
		bool pressed = Input.GetButtonDown ("Fire1") && (clickDelay <= 0);

		if(pressed && grounded){
			clickDelay = 1.0f;
			rb.AddForce (new Vector2 (0, jumpForce));
		}

		UpdateGroundedStatus ();
	}

	void UpdateGroundedStatus(){
		grounded = Physics2D.OverlapCircle (groundCheckTransform.position, 0.1f, groundCheckLayerMask);
	}
}
