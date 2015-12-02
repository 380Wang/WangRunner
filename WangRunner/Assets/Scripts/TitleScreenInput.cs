using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleScreenInput : MonoBehaviour
{
    public Text ActionAbilityText;

    public Text JumpAbilityText;

    //the amount of time that's passed
    float elapsedTime = 0;

    //the amount of time between flashes
    float flashTime = 0.5f;
    
    //the flashing text in the title screen
    public Text tapText;

    bool tutorialEnabled = false;

    GameObject tutorialOverlay;
    GameObject titleTextGroup;

    GeneratorScript levelGen;

    public GameObject basicJumpLevel;

    void Start()
    {
        //AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, new Vector3(0, 0, 0));
        tutorialOverlay = GameObject.Find("Tutorial");
        tutorialOverlay.SetActive(false);

        titleTextGroup = GameObject.Find("TitleTextGroup");

        levelGen = GameObject.Find("Player").GetComponent<GeneratorScript>();
        
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            ActionAbilityText.text = "Press Left to use your action ability (currently slide).";
            JumpAbilityText.text = "Press Right to use your jump ability (currently basic jump)";
        }
    }

    public void TransitionToMainMenu()
    {
        Application.LoadLevel("Main Menu");
        PlayerPrefs.SetInt("FirstTimePlaying", 1);
    }

    void Update()
    {

        if (!tutorialEnabled)
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
                //if it's the person's first time playing, they should learn the basic controls
                if (PlayerPrefs.GetInt("FirstTimePlaying", 0) == 0)
                {
                    tutorialOverlay.SetActive(true);
                    tutorialEnabled = true;
                    titleTextGroup.SetActive(false);

                    levelGen.basicLevels[0] = basicJumpLevel;
                }
                else
                {
                    //otherwise just play the game
                    TransitionToMainMenu();
                }

            }
        }
       
    }
}
