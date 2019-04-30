using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIndicator : MonoBehaviour
{
    public PlayerData playerData;

    public int itemID;
    public bool isSelected;
    public Image itemImage;

    public void RefreshInventory()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();

        if (itemID == 0)
        {
            MakeEmpty();
        }
        else
        {
            for (int i = 0; i < playerData.inventoryItem.Count; i++)
            {
                if (playerData.inventoryItem[i].id == itemID)
                {
                    if (playerData.inventoryItem[i].quantity == 0)
                    {
                        playerData.inventoryItem.RemoveAt(i);
                        MakeEmpty();
                        break;
                    }
                    itemImage.sprite = playerData.inventoryItem[i].itemImage;
                    transform.GetChild(0).GetComponent<Text>().text = playerData.inventoryItem[i].quantity.ToString();
                    transform.GetChild(0).gameObject.SetActive(true);
                    if (playerData.inventoryItem[i].isASingleTool == true)
                    {
                        transform.GetChild(0).gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }
    }

    public void RefreshItemBox()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        if (itemID == 0)
        {
            MakeEmpty();
        }
        else
        {
            for (int i = 0; i < playerData.inventoryBoxItem.Count; i++)
            {
                if (playerData.inventoryBoxItem[i].id == itemID)
                {
                    if (playerData.inventoryBoxItem[i].quantity == 0)
                    {
                        playerData.inventoryBoxItem.RemoveAt(i);
                        MakeEmpty();
                        break;
                    }
                    itemImage.sprite = playerData.inventoryBoxItem[i].itemImage;
                    transform.GetChild(0).GetComponent<Text>().text = playerData.inventoryBoxItem[i].quantity.ToString();
                    transform.GetChild(0).gameObject.SetActive(true);
                    if (playerData.inventoryBoxItem[i].isASingleTool == true)
                    {
                        transform.GetChild(0).gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }
    }

    public void MakeEmpty()
    {
        itemID = 0;
        itemImage.sprite = null;
        transform.GetChild(0).GetComponent<Text>().text = 0.ToString();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
