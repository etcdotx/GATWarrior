using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour {

    [Header("Script List")]
    public GameDataBase gameDataBase;
    public StartGame startGame;
    public CameraMovement cm;
    public Inventory inventory;
    public Quest quest;
    public InventoryBox inventoryBox;
    public InputSetup inputSetup;

    [Header("Player Data")]
    public int[] characterAppearance; //Gender,Skincolor,hair,haircolor
    public GameObject character;
    public GameObject[] body;// 0 Gender, 1 Hair

    [Header("Player Wealth")]
    public int gold;

    [Header("Player Status")]
    public string playerName;
    public float maxHealth;
    public float curHealth;
    public GameObject healthIndicator;
    public Image maxHealthImg;
    public Image curHealthImg;

    [Header("Menu Manager")]
    public GameMenuManager gameMenuManager;

    [Header("Inventory")]
    public List<Item> inventoryItem = new List<Item>();
    public List<Item> inventoryBoxItem = new List<Item>();

    [Header("Quest")]
    public GameObject questListPrefab;
    public int questListIndex;
    public List<CollectionQuest> collectionQuest = new List<CollectionQuest>();
    public List<CollectionQuest> collectionQuestComplete = new List<CollectionQuest>();
    public List<int> finishedCollectionQuestID = new List<int>();

    [Header("Player State")]
    public GameObject charPrefab;
    public bool stateNearSoil;
    public bool stateHpNotMax;

    [Header("FOR DEVELOPMENT")]
    public bool DEVELOPERMODE;
    public bool dontloadCharacter;
    public List<Item> itemToAdd = new List<Item>();
    public List<Item> itemBoxToAdd = new List<Item>();

    private void Awake()
    {
        //Get Script
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        gameDataBase = GameObject.FindGameObjectWithTag("GameDataBase").GetComponent<GameDataBase>();
        gameMenuManager = GameObject.FindGameObjectWithTag("GameMenuManager").GetComponent<GameMenuManager>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        quest = GameObject.FindGameObjectWithTag("Quest").GetComponent<Quest>();
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();

        //DEVELOPERMODE
        if (DEVELOPERMODE == true)
        {
            gameDataBase.saveSlot = 0;
        }
    }

    void Start()
    {
        questListIndex = -1;

        //Set Default
        inventory.inventoryView.SetActive(false);
        quest.questView.SetActive(false);

        //characterstats
        curHealth = maxHealth;
        RefreshHp();
        gold = 1250;

        //additem
        for (int i = 0; i < itemToAdd.Count; i++)
        {
            AddItem(itemToAdd[i]);
        }
        for (int i = 0; i < itemBoxToAdd.Count; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                AddItemToBox(itemBoxToAdd[i]);
            }
        }
    }

    public void RefreshHp() {
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
            //PlayerSaveData data = SaveSystem.LoadPlayer(gameDataBase.saveSlot.ToString());
            //characterAppearance = new int[data.characterAppearance.Length];
            //for (int i = 0; i < characterAppearance.Length; i++)
            //{
            //    characterAppearance[i] = data.characterAppearance[i];
            //}
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
                Debug.Log("error");
            }
        }
        else {
            //Instantiate(charPrefab, spawnLocation.transform.position, spawnLocation.transform.rotation, null);
            Debug.Log("Start without instantiate");
        }

        do {
            try
            {
                //Get
                cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
                character = GameObject.FindGameObjectWithTag("Player");
                startGame = GameObject.Find("StartGame").GetComponent<StartGame>();

                //Modify
                cm.player = character;
                character.GetComponent<Rigidbody>().isKinematic = false;
                //character.transform.localScale = startGame.characterScale;
            }
            catch {
                Debug.Log("There is no camera or a character or a start game script");
            }
        } while (cm == null);
    }

    public void AddQuest(CollectionQuest cq)
    {
        //memasukkan quest baru kedalam ke quest yang dimiliki player
        CollectionQuest newQuest = ScriptableObject.CreateInstance<CollectionQuest>();
        newQuest.Duplicate(cq);

        collectionQuest.Add(newQuest);
        //memasukkan quest list kedalam ui
        AddNewQuestToList(newQuest);
        //mengecek status quest baru
        CheckNewQuestProgress(newQuest);
    }

    public void AddNewQuestToList(CollectionQuest cq)
    {
        Instantiate(questListPrefab, quest.questContent.transform);
        int a = quest.questContent.transform.childCount - 1; //last quest (new addded)
        quest.questContent.transform.GetChild(a).GetComponent<QuestIndicator>().questText.text = cq.title;
        quest.questContent.transform.GetChild(a).GetComponent<QuestIndicator>().questID = cq.id;
        quest.questMaxIndex++;
    }

    public void RemoveQuestList(CollectionQuest cq)
    {
        for (int i = 0; i < quest.questContent.transform.childCount; i++)
        {
            if (quest.questContent.transform.GetChild(i).GetComponent<QuestIndicator>().questID == cq.id)
            {
                Destroy(quest.questContent.transform.GetChild(i).gameObject);
                quest.questMaxIndex--;
                quest.RefreshQuest();
                break;
            }
        }
    }

    public void CheckNewQuestProgress(CollectionQuest newQuest)
    {
        newQuest.curAmount = 0;
        for (int i = 0; i < inventoryItem.Count; i++)
        {
            if (newQuest.itemToCollect.id == inventoryItem[i].id)
            {
                newQuest.curAmount += inventoryItem[i].quantity;
                break;
            }
        }
        //for (int i = 0; i < inventoryBoxItem.Count; i++)
        //{
        //    if (newQuest.itemToCollect.name == inventoryBoxItem[i].name)
        //    {
        //        newQuest.curAmount += inventoryBoxItem[i].quantity;
        //        break;
        //    }
        //}
        newQuest.CheckProgress();
    }

    public void AddItem(Item item)
    {
        bool itemExist=false;
        for (int i = 0; i < inventoryItem.Count; i++)
        {
            if (inventoryItem[i].id == item.id)
            {
                if (inventoryItem[i].quantity == inventoryItem[i].maxQuantityOnInventory)
                {
                    Debug.Log("Max quantity of " + item.itemName + " = " + inventoryItem[i].quantity + ".");
                    return;
                }
                //jika item tersebut sudah ada dan kuantiti != maxkuantiti di inven, kuantiti ditambah
                inventoryItem[i].quantity++;
                itemExist = true;
                //cek item tersebut ke quest yang exist
                CheckNewItem(inventoryItem[i]);
                break;
            }
        }

        if (itemExist == false)
        {
            bool thereIsSpace = false;
            for (int i = 0; i < inventory.inventoryIndicator.Length; i++)
            {
                if (inventory.inventoryIndicator[i].item == null)
                {
                    thereIsSpace = true;
                    break;
                }
            }
            if (!thereIsSpace)
            {
                Debug.Log("item full");
                return;
            }

            //item baru ditambah kedalam list player
            Item newItem = new Item(item.id, item.itemImage, item.itemName, item.description, item.maxQuantityOnInventory, item.price,
                item.isUsable, item.isConsumable, item.isASingleTool, item.itemType);
            if (item.itemType != null)
                if (item.itemType.ToLower().Equals("seed".ToLower()))
                    newItem.plantID = item.plantID;

            inventoryItem.Add(newItem);
            //cek item tersebut ke quest yang exist
            CheckNewItem(newItem);
        }
        //item tersebut di refresh kedalam inventory
        //Debug.Log(item.name + " , total = " + inventoryItem.Count);
        inventory.RefreshInventory();
    }

    public void AddItemToBox(Item item)
    {
        bool itemExist = false;

        for (int i = 0; i < inventoryBoxItem.Count; i++)
        {
            if (inventoryBoxItem[i].id == item.id)
            {
                //jika item tersebut sudah ada, kuantiti ditambah
                inventoryBoxItem[i].quantity++;
                itemExist = true;
                break;
            }
        }

        if (itemExist == false)
        {
            //item baru ditambah kedalam list player
            Item newItem = new Item(item.id, item.itemImage, item.itemName, item.description, item.maxQuantityOnInventory, item.price,
                item.isUsable, item.isConsumable, item.isASingleTool, item.itemType);
            if (item.itemType != null)
                if (item.itemType.ToLower().Equals("seed".ToLower()))
                    newItem.plantID = item.plantID;

            inventoryBoxItem.Add(newItem);
        }
        //item tersebut di refresh kedalam inventory
        //Debug.Log(item.name + " , total = " + inventoryBoxItem.Count);
        inventoryBox.RefreshInventoryBox();
    }

    public void CheckNewItem(Item addedItem)
    {
        for (int i = 0; i < collectionQuest.Count; i++)
        {
            if (collectionQuest[i].itemToCollect.id == addedItem.id)
            {
                collectionQuest[i].curAmount = 0;

                //jumlah item yang baru, dimasukkan kedalam amount quest yang membutuhkan item tersebut
                collectionQuest[i].curAmount += addedItem.quantity;
                //jika item tersebut sudah memenuhi kriteria, maka quest tersebut complete
                collectionQuest[i].CheckProgress();
                Debug.Log("in");
            }
        }
    }

    public void AddCollectionQuestComplete(CollectionQuest cqc)
    {
        RemoveQuestList(cqc);
        collectionQuest.Remove(cqc);
        bool colQuestExist = false;
        for (int k = 0; k < collectionQuestComplete.Count; k++)
        {
            //jika quest yang sudah selesai, sudah ada di dalam koleksi quest yang sudah selesai, tidak dimasukkan
            if (cqc.id == collectionQuestComplete[k].id)
            {
                colQuestExist = true;
                //Debug.Log("Collection Quest Complete Exist");
            }
        }
        if (colQuestExist == false)
        {
            //jika quest yang sudah selesai, tidak ada di dalam koleksi quest yang sudah selesai, masukkan quest tersebut
            CollectionQuest newQuestCom = ScriptableObject.CreateInstance<CollectionQuest>();
            newQuestCom.Duplicate(cqc);

            collectionQuestComplete.Add(newQuestCom);
            //AddCompleteQuestList(newQuestCom);
            Debug.Log("Collection Quest Complete : " + collectionQuestComplete.Count);
        }
    }
}
