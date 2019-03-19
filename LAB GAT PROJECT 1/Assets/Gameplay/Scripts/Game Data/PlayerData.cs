using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour {

    [Header("Script List")]
    public GameDataBase gameDataBase;
    public StartGame startGame;
    public CameraMovement cm;
    public InventoryBox inventoryBox;
    public InputSetup inputSetup;

    [Header("Player Data")]
    public int[] characterAppearance; //Gender,Skincolor,hair,haircolor
    public GameObject character;
    public GameObject[] body;// 0 Gender, 1 Hair

    [Header("Player Status")]
    public string playerName;
    public float maxHealth;
    public float curHealth;
    public Image maxHealthImg;
    public Image curHealthImg;

    [Header("Menu Manager")]
    public MenuManager menuManager;

    [Header("Inventory")]
    public GameObject inventoryView;
    public GameObject[] inventoryIndicator;
    public int[] inventoryItemID;
    public List<Item> item = new List<Item>();

    [Header("Quest")]
    public GameObject questView;
    public GameObject questContent;
    public GameObject questListPrefab;
    public int questListIndex;
    public List<int> playerChainQuest = new List<int>();
    public List<CollectionQuest> collectionQuest = new List<CollectionQuest>();
    public List<CollectionQuest> collectionQuestComplete = new List<CollectionQuest>();
    public List<CollectionQuest> collectionQuestUnusable = new List<CollectionQuest>();
    public List<int> finishedCollectionQuestID = new List<int>();

    [Header("FOR DEVELOPMENT")]
    public bool DEVELOPERMODE;
    public GameObject charPrefab;
    bool dontloadCharacter;

    void Start()
    {
        //Get Script
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        gameDataBase = GameObject.FindGameObjectWithTag("GameDataBase").GetComponent<GameDataBase>();
        menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();

        //DEVELOPERMODE
        if (DEVELOPERMODE == true)
        {
            dontloadCharacter = true;
            gameDataBase.saveSlot = 0;
        }

        Debug.Log(gameDataBase.saveSlot.ToString());

        //Get Gameobject or others
        inventoryView = GameObject.FindGameObjectWithTag("InventoryUI").transform.Find("InventoryView").gameObject;
        int h = inventoryView.transform.Find("InventoryViewPort").childCount;
        for (int i = 0; i < h; i++)
        {
            inventoryIndicator[i] = inventoryView.transform.Find("InventoryViewPort").transform.GetChild(i).gameObject;
        }
        questView = GameObject.FindGameObjectWithTag("QuestUI").transform.Find("QuestView").gameObject;
        questContent = questView.transform.Find("QuestViewPort").transform.Find("QuestContent").gameObject;
        questListIndex = -1;
        playerChainQuest.Add(1);

        //Set Default
        inventoryView.SetActive(false);
        questView.SetActive(false);
        RefreshItem();

        //player status
        maxHealth = 100;
        curHealth = 100;
    }

    private void Update()
    {
        //debug
        curHealthImg.fillAmount = curHealth / maxHealth;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this, gameDataBase.saveSlot.ToString());
    }

    public void LoadPlayer(string spawnLocationName)
    {
        //start on new game/scene
        GameObject spawnLocation = GameObject.Find(spawnLocationName);

        try
        {
            PlayerSaveData data = SaveSystem.LoadPlayer(gameDataBase.saveSlot.ToString());
            characterAppearance = new int[data.characterAppearance.Length];
            for (int i = 0; i < characterAppearance.Length; i++)
            {
                characterAppearance[i] = data.characterAppearance[i];
            }
        } catch(UnityException ex)
        {
            Debug.Log(ex);
        }

        //Collect Appearance Data

        if (dontloadCharacter == false)
        {
            //Apply Appearance
            try
            {
                body[0] = GameObject.Instantiate(gameDataBase.genderType[characterAppearance[0]], spawnLocation.transform.position, spawnLocation.transform.rotation, null);
                body[0].GetComponent<Renderer>().material.color = gameDataBase.skinColor[characterAppearance[1]];
                if (characterAppearance[0] == 0)
                {
                    body[1] = GameObject.Instantiate(gameDataBase.maleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
                }
                else
                {
                    body[1] = GameObject.Instantiate(gameDataBase.femaleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
                }
                body[1].GetComponent<Renderer>().material.color = gameDataBase.hairColor[characterAppearance[3]];
            }
            catch
            {
            }
        }
        else {
            Instantiate(charPrefab, spawnLocation.transform.position, spawnLocation.transform.rotation, null);
        }

        do {
            try
            {
                //Get
                cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
                character = GameObject.FindGameObjectWithTag("Player");
                startGame = GameObject.Find("StartGame").GetComponent<StartGame>();

                //Modify
                cm.lookAt = character.transform;
                character.GetComponent<Rigidbody>().isKinematic = false;
                character.transform.localScale = startGame.characterScale;
            }
            catch {
                Debug.Log("There is no camera or a character or a start game script");
            }
        } while (cm == null);
    }

    public void AddQuest(CollectionQuest newQuest)
    {
        //memasukkan quest baru kedalam ke quest yang dimiliki player
        collectionQuest.Add(newQuest);
        //memasukkan quest list kedalam ui
        AddQuestToList(newQuest);
        //mengecek status quest baru
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
            if (newQuest.itemToCollect.name == item[i].name)
            {
                //jika item dari quest baru sudah dimiliki, maka amountnya disamakan dengan quantity
                newQuest.curAmount = item[i].quantity;
                //jika item dari quest tersebut sudah memenuhi target maka quest tersebut dimasukkan kedalam quest complete coll
                newQuest.CheckProgress();
                Debug.Log("here");
                if (newQuest.isComplete == true)
                {
                    AddCollectionQuestComplete(newQuest);
                }
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
                //jika item tersebut sudah ada, kuantiti ditambah
                item[i].quantity++;
                itemExist = true;
                Debug.Log(newItem.name + " added");
                //cek item tersebut ke quest yang exist
                CheckNewItem(item[i]);
                //item tersebut di refresh kedalam inventory
                RefreshItem();
                break;
            }
        }

        if (itemExist == false)
        {
            //item baru ditambah kedalam list player
            item.Add(newItem);
            for (int i = 0; i < inventoryItemID.Length; i++)
            {
                if (inventoryItemID[i] == 0)
                {
                    //item id dimasukkan kedalam array yang masih 0 idnya
                    inventoryItemID[i] = newItem.id;
                    break;
                }
            }
            Debug.Log("New " + newItem.name + " added");
            //cek item tersebut ke quest yang exist
            CheckNewItem(newItem);
            //item tersebut di refresh kedalam inventory
            RefreshItem();
        }
    }


    public void CheckNewItem(Item addedItem)
    {
        for (int i = 0; i < collectionQuest.Count; i++)
        {
            if (collectionQuest[i].itemToCollect.name == addedItem.name)
            {
                //jumlah item yang baru, dimasukkan kedalam amount quest yang membutuhkan item tersebut
                collectionQuest[i].curAmount = addedItem.quantity;
                //jika item tersebut sudah memenuhi kriteria, maka quest tersebut complete
                collectionQuest[i].CheckProgress();

                if (collectionQuest[i].isComplete == true)
                {
                    //jika quest sudah selesai, masukkin ke quest id
                    AddCollectionQuestComplete(collectionQuest[i]);
                }
            }
        }
    }

    public void AddCollectionQuestComplete(CollectionQuest cq)
    {
        bool colQuestExist = false;
        for (int k = 0; k < collectionQuestComplete.Count; k++)
        {
            //jika quest yang sudah selesai, sudah ada di dalam koleksi quest yang sudah selesai, tidak dimasukkan
            if (cq.id == collectionQuestComplete[k].id)
            {
                colQuestExist = true;
                //Debug.Log("Collection Quest Complete Exist");
            }
        }
        if (colQuestExist == false)
        {
            //jika quest yang sudah selesai, tidak ada di dalam koleksi quest yang sudah selesai, masukkan quest tersebut
            collectionQuestComplete.Add(cq);
            Debug.Log("Collection Quest Complete : " + collectionQuestComplete.Count);
        }
    }

    public void RefreshItem()
    {
        for (int i = 0; i < inventoryIndicator.Length; i++)
        {
            if (inventoryIndicator[i].GetComponent<InventoryIndicator>().itemID == 0)
            {
                try
                {
                    for (int j = 0; j < item.Count; j++)
                    {
                        if (item[j].isOnInventory == false)
                        {
                            Debug.Log(item[j].name + " " + item[j].isOnInventory);
                            Debug.Log("masuk");
                            inventoryIndicator[i].GetComponent<InventoryIndicator>().itemID = item[j].id;
                            item[j].isOnInventory = true;
                            Debug.Log(item[j].name + " " + item[j].isOnInventory);
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
        menuManager.RefreshQuest();
    }

}
