using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryIndicator : MonoBehaviour, ICancelHandler
{
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
        if (item != null)
        {
            if (item.quantity == 0)
            {
                PlayerData.instance.inventoryItem.Remove(item);
                UsableItem.instance.isUsingItem = false;
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
        if (item != null)
        {
            if (item.quantity == 0)
            {
                PlayerData.instance.inventoryBoxItem.Remove(item);
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
        PlayerData.instance.inventoryItem.Remove(item);
        item = null;
        itemID = 0;
        itemImage.overrideSprite = null;
        transform.GetChild(0).GetComponent<Text>().text = 0.ToString();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnCancel(BaseEventData eventData)
    {
        Inventory.instance.inventoryView.SetActive(false);
        Quest.instance.questView.SetActive(false);
        UsableItem.instance.usableItemView.SetActive(true);
        UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIManager.UIState.Gameplay));
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);
    }
}
