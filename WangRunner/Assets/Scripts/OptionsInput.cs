using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsInput : MonoBehaviour {

    Slider musicSlider;
	// Use this for initialization
	void Start () {
        musicSlider = GameObject.Find("Music Slider").GetComponent<Slider>();
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume", 1);
        Debug.Log(PlayerPrefs.GetFloat("musicVolume", 100));
    }
	
	// Update is called once per frame
	void Update () { 
		musicSlider.value=AudioListener.volume;

	
	}

	public void PressedCreditsButton()
	{
        
		Application.LoadLevel ("CreditsScreen");
	}

	public void PressedBackButton()
	{
		Application.LoadLevel ("Main Menu");
	}

	public void SliderChange()
	{
		AudioListener.volume = musicSlider.value;
        PlayerPrefs.SetFloat("musicVolume", AudioListener.volume + .01f);
        Debug.Log(PlayerPrefs.GetFloat("musicVolume", 100));
    }

}
