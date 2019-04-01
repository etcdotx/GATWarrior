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

    [Header("Player Status")]
    public string playerName;
    public float maxHealth;
    public float curHealth;
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
    public bool stateNearSoil;
    public bool stateHpNotMax;

    [Header("FOR DEVELOPMENT")]
    public bool DEVELOPERMODE;
    public GameObject charPrefab;
    bool dontloadCharacter;

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
            dontloadCharacter = true;
            gameDataBase.saveSlot = 0;
        }
    }

    void Start()
    {
        questListIndex = -1;

        //Set Default
        inventory.inventoryView.SetActive(false);
        quest.questView.SetActive(false);

        //player status
        maxHealth = 100;
        curHealth = 100;
    }

    private void Update()
    {
        curHealthImg.fillAmount = curHealth / maxHealth;
        //Debug.Log("Cq " + collectionQuest.Count);
        //Debug.Log("cqc " + collectionQuestComplete.Count);
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

    public void AddQuest(CollectionQuest cq)
    {
        //memasukkan quest baru kedalam ke quest yang dimiliki player
        CollectionQuest newQuest = new CollectionQuest(cq.sourceID, cq.id, cq.chainQuestID, cq.colAmount, cq.resourcePath, cq.title, cq.verb, cq.description, cq.isOptional);
        newQuest.chainQuestID = cq.chainQuestID;
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
            if (newQuest.itemToCollect.name == inventoryItem[i].name)
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
                //jika item tersebut sudah ada, kuantiti ditambah
                inventoryItem[i].quantity++;
                itemExist = true;
                //cek item tersebut ke quest yang exist
                CheckNewItem(inventoryItem[i]);
                break;
            }
        }

        if (itemExist == false)
        {
            //item baru ditambah kedalam list player
            Item newItem = new Item(item.id, item.imagePath, item.name, item.description, 
                item.isUsable, item.isASingleTool, item.itemType);
            if (item.itemType != null)
                if (item.itemType.ToLower().Equals("plant".ToLower()))
                    newItem.plantID = item.plantID;

            inventoryItem.Add(newItem);
            //cek item tersebut ke quest yang exist
            CheckNewItem(newItem);
        }
        //item tersebut di refresh kedalam inventory
        inventory.RefreshInventory();
    }


    public void CheckNewItem(Item addedItem)
    {
        for (int i = 0; i < collectionQuest.Count; i++)
        {
            if (collectionQuest[i].itemToCollect.name == addedItem.name)
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
            CollectionQuest newQuestCom = new CollectionQuest(cqc.sourceID, cqc.id, cqc.chainQuestID, cqc.colAmount, cqc.resourcePath, cqc.title, cqc.verb, cqc.description, cqc.isOptional);
            newQuestCom.chainQuestID = cqc.chainQuestID;
            collectionQuestComplete.Add(newQuestCom);
            //AddCompleteQuestList(newQuestCom);
            Debug.Log("Collection Quest Complete : " + collectionQuestComplete.Count);
        }
    }
}
