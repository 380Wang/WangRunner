using UnityEngine;
using System.Collections;

public class StorySceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ButtonClicked()
    {
        Application.LoadLevel("Main Menu");
    }
}
