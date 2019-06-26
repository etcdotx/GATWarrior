using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIndicator : MonoBehaviour
{
    public PlayerData playerData;
    public UsableItem usableItem;

    public int itemID;
    public Item item;
    public Image itemImage;

    public bool isSelected;
    public bool marked;
    public GameObject markIndicator;
    public GameObject selectIndicator;

    private void Start()
    {
        selectIndicator.SetActive(false);
        markIndicator.SetActive(false);
    }

    public void RefreshInventory()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        usableItem = GameObject.FindGameObjectWithTag("UsableItem").GetComponent<UsableItem>();
        if (item != null)
        {
            if (item.quantity == 0)
            {
                playerData.inventoryItem.Remove(item);
                usableItem.isUsingItem = false;
                MakeEmpty();
            }
            else
            {
                itemImage.overrideSprite = item.itemImage;
                transform.GetChild(0).GetComponent<Text>().text = item.quantity.ToString();
                transform.GetChild(0).gameObject.SetActive(true);
                if (item.isASingleTool)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            MakeEmpty();
        }
    }

    public void RefreshInventoryBox()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        if (item != null)
        {
            if (item.quantity == 0)
            {
                playerData.inventoryBoxItem.Remove(item);
                MakeEmpty();
            }
            else
            {
                itemImage.overrideSprite = item.itemImage;
                transform.GetChild(0).GetComponent<Text>().text = item.quantity.ToString();
                transform.GetChild(0).gameObject.SetActive(true);
                if (item.isASingleTool)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            MakeEmpty();
        }
    }

    public void MakeEmpty()
    {
        try {
            Debug.Log(item.itemName + " removed");
        } catch { }
        playerData.inventoryItem.Remove(item);
        item = null;
        itemID = 0;
        itemImage.overrideSprite = null;
        transform.GetChild(0).GetComponent<Text>().text = 0.ToString();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
