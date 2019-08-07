using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    //item description pada menu inventory
    public ItemDescription inventoryItemDescription;
    //indicator button guide pada menu inventory dan save
    public GameObject inventoryAndSaveButton;
    //indicator button guide pada menu inventory dan inventory box
    public GameObject inventoryAndInventoryBoxButton;

    [Header("Inventory Settings")]
    //inventory view untuk hide/show inventory
    public GameObject inventoryView;
    //inventory viewport untuk tempat kumpulan inventoryindicator
    public GameObject inventoryViewPort;
    //arraylist untuk total inventoryindicator
    public InventoryIndicator[] inventoryIndicator;
    
    /// <summary>
    /// setelah melakukan function trade(konfirmasi kirim barang) atau function yang nantinya akan dibuat ...
    /// ... maka pointer yang ditunjuk adalah item yang barusan ditrade
    /// </summary>
    public GameObject lastSelectedInventory;

    /// <summary>
    /// untuk tukar menukar barang pada inventory view port
    /// </summary>
    InventoryIndicator invenSwap1;
    InventoryIndicator invenSwap2;
    Item itemSwap1;
    Item itemSwap2;
    
    /// <summary>
    /// kondisi jika sedang swapping inventory
    /// jadi jika lagi swapping pada inventorysave/inventoryinventorybox, lalu mencet tombol back ...
    /// ... maka di cancel swap dulu, baru di exit
    /// </summary>
    public bool isSwapping;
    
    /// <summary>
    /// dipakai pada funtion onselect di inventoryindicator
    /// menyatakan bahwa item yang sedang diselect adalah item tersebut
    /// </summary>
    public Item temporaryItem;

    private void Awake()
    {
        //set singleton
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        //get objectnya
        inventoryView = transform.GetChild(0).Find("InventoryView").gameObject;
        inventoryViewPort = inventoryView.transform.Find("InventoryViewPort").gameObject;
        inventoryItemDescription = inventoryView.transform.Find("InventoryItemDescription").GetComponent<ItemDescription>();
        inventoryAndSaveButton = inventoryView.transform.Find("InventoryAndSaveButton").gameObject;
        inventoryAndInventoryBoxButton = inventoryView.transform.Find("InventoryAndInventoryBoxButton").gameObject;

        //hitung ada berapa indicator pada viewport
        int h = inventoryViewPort.transform.childCount;
        inventoryIndicator = new InventoryIndicator[h];
        for (int i = 0; i < h; i++)
        {
            inventoryIndicator[i] = inventoryViewPort.transform.GetChild(i).GetComponent<InventoryIndicator>();
        }
    }

    private void Start()
    {
        inventoryItemDescription.gameObject.SetActive(true);
        inventoryView.SetActive(false);
        inventoryAndSaveButton.SetActive(false);
        inventoryAndInventoryBoxButton.SetActive(false);
    }

    /// <summary>
    /// function untuk select indicator pertama kali
    /// </summary>
    /// <param name="inventoryIndicator1">indicator pertama yang dipilih</param>
    public void Select1stItem(InventoryIndicator inventoryIndicator1) {
        invenSwap1 = inventoryIndicator1;
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
    /// <param name="inventoryIndicator2">indicator kedua yang dipilih</param>
    public void Select2ndItem(InventoryIndicator inventoryIndicator2) {
        invenSwap2 = inventoryIndicator2;
        if (invenSwap2.item != null)
        {
            itemSwap2 = invenSwap2.item;
        }
        else
            itemSwap2 = null;


        invenSwap1.markIndicator.SetActive(false);
        isSwapping = false;
        Swap();
        RefreshInventory();
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
        else if (itemSwap1 == null && itemSwap2!=null)
        {
            invenSwap1.item = itemSwap2;
            invenSwap2.item = null;
        }
        else if (itemSwap1 != null && itemSwap2==null)
        {
            invenSwap1.item = null;
            invenSwap2.item = itemSwap1;
        }
    }

    /// <summary>
    /// function ketika tidak jadi swap
    /// </summary>
    public void CancelSwap() {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        isSwapping = false;
        invenSwap1.markIndicator.SetActive(false);
    }

    /// <summary>
    /// function untuk mereset tradeindicator (kirim inventory box)
    /// berbeda dengan dari inventory box ke inventory
    /// </summary>
    public void SetInitialQuantityToPut()
    {
        if (temporaryItem != null)
        {
            TradeIndicator.instance.sliderState = TradeIndicator.SliderState.InventoryToInventoryBox;
            TradeIndicator.instance.slider.minValue = 1;
            TradeIndicator.instance.slider.value = 1;
            TradeIndicator.instance.slider.maxValue = temporaryItem.quantity;
            TradeIndicator.instance.slider.gameObject.SetActive(true);
            UIManager.instance.eventSystem.SetSelectedGameObject(TradeIndicator.instance.slider.gameObject);
        }
        else
        {
            Debug.Log("no item");
        }
    }

    /// <summary>
    /// function untuk inventory box menaruh item ke inventory
    /// </summary>
    /// <param name="newItem">item yang dikirim dari inventory box</param>
    /// <param name="quantity">jumlah item</param>
    public void PlaceItem(Item newItem, int quantity)
    {
        bool isItemExist = false;
        //check if item exist or not
        for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
        {
            if (PlayerData.instance.inventoryItem[i].id == newItem.id)
            {
                isItemExist = true;
                //quantity item ditambah quantity baru
                PlayerData.instance.inventoryItem[i].quantity += quantity;
                PlayerData.instance.CheckNewItem(PlayerData.instance.inventoryItem[i]);
                newItem.quantity -= quantity;
                break;
            }
        }

        if (isItemExist == false)
        {
            Item newItemIn = ScriptableObject.CreateInstance<Item>();
            newItemIn.Duplicate(newItem);

            newItemIn.quantity = quantity;
            PlayerData.instance.inventoryItem.Add(newItemIn);
            PlayerData.instance.CheckNewItem(PlayerData.instance.inventoryItem[PlayerData.instance.inventoryItem.Count-1]);
            newItem.quantity -= quantity;
        }

        InventoryBox.instance.RefreshInventoryBox();
        RefreshInventory();
    }

    /// <summary>
    /// untuk merefresh inventory ketika ada perubahan item dll
    /// </summary>
    public void RefreshInventory()
    {        
        for (int i = 0; i < inventoryIndicator.Length; i++)
        {
            if (inventoryIndicator[i].item == null)
            {
                try
                {
                    for (int j = 0; j < PlayerData.instance.inventoryItem.Count; j++)
                    {
                        if (PlayerData.instance.inventoryItem[j].isOnInventory == false)
                        {
                            inventoryIndicator[i].item = PlayerData.instance.inventoryItem[j];
                            inventoryIndicator[i].itemID = PlayerData.instance.inventoryItem[j].id;

                            //ditandai jika dia sudah ditaro
                            PlayerData.instance.inventoryItem[j].isOnInventory = true;
                            break;
                        }
                    }
                }
                catch
                {
                    //Debug.Log("There is no item in " + i + " inventory slot.");
                }
            }
            inventoryIndicator[i].Refresh();
        }
        UsableItem.instance.GetUsableItem();
    }
}
