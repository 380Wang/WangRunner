using UnityEngine;
using System.Collections;

public class TitleScreenInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ButtonTapped()
    {
        //swaps to the maingame scene
        Application.LoadLevel("MainGame");
    }
}
