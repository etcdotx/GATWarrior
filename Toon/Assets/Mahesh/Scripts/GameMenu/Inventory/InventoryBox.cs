using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour
{
    public static InventoryBox instance;

    //item description pada menu inventory box
    public ItemDescription inventoryBoxItemDescription;
    //indicator button guide pada menu inventory dan inventorybox
    public GameObject inventoryAndInventoryBoxButton;

    [Header("Inventory Box Settings")]
    //inventory box view untuk hide/show inventorybox
    public GameObject inventoryBoxView;
    //inventorybox view port untuk tempat kumpulan inventorybox indicator
    public GameObject inventoryBoxViewPort;
    //arraylist untuk total inventorybox indicator
    public InventoryBoxIndicator[] inventoryBoxIndicator;

    /// <summary>
    /// setelah melakukan function trade(konfirmasi kirim barang) atau function yang nantinya akan dibuat ...
    /// ... maka pointer yang ditunjuk adalah item yang barusan ditrade
    /// </summary>
    public GameObject lastSelectedInventory;

    /// <summary>
    /// untuk tukar menukar barang pada inventory view port
    /// </summary>
    InventoryBoxIndicator invenSwap1;
    InventoryBoxIndicator invenSwap2;
    Item itemSwap1;
    Item itemSwap2;

    /// <summary>
    /// kondisi jika sedang swapping inventonrybox
    /// jadi jika lagi swapping pada inventoryinventorybox, lalu mencet tombol back ...
    /// ... maka di cancel swap dulu, baru di exit
    /// </summary>
    public bool isSwapping;

    /// <summary>
    /// dipakai pada funtion onselect di inventorybox indicator
    /// menyatakan bahwa item yang sedang diselect adalah item tersebut
    /// </summary>
    public Item temporaryItem;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        inventoryBoxView = transform.GetChild(0).Find("InventoryBoxView").gameObject;
        inventoryBoxViewPort = inventoryBoxView.transform.Find("InventoryBoxViewPort").gameObject;
        inventoryBoxItemDescription = inventoryBoxView.transform.Find("InventoryBoxItemDescription").GetComponent<ItemDescription>();
        inventoryAndInventoryBoxButton = inventoryBoxView.transform.Find("InventoryAndInventoryBoxButton").gameObject;

        int h = inventoryBoxViewPort.transform.childCount;
        inventoryBoxIndicator = new InventoryBoxIndicator[h];
        for (int i = 0; i < h; i++)
        {
            inventoryBoxIndicator[i] = inventoryBoxViewPort.transform.GetChild(i).GetComponent<InventoryBoxIndicator>();
        }
    }

    public void Start()
    {
        inventoryBoxItemDescription.gameObject.SetActive(true);
        inventoryBoxView.SetActive(false);
        inventoryAndInventoryBoxButton.SetActive(false);
        RefreshInventoryBox();
    }

    /// <summary>
    /// function untuk select indicator pertama kali
    /// </summary>
    /// <param name="inventoryBoxIndicator">indicator pertama yang dipilih</param>
    public void Select1stItem(InventoryBoxIndicator inventoryBoxIndicator) {
        invenSwap1 = inventoryBoxIndicator;
        if (invenSwap1.item != null)
        {
            itemSwap1 = invenSwap1.item;
        }
        else
            itemSwap1 = null;


        invenSwap1.markIndicator.SetActive(true);
        isSwapping = true;
    }

    /// <summary>
    /// function untuk select indicator yang kedua
    /// lalu indicator yang pertama dan kedua di swap
    /// </summary>
    /// <param name="inventoryBoxIndicator2">indicator kedua yang dipilih</param>
    public void Select2ndItem(InventoryBoxIndicator inventoryBoxIndicator2) {
        invenSwap2 = inventoryBoxIndicator2;
        if (invenSwap2.item != null)
        {
            itemSwap2 = invenSwap2.item;
        }
        else
            itemSwap2 = null;

        invenSwap1.markIndicator.SetActive(false);
        isSwapping = false;
        Swap();
        RefreshInventoryBox();
    }

    /// <summary>
    /// function swap
    /// dilakukan setelah function Select1stItem dan Select2ndItem
    /// </summary>
    void Swap() {
        if (itemSwap1 != null && itemSwap2 != null)
        {
            invenSwap1.item = itemSwap2;
            invenSwap2.item = itemSwap1;
        }
        else if (itemSwap1 == null && itemSwap2 != null)
        {
            invenSwap1.item = itemSwap2;
            invenSwap2.item = null;
        }
        else if (itemSwap1 != null && itemSwap2 == null)
        {
            invenSwap1.item = null;
            invenSwap2.item = itemSwap1;
        }
    }

    /// <summary>
    /// function ketika tidak jadi swap
    /// </summary>
    public void CancelSwap()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        isSwapping = false;
        invenSwap1.markIndicator.SetActive(false);
    }

    /// <summary>
    /// function untuk mereset tradeindicator (kirim ke inventory)
    /// kalau dari inventory box ke inventory harus di cek dulu apakah inventory muat
    /// </summary>
    public void SetInitialQuantityToPut()
    {
        if (temporaryItem != null)
        {
            TradeIndicator.instance.sliderState = TradeIndicator.SliderState.InventoryBoxToInventory;
            bool thereIsSpace = false;

            //cari jika di inventory ada item yang sama dengan item yang akan ditaro
            for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
            {
                //jika id nya sama, berarti itemnya sama
                if (PlayerData.instance.inventoryItem[i].id == temporaryItem.id)
                    if (PlayerData.instance.inventoryItem[i].quantity == PlayerData.instance.inventoryItem[i].maxQuantityOnInventory)
                    {
                        Debug.Log("you cannot carry more " + temporaryItem.itemName);
                        return;
                    }
                    else
                    {
                        TradeIndicator.instance.slider.gameObject.SetActive(true);
                        TradeIndicator.instance.slider.minValue = 1;
                        TradeIndicator.instance.slider.value = 1;
                        TradeIndicator.instance.slider.maxValue = temporaryItem.quantity;
                        UIManager.instance.eventSystem.SetSelectedGameObject(TradeIndicator.instance.slider.gameObject);
                        if (temporaryItem.quantity > (temporaryItem.maxQuantityOnInventory - PlayerData.instance.inventoryItem[i].quantity))
                            TradeIndicator.instance.slider.maxValue = (temporaryItem.maxQuantityOnInventory - PlayerData.instance.inventoryItem[i].quantity);
                        return;
                    }
            }

            for (int i = 0; i < Inventory.instance.inventoryIndicator.Length; i++)
            {
                if (Inventory.instance.inventoryIndicator[i].item == null)
                    thereIsSpace = true;
            }

            if (!thereIsSpace)
                return;

            TradeIndicator.instance.slider.minValue = 1;
            TradeIndicator.instance.slider.value = 1;
            TradeIndicator.instance.slider.maxValue = temporaryItem.quantity;
            if (temporaryItem.quantity > temporaryItem.maxQuantityOnInventory)
                TradeIndicator.instance.slider.maxValue = temporaryItem.maxQuantityOnInventory;
            TradeIndicator.instance.slider.gameObject.SetActive(true);
            UIManager.instance.eventSystem.SetSelectedGameObject(TradeIndicator.instance.slider.gameObject);
        }
        else
        {
            Debug.Log("no item");
        }
    }

    /// <summary>
    /// function untuk inventory menaruh ke inventory box
    /// </summary>
    /// <param name="newItem">item yang dikirim dari inventory</param>
    /// <param name="quantity">jumlah item</param>
    public void PlaceItem(Item newItem, int quantity)
    {
        bool isItemExist = false;
        //check if item exist or not
        for (int i = 0; i < PlayerData.instance.inventoryBoxItem.Count; i++)
        {
            if (PlayerData.instance.inventoryBoxItem[i].id == newItem.id)
            {
                isItemExist = true;
                //quantity item ditambah quantity baru
                PlayerData.instance.inventoryBoxItem[i].quantity += quantity;
                newItem.quantity -= quantity;
                break;
            }
        }

        if (isItemExist == false)
        {
            Item newItemInBox = ScriptableObject.CreateInstance<Item>();
            newItemInBox.Duplicate(newItem);

            newItemInBox.quantity = quantity;
            PlayerData.instance.inventoryBoxItem.Add(newItemInBox);
            newItem.quantity -= quantity;
        }

        //check inventory ke quest
        for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
        {
            if (PlayerData.instance.inventoryItem[i].id == newItem.id)
            {
                PlayerData.instance.CheckNewItem(PlayerData.instance.inventoryItem[i]);
            }
        }
        Inventory.instance.RefreshInventory();
        RefreshInventoryBox();
    }

    /// <summary>
    /// untuk merefresh inventory box ketika ada perubahan item dll
    /// </summary>
    public void RefreshInventoryBox()
    {
        for (int i = 0; i < inventoryBoxIndicator.Length; i++)
        {
            if (inventoryBoxIndicator[i].itemID == 0)
            {
                try
                {
                    for (int j = 0; j < PlayerData.instance.inventoryBoxItem.Count; j++)
                    {
                        if (PlayerData.instance.inventoryBoxItem[j].isOnInventoryBox == false)
                        {
                            inventoryBoxIndicator[i].item = PlayerData.instance.inventoryBoxItem[j];
                            inventoryBoxIndicator[i].itemID = PlayerData.instance.inventoryBoxItem[j].id;

                            //ditandai jika dia sudah ditaro
                            PlayerData.instance.inventoryBoxItem[j].isOnInventoryBox = true;
                            break;
                        }
                    }
                }
                catch
                {
                    //Debug.Log("There is no item in " + i + " inventory box.");
                }
            }
            inventoryBoxIndicator[i].Refresh();
        }
        UsableItem.instance.GetUsableItem();
    }
}
