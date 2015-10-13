using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleScreenInput : MonoBehaviour
{
    //the amount of time that's passed
    float elapsedTime = 0;

    //the amount of time between flashes
    float flashTime = 0.5f;
    
    //the flashing text in the title screen
    public Text tapText;


    void Start()
    {
        //AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, new Vector3(0, 0, 0));
    }

    void Update()
    {
        //keeping track of how much time has passed
        elapsedTime += Time.deltaTime;


        //has the amount of time passed to flash the thing?
        if (elapsedTime >= flashTime)
        {
            //reset the amount of time that's been used
            elapsedTime = 0;
            //disable/enable the text to flash it
            tapText.gameObject.SetActive(!tapText.gameObject.activeSelf);
        }
        //did the player tap anywhere on the screen? then swap to the maingame scene
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            Application.LoadLevel("Main Menu");
        }
    }
}
