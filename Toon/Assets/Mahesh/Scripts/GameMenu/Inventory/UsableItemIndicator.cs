using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemIndicator : MonoBehaviour
{
    //item pada usable item
    public Item item;
    //image pada indicator
    public Image itemImage;

    /// <summary>
    /// function untuk refresh image ketika sedang milih usable item
    /// </summary>
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
