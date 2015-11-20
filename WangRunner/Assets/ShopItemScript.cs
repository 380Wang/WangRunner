using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopItemScript : MonoBehaviour
{
    public string ItemName;
    public string Description;
    public int Price;
    public Sprite ItemImage;
    void OnValidate()
    {
        transform.FindChild("ItemName").GetComponent<Text>().text = ItemName;
        transform.FindChild("Description").GetComponent<Text>().text = Description;
        transform.FindChild("PriceText").GetComponent<Text>().text = string.Format("Price\n{0}", Price);
        transform.FindChild("Image").GetComponent<Image>().sprite = ItemImage;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
