using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBoxIndicator : MonoBehaviour, ISelectHandler, ICancelHandler, IDeselectHandler
{
    //item id untuk ngerefresh indicatornya
    public int itemID;
    
    /// <summary>
    /// untuk swap indicator
    /// untuk ngelihat detail item
    /// </summary>
    public Item item;
    //untuk gambar di indicator, public untuk drag n drop
    public Image itemImage;
    //quantity text, public untuk drag n drop
    public Text quantityText;
    //kondisi jika sedang selected
    bool isSelected;
    //dipake kalau sedang di submit (ingin di swap), public untuk drag n drop
    public GameObject markIndicator;

    private void Start()
    {
        itemID = 0;
        markIndicator.SetActive(false);
        isSelected = false;
    }

    private void Update()
    {
        //jika sedang dipilih indicatornya
        if (isSelected)
        {
            if (!InventoryBox.instance.isSwapping && !Inventory.instance.isSwapping)
            {
                if (Input.GetKeyDown(InputSetup.instance.X))
                {
                    if (UIManager.instance.uiState == UIState.InventoryAndInventoryBox)
                        InventoryBox.instance.SetInitialQuantityToPut();
                }
            }
        }
    }

    /// <summary>
    /// function yang dipanggil saat inventorybox sedang di refresh
    /// untuk merefresh setiap indicatornya
    /// </summary>
    public void Refresh()
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
                quantityText.text = item.quantity.ToString();
                quantityText.gameObject.SetActive(true);
                if (item.isASingleTool)
                {
                    quantityText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            MakeEmpty();
        }
    }

    /// <summary>
    /// function untuk kosongin indicator jika quantity 0 atau tidak ada itemnya
    /// </summary>
    public void MakeEmpty()
    {
        //trycatch jika masih ada itemnya namun quantity 0
        try
        {
            Debug.Log(item.itemName + " removed");
        }
        catch { }

        PlayerData.instance.inventoryBoxItem.Remove(item);
        item = null;
        itemID = 0;
        itemImage.overrideSprite = null;
        quantityText.text = 0.ToString();
        quantityText.gameObject.SetActive(false);
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
            UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIState.Gameplay));
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

        if (UIManager.instance.uiState == UIState.InventoryAndInventoryBox)
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
