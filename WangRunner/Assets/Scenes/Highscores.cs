using UnityEngine;
using System.Collections;

public class Highscores : MonoBehaviour {
    //HAVING AN ISSUE RUNNING
	//THIS ERROR KEEPS APPEARING: 
	// Couldn't set project path to: C:\Users\Brian\Documents\GitHub\WangRunner\WangRunner\Assets

	const string privateCode = "a6eOZmVIr02e3o_0Mx5JkQ7tq39vnbHkuH9beLZ2m9tw";
	const string publicCode = "561805416e51b60070b8afa0";
	const string webURL = "http://dreamlo.com/lb/";

	void Awake() {
		AddNewHighscore ("Brian", 999);
		AddNewHighscore ("Nathan", 4);
		AddNewHighscore ("Ben", 42);
	}

	public void AddNewHighscore (string username, int score){ 
		StartCoroutine(UploadNewHighscore(username, score));
    }

	IEnumerator UploadNewHighscore(string username, int score){
		WWW www = new WWW (webURL + privateCode + "/add/" + WWW.EscapeURL (username) + "/" + score);
		yield return www;

		if (string.IsNullOrEmpty (www.error))
			print ("Upload Successful");
		else {
			print ("Error uploading: " + www.error);
		}
	}
}
