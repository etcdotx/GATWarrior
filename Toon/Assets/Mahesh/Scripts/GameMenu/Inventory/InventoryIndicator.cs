using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryIndicator : MonoBehaviour, ISelectHandler, ICancelHandler, IDeselectHandler
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
                        Inventory.instance.SetInitialQuantityToPut();
                }
            }
        }
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
        if (Inventory.instance.isSwapping)
        {
            Inventory.instance.CancelSwap();
        }
        else if (InventoryBox.instance.isSwapping)
        {
            InventoryBox.instance.CancelSwap();
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
        Inventory.instance.inventoryItemDescription.item = item;
        Inventory.instance.inventoryItemDescription.RefreshDescription();


        Inventory.instance.lastSelectedInventory = gameObject;
        Inventory.instance.temporaryItem = null;
        Inventory.instance.temporaryItem = item;

        if (UIManager.instance.uiState == UIManager.UIState.InventoryAndSave)
        {
            Inventory.instance.inventoryAndSaveButton.SetActive(true);
        }
        else if (UIManager.instance.uiState == UIManager.UIState.InventoryAndInventoryBox)
        {
            Inventory.instance.inventoryAndInventoryBoxButton.SetActive(true);
        }
        isSelected = true;
    }

    public void SwapInventory()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        if (!Inventory.instance.isSwapping)
            Inventory.instance.Select1stItem(this);
        else
            Inventory.instance.Select2ndItem(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        Inventory.instance.inventoryAndSaveButton.SetActive(false);
        Inventory.instance.inventoryAndInventoryBoxButton.SetActive(false);
    }
}
