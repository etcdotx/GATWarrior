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
    public PlayerData playerData;

    public void RefreshDescription()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        itemImage.overrideSprite = item.itemImage;
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        gold.text = item.price.ToString();

        inventoryBoxQuantity.text = "0";
        inventoryQuantity.text = "0/"+item.maxQuantityOnInventory.ToString();

        for (int i = 0; i < playerData.inventoryBoxItem.Count; i++)
        {
            if (item.id == playerData.inventoryBoxItem[i].id)
            {
                inventoryBoxQuantity.text = playerData.inventoryBoxItem[i].quantity.ToString();
                break;
            }
        }
        for (int i = 0; i < playerData.inventoryItem.Count; i++)
        {
            if (item.id == playerData.inventoryItem[i].id)
            {
                inventoryQuantity.text = playerData.inventoryItem[i].quantity.ToString() + "/" + playerData.inventoryItem[i].maxQuantityOnInventory.ToString();
                break;
            }
        }
    }
}
