using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopIndicator : MonoBehaviour
{
    public Item item;
    public Image image;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public GameObject selectedIndicator;

    public void RefreshIndicator()
    {
        itemNameText.text = item.itemName;
        priceText.text = item.price.ToString();
        image.sprite = item.itemImage;
    }
}
