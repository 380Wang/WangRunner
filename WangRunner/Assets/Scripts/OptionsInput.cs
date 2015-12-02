using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsInput : MonoBehaviour {

    Slider musicSlider;
    GameObject messageBox;
	void Start () {
        messageBox = GameObject.Find("MessageBox");
        messageBox.SetActive(false);
        musicSlider = GameObject.Find("Music Slider").GetComponent<Slider>();
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume", 1);
        Debug.Log(PlayerPrefs.GetFloat("musicVolume", 100));
    }
	
	
	void Update () { 
		musicSlider.value=AudioListener.volume;
	}

    public void ShowMessageBox()
    {
        messageBox.SetActive(true);
    }

    
    public void HideMessageBox()
    {
         messageBox.SetActive(false);
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt("Glider", 0);
        PlayerPrefs.SetInt("AirStabilizer", 0);
        PlayerPrefs.SetInt("BoostJet", 0);
        PlayerPrefs.SetInt("Uppercut", 0);
        PlayerPrefs.SetInt("DiveKick", 0);
        PlayerPrefs.SetInt("GrapplingHook", 0);
        PlayerPrefs.SetInt("Jetpack", 0);

        PlayerPrefs.SetInt("CurrentCoins", 0);

        PlayerPrefs.SetInt("FirstTimePlaying", 0);

        HideMessageBox();
    }

	public void PressedCreditsButton()
	{
        
		Application.LoadLevel ("CreditsScreen");
	}

	public void PressedBackButton()
	{
		Application.LoadLevel ("Main Menu");
	}

	public void DecVolume()
	{
			if (AudioListener.volume > 0) {
			AudioListener.volume = (AudioListener.volume - 0.1F);
			PlayerPrefs.SetFloat("musicVolume", AudioListener.volume + .01f);
		}
	}

	public void IncVolume()
	{
		if (AudioListener.volume < 1) 
		{
			AudioListener.volume = (AudioListener.volume + 0.1F);
			PlayerPrefs.SetFloat("musicVolume", AudioListener.volume + .01f);
		}
	}

	public void SliderChange()
	{
		AudioListener.volume = musicSlider.value;
        PlayerPrefs.SetFloat("musicVolume", AudioListener.volume + .01f);
    }

}
