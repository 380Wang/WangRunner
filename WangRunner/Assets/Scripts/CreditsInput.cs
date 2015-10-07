using UnityEngine;
using System.Collections;

public class CreditsInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void PressedBackButton()
	{
		Application.LoadLevel ("Options");
	}
}
