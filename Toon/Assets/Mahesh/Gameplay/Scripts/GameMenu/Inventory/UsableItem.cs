using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public PlayerData playerData;
    public InputSetup inputSetup;
    public GameObject player;
    public CharacterCombat characterCombat;
    public Inventory inventory;

    [Header("Indicator")]
    public GameObject usableItemUI;
    public GameObject usableItemView;
    public GameObject usableItemViewPort;
    public GameObject usableItemContent;

    [Header("Container")]
    public GameObject[] usableIndicator = new GameObject[3];

    [Header("Data")]
    public List<Item> usableItemList = new List<Item>();
    public Item selectedItem;
    public bool isItemUsable;
    public bool isSelectingItem;
    public bool isUsingItem;

    // Start is called before the first frame update
    void Start()
    {
        //playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        player = GameObject.FindGameObjectWithTag("Player");
        characterCombat = player.GetComponent<CharacterCombat>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();

        usableItemUI = GameObject.FindGameObjectWithTag("UsableItemUI");
        usableItemView = usableItemUI.transform.Find("UsableItemView").gameObject;
        usableItemViewPort = usableItemView.transform.Find("UsableItemViewPort").gameObject;
        usableItemContent = usableItemViewPort.transform.Find("UsableItemContent").gameObject;

        usableIndicator = new GameObject[usableItemContent.transform.childCount];
        for (int i = 0; i < usableIndicator.Length; i++)
        {
            usableIndicator[i] = usableItemContent.transform.GetChild(i).gameObject;
        }
        isUsingItem = false;
    }

    private void Update()
    {
        if (GameStatus.isTalking == false && GameStatus.IsPaused == false)
        {
            if (Input.GetKey(KeyCode.Joystick1Button4))
            {
                isSelectingItem = true;
                if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    SlideItem(true);
                }
                if (Input.GetKeyDown(KeyCode.Joystick1Button2))
                {
                    SlideItem(false);
                }
            }
            if (Input.GetKeyUp(KeyCode.Joystick1Button4))
            {
                isSelectingItem = false;
            }

            if (Input.GetKeyDown(inputSetup.useItem) && isItemUsable == true && characterCombat.combatMode==false && isSelectingItem==false)
            {
                Debug.Log("useitem");
                UseItem();
            }
        }
    }

    public void UseItem()
    {
        isUsingItem = true;
        selectedItem.Use();
        for (int i = 0; i < inventory.inventoryIndicator.Length; i++)
        {
            inventory.inventoryIndicator[i].GetComponent<InventoryIndicator>().RefreshInventory();
        }
        inventory.RefreshInventory();
    }

    public void SlideItem(bool isRight)
    {
        for (int i = 0; i < usableIndicator.Length; i++)
        {
            for (int j = 0; j < usableItemList.Count; j++)
            {
                if (usableIndicator[i].GetComponent<UsableItemIndicator>().item == usableItemList[j])
                {
                    if (isRight == true)
                    {
                        if (j == usableItemList.Count - 1)
                            usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[0];
                        else
                            usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[j + 1];

                        usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
                        break;
                    }
                    else {
                        if (j == 0)
                            usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[usableItemList.Count - 1];
                        else
                            usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[j - 1];

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
        if (usableItemList.Count > 0)
        {
            selectedItem = usableIndicator[1].GetComponent<UsableItemIndicator>().item;
            CheckIfItemIsUsable();
        }
    }
    
    //function ini dipanggil saat nge slide item dan juga jika ada trigger
    //misalnya saat memegang plant dekat dengan soil
    public void CheckIfItemIsUsable()
    {
        isItemUsable = selectedItem.IsItemUsable();
    }

    public void GetUsableItem()
    {
        if (!isUsingItem)
        {
            usableItemList.Clear();
            for (int i = 0; i < playerData.inventoryItem.Count; i++)
            {
                if (playerData.inventoryItem[i].isUsable == true)
                {
                    usableItemList.Add(playerData.inventoryItem[i]);
                }
            }
        }

        if (usableItemList.Count == 0)
        {
            for (int i = 0; i < usableIndicator.Length; i++)
            {
                usableIndicator[i].GetComponent<UsableItemIndicator>().item = null;
                usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            }
            return;
        }

        if (usableItemList.Count == 1)
        {
            for (int i = 0; i < usableIndicator.Length; i++)
            {
                usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[0];
                usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            }
        }
        else if (usableItemList.Count == 2)
        {
            usableIndicator[0].GetComponent<UsableItemIndicator>().item = usableItemList[0];
            usableIndicator[0].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            usableIndicator[1].GetComponent<UsableItemIndicator>().item = usableItemList[1];
            usableIndicator[1].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            usableIndicator[2].GetComponent<UsableItemIndicator>().item = usableItemList[0];
            usableIndicator[2].GetComponent<UsableItemIndicator>().RefreshUsableItem();
        }
        else if (!isUsingItem)
        {
            for (int i = 0; i < usableItemList.Count; i++)
            {
                usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[i];
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

        if (!isUsingItem)
        {
            selectedItem = usableIndicator[1].GetComponent<UsableItemIndicator>().item;
            CheckIfItemIsUsable();
            isUsingItem = false;
        }
    }
}
