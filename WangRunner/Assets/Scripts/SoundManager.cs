using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource efxSource;                   
	public AudioSource musicSource;                
	public static SoundManager instance = null;

	// Use this for initialization
	void Start () {
	
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySingle(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		efxSource.clip = clip;
		
		//Play the clip.
		efxSource.Play ();
	}
}
