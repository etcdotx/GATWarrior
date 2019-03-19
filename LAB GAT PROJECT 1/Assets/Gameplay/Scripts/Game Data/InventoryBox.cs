using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour
{
    public PlayerData playerData;
    public MenuManager menuManager;

    [Header("UI Settings")]
    public GameObject inventoryBoxUI;
    public GameObject inventoryBoxView;
    public GameObject inventoryBoxViewPort;
    public GameObject inventoryBoxContent;
    public Scrollbar inventoryBoxScrollbar;

    public List<Item> item = new List<Item>();
    public GameObject[] inventoryBoxIndicator;
    public GameObject[,] inventoryBoxPos;
    public int inventoryBoxColumn;
    public int inventoryBoxRow;
    public int inventoryBoxContentChildCount;
    public int inventoryBoxColumnIndex;
    public int inventoryBoxRowIndex;
    public bool isItemBoxOpened;

    public void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
        inventoryBoxUI = GameObject.FindGameObjectWithTag("InventoryBoxUI");
        inventoryBoxView = inventoryBoxUI.transform.Find("InventoryBoxView").gameObject;
        inventoryBoxViewPort = inventoryBoxView.transform.Find("InventoryBoxViewPort").gameObject;
        inventoryBoxContent = inventoryBoxViewPort.transform.Find("InventoryBoxContent").gameObject;
        inventoryBoxScrollbar = inventoryBoxView.transform.Find("InventoryBoxScrollbar").GetComponent<Scrollbar>();

        inventoryBoxView.SetActive(false);
        inventoryBoxContentChildCount = inventoryBoxContent.transform.childCount;
        inventoryBoxColumn = inventoryBoxContent.GetComponent<GridLayoutGroup>().constraintCount;
        inventoryBoxRow = inventoryBoxContentChildCount / inventoryBoxColumn;
        inventoryBoxPos = new GameObject[inventoryBoxRow, inventoryBoxColumn];
        inventoryBoxColumnIndex = 0;
        inventoryBoxRowIndex = 0;

        int c = 0;
        for (int i = 0; i < inventoryBoxRow; i++)
        {
            for (int j = 0; j < inventoryBoxColumn; j++)
            {
                inventoryBoxPos[i, j] = inventoryBoxContent.transform.GetChild(c).gameObject;
                inventoryBoxIndicator[c] = inventoryBoxPos[i, j].gameObject;
                c++;
            }
        }
        InventoryBoxSelection();
    }

    public void InventoryBoxSelection()
    {
        if (menuManager.inputAxis.y == -1) // kebawah
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
        else if (menuManager.inputAxis.y == 1) // keatas
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

        if (menuManager.inputAxis.x == -1) // kekiri
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
        else if (menuManager.inputAxis.x == 1) //kekanan
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
        MarkItemBox();
        ScrollItemBox();
        RefreshItem();
    }

    void MarkItemBox()
    {
        for (int i = 0; i < inventoryBoxRow; i++)
        {
            for (int j = 0; j < inventoryBoxColumn; j++)
            {
                if (inventoryBoxPos[i, j].GetComponent<InventoryIndicator>().isSelected == false)
                {
                    inventoryBoxPos[i, j].GetComponent<Image>().color = menuManager.normalColor;
                }
            }
        }
        if (inventoryBoxPos[inventoryBoxRowIndex, inventoryBoxColumnIndex].GetComponent<InventoryIndicator>().isSelected == false)
        {
            inventoryBoxPos[inventoryBoxRowIndex, inventoryBoxColumnIndex].GetComponent<Image>().color = menuManager.markColor;
        }
    }


    public void ScrollItemBox()
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

    public void PlaceItem(Item newItem, int quantity)
    {
        bool isItemExist = false;
        //check if item exist or not
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].id == newItem.id)
            {
                isItemExist = true;
                //quantity item ditambah quantity baru
                item[i].quantity += quantity;
                newItem.quantity -= quantity;
                Debug.Log(item[i].quantity);
                Debug.Log(newItem.quantity);
                Debug.Log("exist");
                break;
            }
        }

        if (isItemExist == false)
        {
            Item newItemInBox = new Item(newItem.id, newItem.imagePath, newItem.name, newItem.description, newItem.isUsable);
            newItemInBox.quantity = quantity;
            item.Add(newItemInBox);
            newItem.quantity -= quantity;
            Debug.Log(item[0].quantity);
            Debug.Log(newItem.quantity);
        }
        playerData.RefreshItem();
        RefreshItem();
    }

    public void RefreshItem()
    {
        for (int i = 0; i < inventoryBoxIndicator.Length; i++)
        {
            if (inventoryBoxIndicator[i].GetComponent<InventoryIndicator>().itemID == 0)
            {
                try
                {
                    for (int j = 0; j < item.Count; j++)
                    {
                        if (item[j].isOnItemBox == false)
                        {
                            Debug.Log("masuk");
                            inventoryBoxIndicator[i].GetComponent<InventoryIndicator>().itemID = item[j].id;
                            item[j].isOnItemBox = true;
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
    }
}
