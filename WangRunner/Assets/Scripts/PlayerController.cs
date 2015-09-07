using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	bool isJumping = false;
	Rigidbody2D rb;

	public List<BoxCollider2D> platforms = new List<BoxCollider2D>();
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		GameObject[] groundStuff = GameObject.FindGameObjectsWithTag ("Ground");

		for (int i = 0; i < groundStuff.Length; i++) {
			platforms.Add(groundStuff[i].GetComponent<BoxCollider2D>());
		}
	}

	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < platforms.Count; i++) {
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
		}

	}
}
