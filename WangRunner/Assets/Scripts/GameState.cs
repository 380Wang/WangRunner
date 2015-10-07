using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public bool paused = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**
	 * Pauses or unpauses game
	 **/
	public void PauseGame(){
		if (paused) {
			Time.timeScale = 1;
			paused = !paused;
		}
		else {
			Time.timeScale = 0;
			paused = !paused;
		}
	}

	public void BackButtonPressed(){

		Application.LoadLevel ("Main Menu");
	}
}
