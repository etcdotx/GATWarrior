using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public int itemID;
    public Image itemImage;
    public bool isSelected;

    public void RefreshItem()
    {
        for (int i = 0; i < ItemDataBase.item.Count; i++)
        {
            if (ItemDataBase.item[i].id == itemID)
            {
                itemImage.sprite = ItemDataBase.item[i].itemImage;
                transform.GetChild(0).GetComponent<Text>().text = ItemDataBase.item[i].quantity.ToString();
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
