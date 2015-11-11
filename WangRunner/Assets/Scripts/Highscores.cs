using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Highscores : MonoBehaviour {
	public Text currentScore;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void UpdateScore()
	{
		currentScore.text = "Distance: " + ((int)transform.position.x + 3) + "m";
	}
}
