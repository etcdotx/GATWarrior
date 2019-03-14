using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [Header("Script List")]
    public GameDataBase gdb;
    public StartGame sg;
    public SaveSlot ss;
    public InputSetup inputSetup;
    public CameraMovement cm;

    [Header("Player Data")]
    public bool cantOpenMenu;
    public int[] characterAppearance; //Gender,Skincolor,hair,haircolor
    public GameObject character;
    public GameObject[] body;// 0 Gender, 1 Hair

    [Header("Menu Manager")]
    public MenuManager menuManager;

    [Header("Inventory")]
    public GameObject inventoryView;
    public Image[] inventoryIndicator;
    public int[] inventoryItemID;
    public List<Item> item = new List<Item>();

    [Header("Quest")]
    public GameObject questView;
    public GameObject questContent;
    public GameObject questListPrefab;
    public int questListIndex;
    public List<CollectionQuest> collectionQuest = new List<CollectionQuest>();
    public List<int> finishedCollectionQuestID = new List<int>();

    // Use this for initialization
    void Start()
    {
        //Get Script
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        gdb = GameObject.FindGameObjectWithTag("GDB").GetComponent<GameDataBase>();
        ss = GameObject.Find("SaveSlot").GetComponent<SaveSlot>();
        menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();

        //Get Gameobject or others
        inventoryView = GameObject.FindGameObjectWithTag("Inventory").transform.Find("InventoryView").gameObject;
        questView = GameObject.FindGameObjectWithTag("Quest").transform.Find("QuestView").gameObject;
        questContent = questView.transform.Find("QuestViewPort").transform.Find("QuestContent").gameObject;
        questListIndex = -1;

        //Set Default
        inventoryView.SetActive(false);
        questView.SetActive(false);
        cantOpenMenu = true;
        RefreshItem();
    }

    private void Update()
    {
        CharacterInput();
    }

    public void CharacterInput()
    {
        if (GameStatus.isTalking == false && cantOpenMenu == false)
        {
            if (Input.GetKeyDown(inputSetup.openInventory))
            {
                if (inventoryView.activeSelf == false)
                {
                    inventoryView.SetActive(true);
                    questView.SetActive(true);
                    menuManager.ResetMenu();
                }
                else {
                    inventoryView.SetActive(false);
                    questView.SetActive(false);
                }
            }
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this, ss.saveSlot.ToString());
    }

    public void LoadPlayer(string spawnLocationName)
    {
        //start on new game/scene
        GameObject spawnLocation = GameObject.Find(spawnLocationName);
        cantOpenMenu = false;

        PlayerData data = SaveSystem.LoadPlayer(ss.saveSlot.ToString());

        //Collect Appearance Data
        characterAppearance = new int[data.characterAppearance.Length];
        for (int i = 0; i < characterAppearance.Length; i++)
        {
            characterAppearance[i] = data.characterAppearance[i];
        }

        //Apply Appearance
        try
        {
            body[0] = GameObject.Instantiate(gdb.genderType[characterAppearance[0]], spawnLocation.transform.position, spawnLocation.transform.rotation, null);
            body[0].GetComponent<Renderer>().material.color = gdb.skinColor[characterAppearance[1]];
            if (characterAppearance[0] == 0)
            {
                body[1] = GameObject.Instantiate(gdb.maleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
            }
            else
            {
                body[1] = GameObject.Instantiate(gdb.femaleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
            }
            body[1].GetComponent<Renderer>().material.color = gdb.hairColor[characterAppearance[3]];
        }
        catch {
        }
        
        do {
            try
            {
                //Get
                cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
                character = GameObject.FindGameObjectWithTag("Player");
                sg = GameObject.Find("StartGame").GetComponent<StartGame>();

                //Modify
                cm.lookAt = character.transform;
                character.GetComponent<Rigidbody>().isKinematic = false;
                character.transform.localScale = sg.characterScale;
            }
            catch { }
        } while (cm == null);
    }

    public void AddQuest(CollectionQuest newQuest)
    {
        collectionQuest.Add(newQuest);
        AddQuestToList(newQuest);
        CheckNewQuestProgress(newQuest);
    }

    public void AddQuestToList(CollectionQuest newQuest)
    {
        Instantiate(questListPrefab, questContent.transform);
        questListIndex++;
        questContent.transform.GetChild(questListIndex).GetComponent<QuestListUI>().questText.text = collectionQuest[questListIndex].title;
        questContent.transform.GetChild(questListIndex).GetComponent<QuestListUI>().questID = collectionQuest[questListIndex].id;
        menuManager.questMaxIndex++;
    }

    public void CheckNewQuestProgress(CollectionQuest newQuest)
    {
        for (int i = 0; i < item.Count; i++)
        {
            Debug.Log(newQuest.itemToCollect.name + " " + item[i].name);
            if (newQuest.itemToCollect.name == item[i].name)
            {
                newQuest.curAmount = item[i].quantity;
                newQuest.CheckProgress();
            }
        }
    }

    public void AddItem(Item newItem)
    {
        bool itemExist=false;

        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].id == newItem.id)
            {
                item[i].quantity++;
                itemExist = true;
                Debug.Log(newItem.name + " added");
                CheckNewItem(item[i]);
                RefreshItem();
                break;
            }
        }

        if (itemExist == false)
        {
            item.Add(newItem);
            for (int i = 0; i < inventoryItemID.Length; i++)
            {
                if (inventoryItemID[i] == 0)
                {
                    inventoryItemID[i] = newItem.id;
                    break;
                }
            }
            Debug.Log("New " + newItem.name + " added");
            CheckNewItem(newItem);
            RefreshItem();
        }
    }


    public void CheckNewItem(Item addedItem)
    {
        for (int i = 0; i < collectionQuest.Count; i++)
        {
            if (collectionQuest[i].itemToCollect.name == addedItem.name)
            {
                collectionQuest[i].curAmount = addedItem.quantity;
                collectionQuest[i].CheckProgress();
            }
        }
    }

    public void RefreshItem()
    {
        for (int i = 0; i < inventoryIndicator.Length; i++)
        {
            try
            {
                inventoryIndicator[i].transform.GetChild(0).gameObject.SetActive(false);
                if (item[i].quantity != 0)
                {
                    inventoryIndicator[i].sprite = item[i].itemImage;
                    inventoryIndicator[i].transform.GetChild(0).GetComponent<Text>().text = item[i].quantity.ToString();
                    inventoryIndicator[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            } catch { }
        }
        menuManager.RefreshQuest();
    }

}
