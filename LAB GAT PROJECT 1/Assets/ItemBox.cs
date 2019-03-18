using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    public Player player;
    public MenuManager menuManager;
    public GameObject itemBoxUI;
    public GameObject itemBoxView;
    public GameObject itemBoxViewPort;
    public GameObject itemBoxContent;
    public Scrollbar itemBoxScrollbar;

    public List<Item> item = new List<Item>();
    public GameObject[] itemBoxIndicator;
    public GameObject[,] itemBoxPos;
    public int itemBoxColumn;
    public int itemBoxRow;
    public int itemBoxContentChildCount;
    public int itemBoxColumnIndex;
    public int itemBoxRowIndex;
    public bool isItemBoxOpened;

    [Header("Menu Pointer")]
    public int menuNumber;//0 inventory, 1 quest    
    public Color32 normalColor;
    public Color32 markColor;
    public Color32 selectedColor;
    public GameObject[] menuPointer;

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
        itemBoxUI = GameObject.FindGameObjectWithTag("ItemBoxUI");
        itemBoxView = itemBoxUI.transform.Find("ItemBoxView").gameObject;
        itemBoxViewPort = itemBoxView.transform.Find("ItemBoxViewPort").gameObject;
        itemBoxScrollbar = itemBoxView.transform.Find("ItemBoxScrollbar").GetComponent<Scrollbar>();
        itemBoxContent = itemBoxViewPort.transform.Find("ItemBoxContent").gameObject;

        itemBoxView.SetActive(false);
        itemBoxContentChildCount = itemBoxContent.transform.childCount;
        itemBoxColumn = itemBoxContent.GetComponent<GridLayoutGroup>().constraintCount;
        itemBoxRow = itemBoxContentChildCount / itemBoxColumn;
        itemBoxPos = new GameObject[itemBoxRow, itemBoxColumn];
        itemBoxColumnIndex = 0;
        itemBoxRowIndex = 0;
        int c = 0;
        for (int i = 0; i < itemBoxRow; i++)
        {
            for (int j = 0; j < itemBoxColumn; j++)
            {
                itemBoxPos[i, j] = itemBoxContent.transform.GetChild(c).gameObject;
                itemBoxIndicator[c] = itemBoxPos[i, j].gameObject;
                c++;
            }
        }
        ItemBoxSelection();
    }

    public void ItemBoxSelection()
    {
        if (menuManager.inputAxis.y == -1) // kebawah
        {
            if (itemBoxRowIndex < itemBoxRow - 1)
            {
                itemBoxRowIndex++;
            }
            else
            {
                itemBoxRowIndex = 0;
            }
        }
        else if (menuManager.inputAxis.y == 1) // keatas
        {
            if (itemBoxRowIndex > 0)
            {
                itemBoxRowIndex--;
            }
            else
            {
                itemBoxRowIndex = itemBoxRow - 1;
            }
        }

        if (menuManager.inputAxis.x == -1) // kekiri
        {
            if (itemBoxColumnIndex > 0)
            {
                itemBoxColumnIndex--;
            }
            else
            {
                itemBoxColumnIndex = itemBoxColumn - 1;
            }
        }
        else if (menuManager.inputAxis.x == 1) //kekanan
        {
            if (itemBoxColumnIndex < itemBoxColumn - 1)
            {
                itemBoxColumnIndex++;
            }
            else
            {
                itemBoxColumnIndex = 0;
            }
        }
        MarkItemBox();
        ScrollItemBox();
    }

    void MarkItemBox()
    {
        for (int i = 0; i < itemBoxRow; i++)
        {
            for (int j = 0; j < itemBoxColumn; j++)
            {
                if (itemBoxPos[i, j].GetComponent<InventoryItem>().isSelected == false)
                {
                    itemBoxPos[i, j].GetComponent<Image>().color = menuManager.normalColor;
                }
            }
        }
        if (itemBoxPos[itemBoxRowIndex, itemBoxColumnIndex].GetComponent<InventoryItem>().isSelected == false)
        {
            itemBoxPos[itemBoxRowIndex, itemBoxColumnIndex].GetComponent<Image>().color = menuManager.markColor;
        }
    }


    public void ScrollItemBox()
    {
        if (itemBoxRowIndex == itemBoxRow - 1)
        {
            itemBoxScrollbar.value = 0;
        }
        else
        {
            float a = (float)itemBoxRow;
            float b = (float)itemBoxRowIndex;
            float c = a - b;
            float d = c / a;
            itemBoxScrollbar.value = c / a;
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
            Item a = new Item(newItem.id, newItem.imagePath, newItem.name, newItem.description, newItem.isUsable);
            a.quantity = quantity;
            item.Add(a);
            newItem.quantity -= quantity;
            Debug.Log(item[0].quantity);
            Debug.Log(newItem.quantity);
        }
        player.RefreshItem();
        RefreshItem();
    }

    public void RefreshItem()
    {
        for (int i = 0; i < itemBoxIndicator.Length; i++)
        {
            if (itemBoxIndicator[i].GetComponent<InventoryItem>().itemID == 0)
            {
                try
                {
                    for (int j = 0; j < item.Count; j++)
                    {
                        if (item[j].isOnItemBox == false)
                        {
                            Debug.Log(item[j].name + " " + item[j].isOnItemBox);
                            Debug.Log("masuk");
                            itemBoxIndicator[i].GetComponent<InventoryItem>().itemID = item[j].id;
                            item[j].isOnItemBox = true;
                            Debug.Log(item[j].name + " " + item[j].isOnItemBox);
                            break;
                        }
                    }
                }
                catch
                {
                    //Debug.Log("There is no item in " + i + " inventory box.");
                }
            }
            itemBoxIndicator[i].GetComponent<InventoryItem>().RefreshItemBox();
        }
    }
}
