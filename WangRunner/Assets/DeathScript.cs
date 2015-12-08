using UnityEngine;
using System.Collections;

public class DeathScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
	
	}

    //call this function when the player dies so the scores can properly be updated
    public void Died(int distance, int coins, int powerups)
    {
        int currentCoins = PlayerPrefs.GetInt("CurrentCoins",0);

        currentCoins += coins;

        PlayerPrefs.SetInt("CurrentCoins", currentCoins);

        int[] highScoreDistances = new int[3];
        int[] highScoreCoins = new int[3];
        int[] highScorePowerups = new int[3];

        for (int i = 0; i < highScoreDistances.Length; i++)
        {
            highScoreDistances[i] = PlayerPrefs.GetInt(string.Format("HighScoreDistance{0}", i + 1));
            highScoreCoins[i] = PlayerPrefs.GetInt(string.Format("HighScoreCoins{0}", i + 1));
            highScorePowerups[i] = PlayerPrefs.GetInt(string.Format("HighScorePowerups{0}", i + 1));
        }

        for (int i = 0; i < highScoreDistances.Length; i++)
        {
            if(distance > highScoreDistances[i])
            {
                //place score, and sink scores down 1
                int temp = highScoreDistances[i];
                highScoreDistances[i] = distance;
                distance = temp;
            }

            if (coins > highScoreCoins[i])
            {
                //place score, and sink scores down 1
                int temp = highScoreCoins[i];
                highScoreCoins[i] = coins;
                coins = temp;
            }

            if (powerups > highScorePowerups[i])
            {
                //place score, and sink scores down 1
                int temp = highScorePowerups[i];
                highScorePowerups[i] = powerups;
                powerups = temp;
            }
        }

        for (int i = 0; i < highScoreDistances.Length; i++)
        {
            PlayerPrefs.SetInt(string.Format("HighScoreDistance{0}", i + 1), highScoreDistances[i]);
            PlayerPrefs.SetInt(string.Format("HighScoreCoins{0}", i + 1), highScoreCoins[i]);
            PlayerPrefs.SetInt(string.Format("HighScorePowerups{0}", i + 1), highScorePowerups[i]);
        }
    }
}
