using UnityEngine;
using System.Collections;

public class SingletonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume", 1);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
