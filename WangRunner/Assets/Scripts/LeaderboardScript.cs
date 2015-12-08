using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour
{
    public Text[] distanceTexts;

    public Text[] coinsTexts;

    public Text[] powerupsTexts;

    void Start()
    {
        int[] highScoreDistances = new int[3];
        int[] highScoreCoins = new int[3];
        int[] highScorePowerups = new int[3];

        for (int i = 0; i < highScoreDistances.Length; i++)
        {
            highScoreDistances[i] = PlayerPrefs.GetInt(string.Format("HighScoreDistance{0}", i + 1));
            highScoreCoins[i] = PlayerPrefs.GetInt(string.Format("HighScoreCoins{0}", i + 1));
            highScorePowerups[i] = PlayerPrefs.GetInt(string.Format("HighScorePowerups{0}", i + 1));
        }

        for(int i = 0; i < distanceTexts.Length; i++)
        {
            distanceTexts[i].text = string.Format("1st: {0}m", highScoreDistances[i]);
            coinsTexts[i].text = string.Format("1st: {0} coins", highScoreCoins[i]);
            powerupsTexts[i].text = string.Format("1st: {0} powerups", highScorePowerups[i]);
        }
    }

    void Update()
    {

    }

    public void BackPressed()
    {
        Application.LoadLevel("Main Menu");
    }
}
