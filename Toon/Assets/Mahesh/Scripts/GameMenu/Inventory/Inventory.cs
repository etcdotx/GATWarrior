using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [Header("Inventory Settings")]
    public int inventoryColumn;
    public int inventoryRow;
    public int inventoryColumnIndex;
    public int inventoryRowIndex;
    public int inventoryViewPortChildCount;
    public GameObject inventoryView;
    public GameObject inventoryViewPort;
    public InventoryIndicator[] inventoryIndicator;
    public InventoryIndicator[,] inventoryIndicatorPos; //5 column

    public InventoryIndicator invenSwap1;
    public InventoryIndicator invenSwap2;
    public Item itemSwap1;
    public Item itemSwap2;

    public bool isSwapping;
    public Item temporaryItem;
    public GameObject slider;
    public Slider inventoryQuantitySlider;
    public bool isSettingQuantity;

    //[Header("Swap Setting")]
    //public int swapIndex1;
    //public int swapIndex2;

    private void Awake()
    {
        //singleton
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        inventoryView = transform.GetChild(0).Find("InventoryView").gameObject;
        inventoryViewPort = inventoryView.transform.Find("InventoryViewPort").gameObject;

        int h = inventoryViewPort.transform.childCount;
        inventoryIndicator = new InventoryIndicator[h];
        for (int i = 0; i < h; i++)
        {
            inventoryIndicator[i] = inventoryViewPort.transform.GetChild(i).GetComponent<InventoryIndicator>();
        }
        slider = inventoryView.transform.Find("Slider").gameObject;
        inventoryQuantitySlider = slider.transform.Find("InventoryQuantitySlider").GetComponent<Slider>();

        inventoryViewPortChildCount = inventoryViewPort.transform.childCount;
        inventoryColumn = inventoryViewPort.GetComponent<GridLayoutGroup>().constraintCount;
        inventoryRow = inventoryViewPortChildCount / inventoryColumn;
        inventoryIndicatorPos = new InventoryIndicator[inventoryRow, inventoryColumn];
        inventoryColumnIndex = 0;
        inventoryRowIndex = 0;
        int c = 0;
        for (int i = 0; i < inventoryRow; i++)
        {
            for (int j = 0; j < inventoryColumn; j++)
            {
                inventoryIndicatorPos[i, j] = inventoryViewPort.transform.GetChild(c).GetComponent<InventoryIndicator>();
                c++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slider.SetActive(false);
    }

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

    public void CancelSwap() {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        isSwapping = false;
        invenSwap1.markIndicator.SetActive(false);
    }

    public void PutIntoInventoryBox()
    {
        if (!isSettingQuantity && !isSwapping)
        {
            SetInitialQuantityToPut();
        }
        else if (isSettingQuantity)
        {
            InventoryBox.instance.PlaceItem(temporaryItem, (int)inventoryQuantitySlider.value);
            slider.SetActive(false);
            isSettingQuantity = false;
        }
    }

    public void SetInitialQuantityToPut()
    {
        if (inventoryIndicatorPos[inventoryRowIndex, inventoryColumnIndex].item != null)
        {
            inventoryQuantitySlider.value = 1;
            temporaryItem = inventoryIndicatorPos[inventoryRowIndex, inventoryColumnIndex].item;
            slider.SetActive(true);
            inventoryQuantitySlider.minValue = 1;
            inventoryQuantitySlider.maxValue = temporaryItem.quantity;
            inventoryQuantitySlider.transform.Find("Text").GetComponent<Text>().text = inventoryQuantitySlider.value.ToString();
            isSettingQuantity = true;
        }
        else
        {
            Debug.Log("no item");
        }
    }

    public void IncreaseQuantityToPut()
    {
        inventoryQuantitySlider.value += 1;
        inventoryQuantitySlider.transform.Find("Text").GetComponent<Text>().text = inventoryQuantitySlider.value.ToString();
    }

    public void DecreaseQuantityToPut()
    {
        inventoryQuantitySlider.value -= 1;
        inventoryQuantitySlider.transform.Find("Text").GetComponent<Text>().text = inventoryQuantitySlider.value.ToString();
    }

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
            Item newItemIn = new Item(newItem.id, newItem.itemImage, newItem.itemName, newItem.description, newItem.maxQuantityOnInventory, 
                newItem.price, newItem.isUsable, newItem.isConsumable, newItem.isASingleTool, newItem.itemType);
            if (newItem.itemType != null)
                if (newItem.itemType.ToLower().Equals("seed".ToLower()))
                    newItemIn.plantID = newItem.plantID;

            newItemIn.quantity = quantity;
            PlayerData.instance.inventoryItem.Add(newItemIn);
            PlayerData.instance.CheckNewItem(PlayerData.instance.inventoryItem[PlayerData.instance.inventoryItem.Count-1]);
            newItem.quantity -= quantity;
        }

        InventoryBox.instance.RefreshInventoryBox();
        RefreshInventory();
    }

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
            inventoryIndicator[i].RefreshInventory();
        }
        UsableItem.instance.GetUsableItem();
    }
}
