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

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();

        usableItemUI = GameObject.FindGameObjectWithTag("UsableItemUI");
        usableItemView = usableItemUI.transform.Find("UsableItemView").gameObject;
        usableItemViewPort = usableItemView.transform.Find("UsableItemViewPort").gameObject;
        usableItemContent = usableItemViewPort.transform.Find("UsableItemContent").gameObject;

        for (int i = 0; i < usableIndicator.Length; i++)
        {
            usableIndicator[i] = usableItemContent.transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Joystick1Button4))
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                for (int i = 0; i < usableIndicator.Length; i++)
                {
                    for (int j = 0; j < usableItemID.Count; j++)
                    {
                        if (usableIndicator[i].GetComponent<UsableItemIndicator>().itemID == usableItemID[j])
                        {
                            if (j == usableItemID.Count - 1)
                                usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = usableItemID[0];
                            else
                                usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = usableItemID[j + 1];
                            usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
                            break;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                for (int i = 0; i < usableIndicator.Length; i++)
                {
                    for (int j = 0; j < usableItemID.Count; j++)
                    {
                        if (usableIndicator[i].GetComponent<UsableItemIndicator>().itemID == usableItemID[j])
                        {
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
        }
    }

    public void GetUsableItem()
    {
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
        
        for (int i = 0; i < usableItemList.Count; i++)
        {
            usableIndicator[i].GetComponent<UsableItemIndicator>().itemID = usableItemList[i].id;
            usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            if (i == usableIndicator.Length-1)
            {
                break;
            }
        }
    }
}
