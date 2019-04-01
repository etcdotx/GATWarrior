using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public PlayerData playerData;

    [Header("Indicator")]
    public GameObject usableItemUI;
    public GameObject usableItemView;
    public GameObject usableItemViewPort;
    public GameObject usableItemContent;

    [Header("Container")]
    public GameObject[] usableIndicator = new GameObject[3];
    public GameObject selectedLocation;

    [Header("Data")]
    public List<Item> usableItemList = new List<Item>();
    public List<int> usableItemID = new List<int>();
    public Item selectedItem;
    public bool isItemUsable;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();

        usableItemUI = GameObject.FindGameObjectWithTag("UsableItemUI");
        usableItemView = usableItemUI.transform.Find("UsableItemView").gameObject;
        usableItemViewPort = usableItemView.transform.Find("UsableItemViewPort").gameObject;
        usableItemContent = usableItemViewPort.transform.Find("UsableItemContent").gameObject;

        usableIndicator = new GameObject[usableItemContent.transform.childCount];
        for (int i = 0; i < usableIndicator.Length; i++)
        {
            usableIndicator[i] = usableItemContent.transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        if (GameStatus.isTalking == false && GameStatus.IsPaused == false)
        {
            if (Input.GetKey(KeyCode.Joystick1Button4))
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    SlideItem(true);
                }
                if (Input.GetKeyDown(KeyCode.Joystick1Button2))
                {
                    SlideItem(false);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button2) && isItemUsable == true)
            {
                Debug.Log(selectedItem.name + " is used");
                selectedItem.Use();
            }
        }
    }

    public void SlideItem(bool isRight)
    {
        for (int i = 0; i < usableIndicator.Length; i++)
        {
            for (int j = 0; j < usableItemID.Count; j++)
            {
                if (usableIndicator[i].GetComponent<UsableItemIndicator>().itemID == usableItemID[j])
                {
                    if (isRight == true)
                    {
                        if (j == usableItemID.Count - 1)
                            usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = usableItemID[0];
                        else
                            usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = usableItemID[j + 1];

                        usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
                        break;
                    }
                    else {
                        if (j == 0)
                            usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = usableItemID[usableItemID.Count - 1];
                        else
                            usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = usableItemID[j - 1];

                        usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
                        break;
                    }
                }
            }
        }
        SelectItem();
    }

    void SelectItem()
    {
        for (int i = 0; i < playerData.inventoryItem.Count; i++)
        {
            if (playerData.inventoryItem[i].id == usableIndicator[1].GetComponent<UsableItemIndicator>().itemID)
            {
                selectedItem = playerData.inventoryItem[i];
                break;
            }
        }

        try
        {
            CheckIfItemIsUsable();
        } catch { }
    }

    public void CheckIfItemIsUsable()
    {
        isItemUsable = selectedItem.IsItemUsable();
    }

    public void GetUsableItem()
    {
        usableItemList.Clear();
        usableItemID.Clear();
        for (int i = 0; i < playerData.inventoryItem.Count; i++)
        {
            bool exist = false;
            if (playerData.inventoryItem[i].isUsable == true)
            {
                for (int j = 0; j < usableItemList.Count; j++)
                {
                    if (playerData.inventoryItem[i].id == usableItemList[j].id)
                    {
                        exist = true;
                        break;
                    }
                }
                if (exist == false)
                {
                    usableItemList.Add(playerData.inventoryItem[i]);
                    usableItemID.Add(playerData.inventoryItem[i].id);
                }
            }
        }

        if (usableItemList.Count == 0)
        {
            for (int i = 0; i < usableIndicator.Length; i++)
            {
                usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = 0;
                usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            }
        }

        if (usableItemList.Count == 1)
        {
            for (int i = 0; i < usableIndicator.Length; i++)
            {
                usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = usableItemList[0].id;
                usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            }
        }
        else if (usableItemList.Count == 2)
        {
            usableIndicator[0].GetComponent<UsableItemIndicator>().itemID = usableItemList[0].id;
            usableIndicator[0].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            usableIndicator[1].GetComponent<UsableItemIndicator>().itemID = usableItemList[1].id;
            usableIndicator[1].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            usableIndicator[2].GetComponent<UsableItemIndicator>().itemID = usableItemList[0].id;
            usableIndicator[2].GetComponent<UsableItemIndicator>().RefreshUsableItem();
        }
        else
        {
            for (int i = 0; i < usableItemList.Count; i++)
            {
                usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = usableItemList[i].id;
                usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
                if (i == usableIndicator.Length - 1)
                {
                    break;
                }
            }
        }

        if (usableItemList.Count > 0)
        {
            isItemUsable = true;
        }
        else
        {
            isItemUsable = false;
        }
    }
}
