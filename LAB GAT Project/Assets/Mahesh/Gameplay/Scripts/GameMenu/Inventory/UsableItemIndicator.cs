using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemIndicator : MonoBehaviour
{
    public PlayerData playerData;

    public int itemID;
    public bool isSelected;
    public Image itemImage;

    public void RefreshUsableItem()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        if (itemID == 0)
        {
            itemImage.sprite = null;
        }
        for (int i = 0; i < playerData.inventoryItem.Count; i++)
        {
            if (playerData.inventoryItem[i].id == itemID)
            {
                itemImage.sprite = playerData.inventoryItem[i].itemImage;
                break;
            }
        }
    }
}
