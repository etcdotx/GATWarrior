using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour
{
    public PlayerData playerData;
    public InputSetup inputSetup;
    public Inventory inventory;
    public GameMenuManager gameMenuManager;
    public UsableItem usableItem;

    [Header("UI Settings")]
    public GameObject inventoryBoxUI;
    public GameObject inventoryBoxView;
    public GameObject inventoryBoxViewPort;
    public GameObject inventoryBoxContent;
    public Scrollbar inventoryBoxScrollbar;

    public GameObject[] inventoryBoxIndicator;
    public GameObject[,] inventoryBoxPos;
    public int inventoryBoxColumn;
    public int inventoryBoxRow;
    public int inventoryBoxContentChildCount;
    public int inventoryBoxColumnIndex;
    public int inventoryBoxRowIndex;
    public bool isItemBoxOpened;

    //swap
    public InventoryIndicator invenSwap1;
    public InventoryIndicator invenSwap2;
    public bool isSwapping;
    public Item temporaryItem;

    //put ke inven
    public bool isSettingQuantity;
    public GameObject slider;
    public Slider inventoryBoxQuantitySlider;

    public void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        gameMenuManager = GameObject.FindGameObjectWithTag("GameMenuManager").GetComponent<GameMenuManager>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        usableItem = GameObject.FindGameObjectWithTag("UsableItem").GetComponent<UsableItem>();

        inventoryBoxUI = GameObject.FindGameObjectWithTag("InventoryBoxUI");
        inventoryBoxView = inventoryBoxUI.transform.Find("InventoryBoxView").gameObject;
        inventoryBoxViewPort = inventoryBoxView.transform.Find("InventoryBoxViewPort").gameObject;
        inventoryBoxContent = inventoryBoxViewPort.transform.Find("InventoryBoxContent").gameObject;
        inventoryBoxScrollbar = inventoryBoxView.transform.Find("InventoryBoxScrollbar").GetComponent<Scrollbar>();

        slider = inventoryBoxView.transform.Find("Slider").gameObject;
        slider.SetActive(false);
        inventoryBoxQuantitySlider = slider.transform.Find("InventoryBoxQuantitySlider").GetComponent<Slider>();

        inventoryBoxView.SetActive(false);
        inventoryBoxContentChildCount = inventoryBoxContent.transform.childCount;
        inventoryBoxColumn = inventoryBoxContent.GetComponent<GridLayoutGroup>().constraintCount;
        inventoryBoxRow = inventoryBoxContentChildCount / inventoryBoxColumn;
        inventoryBoxPos = new GameObject[inventoryBoxRow, inventoryBoxColumn];
        inventoryBoxColumnIndex = 0;
        inventoryBoxRowIndex = 0;

        int c = 0;
        inventoryBoxIndicator = new GameObject[inventoryBoxContent.transform.childCount];
        for (int i = 0; i < inventoryBoxRow; i++)
        {
            for (int j = 0; j < inventoryBoxColumn; j++)
            {
                inventoryBoxPos[i, j] = inventoryBoxContent.transform.GetChild(c).gameObject;
                inventoryBoxIndicator[c] = inventoryBoxPos[i, j].gameObject;
                c++;
            }
        }
        RefreshInventoryBox();
    }

    public void InventoryBoxSwapping()
    {
        if (Input.GetKeyDown(inputSetup.select) && isSwapping == false)
        {
            invenSwap1 = inventoryBoxPos[inventoryBoxRowIndex, inventoryBoxColumnIndex].GetComponent<InventoryIndicator>();
            invenSwap1.gameObject.GetComponent<Image>().color = gameMenuManager.selectedColor;
            invenSwap1.isSelected = true;
            isSwapping = true;
            StartCoroutine(gameMenuManager.ButtonInputHold());
        }
        else if (Input.GetKeyDown(inputSetup.select) && isSwapping == true)
        {
            invenSwap2 = inventoryBoxPos[inventoryBoxRowIndex, inventoryBoxColumnIndex].GetComponent<InventoryIndicator>();
            int id1 = invenSwap1.itemID;
            int id2 = invenSwap2.itemID;
            invenSwap1.itemID = id2;
            invenSwap2.itemID = id1;
            RefreshInventoryBox();
            ResetInventoryBoxSwap();
        }
    }

    public void ResetInventoryBoxSwap()
    {
        invenSwap1.isSelected = false;
        invenSwap1.gameObject.GetComponent<Image>().color = gameMenuManager.normalColor;
        isSwapping = false;
        StartCoroutine(gameMenuManager.ButtonInputHold());
    }

    public void InventoryBoxSelection()
    {
        if (gameMenuManager.inputAxis.y == -1) // kebawah
        {
            if (inventoryBoxRowIndex < inventoryBoxRow - 1)
            {
                inventoryBoxRowIndex++;
            }
            else
            {
                inventoryBoxRowIndex = 0;
            }
        }
        else if (gameMenuManager.inputAxis.y == 1) // keatas
        {
            if (inventoryBoxRowIndex > 0)
            {
                inventoryBoxRowIndex--;
            }
            else
            {
                inventoryBoxRowIndex = inventoryBoxRow - 1;
            }
        }

        if (gameMenuManager.inputAxis.x == -1) // kekiri
        {
            if (inventoryBoxColumnIndex > 0)
            {
                inventoryBoxColumnIndex--;
            }
            else
            {
                inventoryBoxColumnIndex = inventoryBoxColumn - 1;
            }
        }
        else if (gameMenuManager.inputAxis.x == 1) //kekanan
        {
            if (inventoryBoxColumnIndex < inventoryBoxColumn - 1)
            {
                inventoryBoxColumnIndex++;
            }
            else
            {
                inventoryBoxColumnIndex = 0;
            }
        }
        MarkInventoryBox();
        ScrollInventoryBox();
    }

    public void MarkInventoryBox()
    {
        try
        {
            for (int i = 0; i < inventoryBoxRow; i++)
            {
                for (int j = 0; j < inventoryBoxColumn; j++)
                {
                    if (inventoryBoxPos[i, j].GetComponent<InventoryIndicator>().isSelected == false)
                    {
                        inventoryBoxPos[i, j].GetComponent<Image>().color = gameMenuManager.normalColor;
                    }
                }
            }
            if (inventoryBoxPos[inventoryBoxRowIndex, inventoryBoxColumnIndex].GetComponent<InventoryIndicator>().isSelected == false)
            {
                inventoryBoxPos[inventoryBoxRowIndex, inventoryBoxColumnIndex].GetComponent<Image>().color = gameMenuManager.markColor;
            }
        } catch { }
    }

    public void ScrollInventoryBox()
    {
        if (inventoryBoxRowIndex == inventoryBoxRow - 1)
        {
            inventoryBoxScrollbar.value = 0;
        }
        else
        {
            float a = (float)inventoryBoxRow;
            float b = (float)inventoryBoxRowIndex;
            float c = a - b;
            float d = c / a;
            inventoryBoxScrollbar.value = c / a;
        }
    }

    public void PutInventory()
    {
        if (Input.GetKeyDown(inputSetup.putInventory) && isSettingQuantity == false && isSwapping == false)
        {
            inventoryBoxQuantitySlider.value = 1;
            SetInitialQuantityToPut();
        }
        if (Input.GetKeyDown(inputSetup.select) && isSettingQuantity == true)
        {
            inventory.PlaceItem(temporaryItem, (int)inventoryBoxQuantitySlider.value);
            slider.SetActive(false);
            isSettingQuantity = false;
        }
    }

    public void SetInitialQuantityToPut()
    {
        bool isItemExist = true;
        if (inventoryBoxPos[inventoryBoxRowIndex, inventoryBoxColumnIndex].GetComponent<InventoryIndicator>().itemID == 0)
        {
            Debug.Log("no item");
            isItemExist = false;
        }
        if (isItemExist == true)
        {
            for (int i = 0; i < playerData.inventoryBoxItem.Count; i++)
            {
                if (inventoryBoxPos[inventoryBoxRowIndex, inventoryBoxColumnIndex].GetComponent<InventoryIndicator>().itemID == playerData.inventoryBoxItem[i].id)
                {
                    temporaryItem = playerData.inventoryBoxItem[i]; //jenis item
                    slider.SetActive(true);
                    inventoryBoxQuantitySlider.minValue = 1;
                    inventoryBoxQuantitySlider.maxValue = playerData.inventoryBoxItem[i].quantity;
                    inventoryBoxQuantitySlider.transform.Find("Text").GetComponent<Text>().text = inventoryBoxQuantitySlider.value.ToString();
                    isItemExist = true;
                    isSettingQuantity = true;
                    break;
                }
            }
        }
    }

    public void IncreaseQuantityToPut()
    {
        inventoryBoxQuantitySlider.value += 1;
        inventoryBoxQuantitySlider.transform.Find("Text").GetComponent<Text>().text = inventoryBoxQuantitySlider.value.ToString();
    }

    public void DecreaseQuantityToPut()
    {
        inventoryBoxQuantitySlider.value -= 1;
        inventoryBoxQuantitySlider.transform.Find("Text").GetComponent<Text>().text = inventoryBoxQuantitySlider.value.ToString();
    }

    public void PlaceItem(Item newItem, int quantity)
    {
        bool isItemExist = false;
        //check if item exist or not
        for (int i = 0; i < playerData.inventoryBoxItem.Count; i++)
        {
            if (playerData.inventoryBoxItem[i].id == newItem.id)
            {
                isItemExist = true;
                //quantity item ditambah quantity baru
                playerData.inventoryBoxItem[i].quantity += quantity;
                newItem.quantity -= quantity;
                Debug.Log(playerData.inventoryBoxItem[i].quantity);
                Debug.Log(newItem.quantity);
                Debug.Log("exist");
                break;
            }
        }
        if (isItemExist == false)
        {
            Item newItemInBox = new Item(newItem.id, newItem.itemImage, newItem.itemName,
                newItem.description, newItem.isUsable, newItem.isConsumable, newItem.isASingleTool, newItem.itemType);
            if (newItem.itemType != null)
                if (newItem.itemType.ToLower().Equals("plant".ToLower()))
                    newItemInBox.plantID = newItem.plantID;

            newItemInBox.quantity = quantity;
            playerData.inventoryBoxItem.Add(newItemInBox);
            newItem.quantity -= quantity;
        }

        //check inventory ke quest
        for (int i = 0; i < playerData.inventoryItem.Count; i++)
        {
            if (playerData.inventoryItem[i].id == newItem.id)
            {
                playerData.CheckNewItem(playerData.inventoryItem[i]);
                Debug.Log(playerData.inventoryItem[i].quantity);
                Debug.Log("ya");
            }
        }
        inventory.RefreshInventory();
        RefreshInventoryBox();
    }

    public void RefreshInventoryBox()
    {
        for (int i = 0; i < inventoryBoxIndicator.Length; i++)
        {
            if (inventoryBoxIndicator[i].GetComponent<InventoryIndicator>().itemID == 0)
            {
                try
                {
                    for (int j = 0; j < playerData.inventoryBoxItem.Count; j++)
                    {
                        if (playerData.inventoryBoxItem[j].isOnItemBox == false)
                        {
                            inventoryBoxIndicator[i].GetComponent<InventoryIndicator>().itemID = playerData.inventoryBoxItem[j].id;
                            playerData.inventoryBoxItem[j].isOnItemBox = true;
                            break;
                        }
                    }
                }
                catch
                {
                    //Debug.Log("There is no item in " + i + " inventory box.");
                }
            }
            inventoryBoxIndicator[i].GetComponent<InventoryIndicator>().RefreshItemBox();
        }
        usableItem.GetUsableItem();
        usableItem.SlideItem(true);
    }
}
