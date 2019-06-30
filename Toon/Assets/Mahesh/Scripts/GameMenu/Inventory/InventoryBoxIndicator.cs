using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBoxIndicator : MonoBehaviour, ISelectHandler, ICancelHandler, IDeselectHandler
{
    public int itemID;
    public Item item;
    public Image itemImage;

    public bool isSelected;
    public bool marked;
    public GameObject markIndicator;

    private void Start()
    {
        markIndicator.SetActive(false);
        isSelected = false;
    }

    private void Update()
    {
        if (isSelected)
        {
            if (!InventoryBox.instance.isSwapping && !Inventory.instance.isSwapping)
            {
                if (Input.GetKeyDown(InputSetup.instance.X))
                {
                    if (UIManager.instance.uiState == UIManager.UIState.InventoryAndInventoryBox)
                        InventoryBox.instance.SetInitialQuantityToPut();
                }
            }
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
        try
        {
            Debug.Log(item.itemName + " removed");
        }
        catch { }
        PlayerData.instance.inventoryBoxItem.Remove(item);
        item = null;
        itemID = 0;
        itemImage.overrideSprite = null;
        transform.GetChild(0).GetComponent<Text>().text = 0.ToString();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnCancel(BaseEventData eventData)
    {
        if (InventoryBox.instance.isSwapping)
        {
            InventoryBox.instance.CancelSwap();
        }
        else if (Inventory.instance.isSwapping)
        {
            Inventory.instance.CancelSwap();
        }
        else
        {
            UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIManager.UIState.Gameplay));
            isSelected = false;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
        InventoryBox.instance.inventoryBoxItemDescription.item = item;
        InventoryBox.instance.inventoryBoxItemDescription.RefreshDescription();


        InventoryBox.instance.lastSelectedInventory = gameObject;
        InventoryBox.instance.temporaryItem = null;
        InventoryBox.instance.temporaryItem = item;

        if (UIManager.instance.uiState == UIManager.UIState.InventoryAndInventoryBox)
        {
            InventoryBox.instance.inventoryAndInventoryBoxButton.SetActive(true);
        }
        isSelected = true;
        Debug.Log("in");
    }

    public void SwapInventoryBox()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        if (!InventoryBox.instance.isSwapping)
            InventoryBox.instance.Select1stItem(this);
        else
            InventoryBox.instance.Select2ndItem(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        InventoryBox.instance.inventoryAndInventoryBoxButton.SetActive(false);
    }
}
