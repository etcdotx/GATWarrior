using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public int itemID;
    public Image itemImage;
    public bool isSelected;
    public Player player;
    public ItemBox itemBox;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        itemBox = GameObject.FindGameObjectWithTag("ItemBoxScript").GetComponent<ItemBox>();
    }

    public void RefreshInventory()
    {
        for (int i = 0; i < player.item.Count; i++)
        {
            if (player.item[i].id == itemID)
            {
                itemImage.sprite = player.item[i].itemImage;
                transform.GetChild(0).GetComponent<Text>().text = player.item[i].quantity.ToString();
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            }
        }
        if (itemID == 0)
        {
            itemImage.sprite = null;
            transform.GetChild(0).GetComponent<Text>().text = 0.ToString();
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void RefreshItemBox()
    {
        for (int i = 0; i < itemBox.item.Count; i++)
        {
            if (itemBox.item[i].id == itemID)
            {
                itemImage.sprite = itemBox.item[i].itemImage;
                transform.GetChild(0).GetComponent<Text>().text = itemBox.item[i].quantity.ToString();
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            }
        }
        if (itemID == 0)
        {
            itemImage.sprite = null;
            transform.GetChild(0).GetComponent<Text>().text = 0.ToString();
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
