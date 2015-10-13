using UnityEngine;
using System.Collections;

public class MainMenuInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGamePressed(){ 

		Application.LoadLevel ("MainGame");
	}

	public void OptionsPressed(){
		Application.LoadLevel ("Options");
	}

    public void ShopPressed(){
        Application.LoadLevel("ShopScreen");
    }

    public void LeaderboardsPressed(){
        Application.LoadLevel("LeaderboardScreen");
    }
	
}
