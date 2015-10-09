using UnityEngine;
using System.Collections;

public class SingletonScript : MonoBehaviour {

	//MyUnitySingleton single;
	// Use this for initialization
	void Start () {
		//single = new MyUnitySingleton ();
		DontDestroyOnLoad(this.gameObject); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
