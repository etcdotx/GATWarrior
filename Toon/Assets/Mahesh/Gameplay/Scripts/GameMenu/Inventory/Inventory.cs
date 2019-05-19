using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public PlayerData playerData;
    public InputSetup inputSetup;
    public GameMenuManager gameMenuManager;
    public InventoryBox inventoryBox;
    public UsableItem usableItem;

    [Header("Inventory Settings")]
    public int inventoryColumn;
    public int inventoryRow;
    public int inventoryColumnIndex;
    public int inventoryRowIndex;
    public int inventoryViewPortChildCount;
    public GameObject inventoryView;
    public GameObject inventoryViewPort;
    public GameObject[] inventoryIndicator;
    public GameObject[,] inventoryPos; //5 column

    public InventoryIndicator invenSwap1;
    public InventoryIndicator invenSwap2;
    public Item itemSwap1;
    public Item itemSwap2;

    public bool isSwapping;
    public Item temporaryItem;
    public GameObject slider;
    public Slider inventoryQuantitySlider;
    public bool isSettingQuantity;

    [Header("Swap Setting")]
    public int swapIndex1;
    public int swapIndex2;

    private void Awake()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        gameMenuManager = GameObject.FindGameObjectWithTag("GameMenuManager").GetComponent<GameMenuManager>();
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();
        usableItem = GameObject.FindGameObjectWithTag("UsableItem").GetComponent<UsableItem>();

        inventoryView = GameObject.FindGameObjectWithTag("InventoryUI").transform.Find("InventoryView").gameObject;
        inventoryViewPort = inventoryView.transform.Find("InventoryViewPort").gameObject;

        int h = inventoryViewPort.transform.childCount;
        inventoryIndicator = new GameObject[h];
        for (int i = 0; i < h; i++)
        {
            inventoryIndicator[i] = inventoryViewPort.transform.GetChild(i).gameObject;
        }
        slider = inventoryView.transform.Find("Slider").gameObject;
        inventoryQuantitySlider = slider.transform.Find("InventoryQuantitySlider").GetComponent<Slider>();

        inventoryViewPortChildCount = inventoryViewPort.transform.childCount;
        inventoryColumn = inventoryViewPort.GetComponent<GridLayoutGroup>().constraintCount;
        inventoryRow = inventoryViewPortChildCount / inventoryColumn;
        inventoryPos = new GameObject[inventoryRow, inventoryColumn];
        inventoryColumnIndex = 0;
        inventoryRowIndex = 0;
        int c = 0;
        for (int i = 0; i < inventoryRow; i++)
        {
            for (int j = 0; j < inventoryColumn; j++)
            {
                inventoryPos[i, j] = inventoryViewPort.transform.GetChild(c).gameObject;
                c++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slider.SetActive(false);
    }

    public void InventorySelection()
    {
        if (gameMenuManager.inputAxis.y == -1) // kebawah
        {
            if (inventoryRowIndex < inventoryRow - 1)
            {
                inventoryRowIndex++;
            }
            else
            {
                inventoryRowIndex = 0;
            }
        }
        else if (gameMenuManager.inputAxis.y == 1) // keatas
        {
            if (inventoryRowIndex > 0)
            {
                inventoryRowIndex--;
            }
            else
            {
                inventoryRowIndex = inventoryRow - 1;
            }
        }

        if (gameMenuManager.inputAxis.x == -1) // kekiri
        {
            if (inventoryColumnIndex > 0)
            {
                inventoryColumnIndex--;
            }
            else
            {
                inventoryColumnIndex = inventoryColumn - 1;
            }
        }
        else if (gameMenuManager.inputAxis.x == 1) //kekanan
        {
            if (inventoryColumnIndex < inventoryColumn - 1)
            {
                inventoryColumnIndex++;
            }
            else
            {
                inventoryColumnIndex = 0;
            }
        }
        MarkInventory();
    }

    public void MarkInventory()
    {
        for (int i = 0; i < inventoryRow; i++)
        {
            for (int j = 0; j < inventoryColumn; j++)
            {
                inventoryPos[i, j].GetComponent<InventoryIndicator>().markIndicator.SetActive(false);
            }
        }

        inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<InventoryIndicator>().markIndicator.SetActive(true);
    }

    public void InventorySwapping()
    {
        if (Input.GetKeyDown(inputSetup.select) && isSwapping == false)
        {
            Select1stItem();
        }
        else if (Input.GetKeyDown(inputSetup.select) && isSwapping == true)
        {
            Select2ndItem();
            Swap();
            //SetUsableItemPosition(); --> buat nyusun list nya
            RefreshInventory();
            ResetInventorySwap();
        }
    }

    void Select1stItem() {
        invenSwap1 = inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<InventoryIndicator>();
        if (invenSwap1.item != null)
        {
            itemSwap1 = invenSwap1.item;
        }
        else
            itemSwap1 = null;

        for (int i = 0; i < playerData.inventoryItem.Count; i++)
            if (invenSwap1.item == playerData.inventoryItem[i]){
                swapIndex1 = i;
                break;
            }


        invenSwap1.selectIndicator.SetActive(true);
        isSwapping = true;
        StartCoroutine(gameMenuManager.ButtonInputHold());
    }

    void Select2ndItem() {
        invenSwap2 = inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<InventoryIndicator>();
        if (invenSwap2.item != null)
        {
            itemSwap2 = invenSwap2.item;
        }
        else
            itemSwap2 = null;

        for (int i = 0; i < playerData.inventoryItem.Count; i++)
            if (invenSwap2.item == playerData.inventoryItem[i])
            {
                swapIndex2 = i;
                break;
            }
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

    void SetUsableItemPosition() {
        int placement = 0;
        if (itemSwap1 != null && itemSwap2 != null)
        {
            playerData.inventoryItem[swapIndex1] = itemSwap2;
            Debug.Log(playerData.inventoryItem[swapIndex1].itemName);
            playerData.inventoryItem[swapIndex2] = itemSwap1;
            Debug.Log(playerData.inventoryItem[swapIndex2].itemName);
        }
        else if (itemSwap1 != null && itemSwap2 == null)
        {
            bool stop = false;
            for (int i = 0; i < inventoryRow; i++)
            {
                for (int j = 0; j < inventoryColumn; j++)
                {
                    if (inventoryPos[i, j].GetComponent<InventoryIndicator>().item != null)
                    {
                        if (inventoryPos[i, j].GetComponent<InventoryIndicator>().item != itemSwap1 && !stop)
                        {
                            Debug.Log(inventoryPos[i, j].GetComponent<InventoryIndicator>().item.itemName + " != " + itemSwap1.itemName);
                            placement++;
                        }
                        else
                            stop = true;
                    }
                    if (stop)
                        break;
                }
                if (stop)
                    break;
            }
            playerData.inventoryItem.Remove(itemSwap1);
            playerData.inventoryItem.Insert(placement, itemSwap1);
        }
        else if (itemSwap1 == null && itemSwap2 != null)
        {
            bool stop = false;
            for (int i = 0; i < inventoryRow; i++)
            {
                for (int j = 0; j < inventoryColumn; j++)
                {
                    if (inventoryPos[i, j].GetComponent<InventoryIndicator>().item != null)
                    {
                        if (inventoryPos[i, j].GetComponent<InventoryIndicator>().item != itemSwap2 && !stop)
                        {
                            Debug.Log(inventoryPos[i, j].GetComponent<InventoryIndicator>().item.itemName + " != " + itemSwap2.itemName);
                            placement++;
                        }
                        else
                            stop = true;
                    }
                    if (stop)
                        break;
                }
                if (stop)
                    break;
            }
            playerData.inventoryItem.Remove(itemSwap2);
            playerData.inventoryItem.Insert(placement, itemSwap2);
        }
    }

    public void ResetInventorySwap()
    {
        invenSwap1.selectIndicator.SetActive(false);
        isSwapping = false;
        StartCoroutine(gameMenuManager.ButtonInputHold());
        MarkInventory();
    }

    public void PutInventory()
    {
        if (Input.GetKeyDown(inputSetup.putInventory) && isSettingQuantity == false && isSwapping==false)
        {
            inventoryQuantitySlider.value = 1;
            SetInitialQuantityToPut();
        }
        else if (Input.GetKeyDown(inputSetup.select) && isSettingQuantity == true)
        {
            inventoryBox.PlaceItem(temporaryItem, (int)inventoryQuantitySlider.value);
            slider.SetActive(false);
            isSettingQuantity = false;
        }
    }

    public void SetInitialQuantityToPut()
    {
        if (inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<InventoryIndicator>().item != null)
        {
            temporaryItem = inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<InventoryIndicator>().item;
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
        for (int i = 0; i < playerData.inventoryItem.Count; i++)
        {
            if (playerData.inventoryItem[i].id == newItem.id)
            {
                isItemExist = true;
                //quantity item ditambah quantity baru
                playerData.inventoryItem[i].quantity += quantity;
                playerData.CheckNewItem(playerData.inventoryItem[i]);
                newItem.quantity -= quantity;
                break;
            }
        }

        if (isItemExist == false)
        {
            Item newItemIn = new Item(newItem.id, newItem.itemImage, newItem.itemName, newItem.description, newItem.price,
                newItem.isUsable, newItem.isConsumable, newItem.isASingleTool, newItem.itemType);
            if (newItem.itemType != null)
                if (newItem.itemType.ToLower().Equals("seed".ToLower()))
                    newItemIn.plantID = newItem.plantID;

            newItemIn.quantity = quantity;
            playerData.inventoryItem.Add(newItemIn);
            playerData.CheckNewItem(playerData.inventoryItem[playerData.inventoryItem.Count-1]);
            newItem.quantity -= quantity;
        }

        inventoryBox.RefreshInventoryBox();
        RefreshInventory();
    }

    public void RefreshInventory()
    {        
        for (int i = 0; i < inventoryIndicator.Length; i++)
        {
            if (inventoryIndicator[i].GetComponent<InventoryIndicator>().item == null)
            {
                try
                {
                    for (int j = 0; j < playerData.inventoryItem.Count; j++)
                    {
                        if (playerData.inventoryItem[j].isOnInventory == false)
                        {
                            inventoryIndicator[i].GetComponent<InventoryIndicator>().item = playerData.inventoryItem[j];
                            inventoryIndicator[i].GetComponent<InventoryIndicator>().itemID = playerData.inventoryItem[j].id;
                            playerData.inventoryItem[j].isOnInventory = true;
                            break;
                        }
                    }
                }
                catch
                {
                    //Debug.Log("There is no item in " + i + " inventory slot.");
                }
            }
            inventoryIndicator[i].GetComponent<InventoryIndicator>().RefreshInventory();
        }
        usableItem.GetUsableItem();
    }
}
