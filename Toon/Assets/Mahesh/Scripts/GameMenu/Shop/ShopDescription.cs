using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopDescription : MonoBehaviour
{
    public Item item;
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI inventoryQuantity;
    public TextMeshProUGUI inventoryBoxQuantity;

    public void RefreshDescription()
    {
        itemImage.overrideSprite = item.itemImage;
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        gold.text = item.price.ToString();

        inventoryBoxQuantity.text = "0";
        inventoryQuantity.text = "0/"+item.maxQuantityOnInventory.ToString();

        for (int i = 0; i < PlayerData.instance.inventoryBoxItem.Count; i++)
        {
            if (item.id == PlayerData.instance.inventoryBoxItem[i].id)
            {
                inventoryBoxQuantity.text = PlayerData.instance.inventoryBoxItem[i].quantity.ToString();
                break;
            }
        }
        for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
        {
            if (item.id == PlayerData.instance.inventoryItem[i].id)
            {
                inventoryQuantity.text = PlayerData.instance.inventoryItem[i].quantity.ToString() + "/" + PlayerData.instance.inventoryItem[i].maxQuantityOnInventory.ToString();
                break;
            }
        }
    }
}
