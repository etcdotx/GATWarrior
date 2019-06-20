using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemIndicator : MonoBehaviour
{
    public Item item;
    public int itemID;
    public bool isSelected;
    public Image itemImage;

    public void RefreshUsableItem()
    {
        if (item == null)
        {
            itemImage.overrideSprite = null;
        }
        else
        {
            itemImage.overrideSprite = item.itemImage;
        }     
    }
}
