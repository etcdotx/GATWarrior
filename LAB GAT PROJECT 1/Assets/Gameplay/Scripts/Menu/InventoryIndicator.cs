using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIndicator : MonoBehaviour
{
    public PlayerData playerData;
    public InventoryBox inventoryBox;

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
            for (int i = 0; i < playerData.item.Count; i++)
            {
                Debug.Log(playerData.item.Count);
                Debug.Log(playerData.item[i].name + " " + playerData.item[i].quantity);
                if (playerData.item[i].id == itemID)
                {
                    Debug.Log(playerData.item[i].id + " = " + itemID);
                    if (playerData.item[i].quantity == 0)
                    {
                        Debug.Log("ok");
                        playerData.item.RemoveAt(i);
                        Debug.Log(playerData.item.Count);
                        MakeEmpty();
                        break;
                    }
                    itemImage.sprite = playerData.item[i].itemImage;
                    transform.GetChild(0).GetComponent<Text>().text = playerData.item[i].quantity.ToString();
                    transform.GetChild(0).gameObject.SetActive(true);
                    break;
                }
            }
            Debug.Log(playerData.item.Count);
        }
    }

    public void RefreshItemBox()
    {
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();
        if (itemID == 0)
        {
            MakeEmpty();
        }
        else
        {
            for (int i = 0; i < inventoryBox.item.Count; i++)
            {
                Debug.Log(inventoryBox.item[i].name + " " + inventoryBox.item[i].quantity);
                if (inventoryBox.item[i].id == itemID)
                {
                    if (inventoryBox.item[i].quantity == 0)
                    {
                        inventoryBox.item.RemoveAt(i);
                        Debug.Log(inventoryBox.item.Count);
                        MakeEmpty();
                        break;
                    }
                    itemImage.sprite = inventoryBox.item[i].itemImage;
                    transform.GetChild(0).GetComponent<Text>().text = inventoryBox.item[i].quantity.ToString();
                    transform.GetChild(0).gameObject.SetActive(true);
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
