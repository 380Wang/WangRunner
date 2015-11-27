using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopInput : MonoBehaviour {

    //keys are the variable names I'm calling whether you purchased an item or not
    //values are the names of the objects in the list of items to buy
    Dictionary<string, string> allPurchasableItems = new Dictionary<string, string>();

    Dictionary<string, int> itemPrices = new Dictionary<string, int>();
    public Text coinsText;

    GameObject messageBox;
    public Text messageBoxText;
	void Start () {

        coinsText.text = string.Format("Coins: {0}", PlayerPrefs.GetInt("CurrentCoins"));
        messageBox = GameObject.Find("MessageBox");
        messageBox.SetActive(false);

        allPurchasableItems.Add("Glider", "GliderShopItem");
        allPurchasableItems.Add("AirStabilizer", "AirStabilizerShopItem");
        allPurchasableItems.Add("BoostJet", "BoostJetShopItem");
        allPurchasableItems.Add("Uppercut", "UppercutShopItem");
        allPurchasableItems.Add("DiveKick", "DiveKickShopItem");
        allPurchasableItems.Add("GrapplingHook", "GrapplingHookShopItem");
        allPurchasableItems.Add("Jetpack", "JetpackShopItem");

        itemPrices.Add("Glider", 100);
        itemPrices.Add("AirStabilizer", 300);
        itemPrices.Add("BoostJet", 300);
        itemPrices.Add("Uppercut", 400);
        itemPrices.Add("DiveKick", 400);
        itemPrices.Add("GrapplingHook", 400);
        itemPrices.Add("Jetpack", 700);

        getRidOfPurchasedItems();
    }

    void getRidOfPurchasedItems()
    {
        //for every single purchasable item
        foreach (string item in allPurchasableItems.Keys)
        {
            //if we saved that the item was purchased (has a value > 0)
            if (PlayerPrefs.GetInt(item) > 0)
            {
                //remove the item from the list so we can't purchase again
                Destroy(GameObject.Find(allPurchasableItems[item]));
            }
        }
    }

    private void ShowPurchasedDialog()
    {
        messageBoxText.text = "Purchased!";
        messageBox.SetActive(true);
    }

    private void ShowNotEnoughMoneyDialog()
    {
        messageBoxText.text = "Not Enough Money!";
        messageBox.SetActive(true);
    }

    public void HideMessageBox()
    {
        messageBox.SetActive(false);
    }

    private void UpdateCoinsText()
    {
        coinsText.text = string.Format("Coins: {0}", PlayerPrefs.GetInt("CurrentCoins"));
    }

    public void PurchaseItem(string itemToBuy)
    {
        if(PlayerPrefs.GetInt("CurrentCoins") - itemPrices[itemToBuy] >= 0)
        {
            //buy it
            PlayerPrefs.SetInt("CurrentCoins", PlayerPrefs.GetInt("CurrentCoins") - itemPrices[itemToBuy]);
            PlayerPrefs.SetInt(itemToBuy, 1);

            ShowPurchasedDialog();
            UpdateCoinsText();
            getRidOfPurchasedItems();
        }
        else
        {
            //can't buy it
            ShowNotEnoughMoneyDialog();
        }

        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            PlayerPrefs.SetInt("CurrentCoins", 10000);
            UpdateCoinsText();
        }
	}

    public void BackPressed() {
        Application.LoadLevel("Main Menu");
    }
}
