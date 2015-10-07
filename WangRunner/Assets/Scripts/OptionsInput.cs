using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsInput : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () { 
		GameObject.Find ("Music Slider").GetComponent <Slider> ().value=AudioListener.volume ;
	
	}

	public void PressedCreditsButton()
	{
		//todo: go to credits
		Application.LoadLevel ("CreditsScreen");
	}

	public void PressedBackButton()
	{
		Application.LoadLevel ("Main Menu");
	}
	public void DecVolume()
	{
		if (AudioListener.volume > 0) 
		{
			AudioListener.volume = (AudioListener.volume - 0.1F);
		}
	}

	public void IncVolume()
	{
		if (AudioListener.volume < 1) {
			AudioListener.volume = (AudioListener.volume + 0.1F);
		}
	}

	public void SliderChange()
	{
		
		AudioListener.volume = GameObject.Find ("Music Slider").GetComponent <Slider> ().value;
	}

}
