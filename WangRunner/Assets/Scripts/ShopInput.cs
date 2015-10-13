using UnityEngine;
using System.Collections;

public class ShopInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BackPressed() {
        Application.LoadLevel("Main Menu");
    }
}
