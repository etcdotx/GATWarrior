using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour {
    public static PlayerData instance;

    StartGame startGame;

    [Header("Player Wealth")]
    public int gold; //gold player

    [Header("Inventory")]
    //item yang ada di inventory player
    public List<Item> inventoryItem = new List<Item>();
    //item yang ada di inventory box player
    public List<Item> inventoryBoxItem = new List<Item>(); 

    [Header("Quest")]
    //quest yang player sudah ambil
    public List<CollectionQuest> collectionQuest = new List<CollectionQuest>();
    //quest yang sudah player selesaikan
    public List<CollectionQuest> collectionQuestComplete = new List<CollectionQuest>(); 

    [Header("Player State")]
    //ketika player darahnya habis
    public bool stateHpNotMax; 
    
    //DEBUGMODE
    bool dontloadCharacter;

    //item yang ingin dikasih ke inventory pada awal game
    public List<Item> itemToAdd = new List<Item>();
    //item yang ingin dikasih ke inventory box pada awal game
    public List<Item> itemBoxToAdd = new List<Item>(); 

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        if (GameDataBase.instance.DEBUGMODE)
        {
            dontloadCharacter = true;
        }

        if (GameDataBase.instance.newGame)
        {
            gold = 1250;

            //additem awal2
            for (int i = 0; i < itemToAdd.Count; i++)
            {
                AddItem(itemToAdd[i]);
            }
            for (int i = 0; i < itemBoxToAdd.Count; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    AddItemToBox(itemBoxToAdd[i]);
                }
            }

            Debug.Log("new game");
            SavePlayer();
        }
    }

    /// <summary>
    /// script untuk melakukan save game
    /// </summary>
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(GameDataBase.instance.saveSlot.ToString());
    }

    /// <summary>
    /// Script untuk load player
    /// </summary>
    /// <param name="spawnLocationName"></param>
    public void LoadPlayer(string spawnLocationName)
    {
        //start on new game/scene
        GameObject spawnLocation = GameObject.Find(spawnLocationName);

        try
        {
            PlayerSaveData data = SaveSystem.LoadPlayer(GameDataBase.instance.saveSlot.ToString());

            for (int i = 0; i < data.collectionQuestID.Count; i++)
            {
                collectionQuest.Add(GameDataBase.instance.colQuestDictionary[data.collectionQuestID[i]]);
            }
            for (int i = 0; i < data.collectionQuestCompleteID.Count; i++)
            {
                collectionQuestComplete.Add(GameDataBase.instance.colQuestDictionary[data.collectionQuestCompleteID[i]]);
            }
            for (int i = 0; i < data.inventoryItemID.Count; i++)
            {
                AddItem(GameDataBase.instance.itemDictionary[data.inventoryItemID[i]]);
                //inventoryItem.Add(GameDataBase.instance.itemDictionary[data.inventoryItemID[i]]);
            }
            for (int i = 0; i < data.inventoryBoxItemID.Count; i++)
            {
                AddItemToBox(GameDataBase.instance.itemDictionary[data.inventoryBoxItemID[i]]);
                //inventoryBoxItem.Add(GameDataBase.instance.itemDictionary[data.inventoryBoxItemID[i]]);
            }

            Debug.Log("load complete");
        }
        catch
        {
            Debug.Log("error catching data, some item may not exist");
        }

        //Collect Appearance Data

        if (dontloadCharacter == false)
        {
            //load data
        }
        else {
            //Instantiate(charPrefab, spawnLocation.transform.position, spawnLocation.transform.rotation, null);
            Debug.Log("Start without instantiate");
        }

        startGame = GameObject.Find("StartGame").GetComponent<StartGame>();
    }

    public void AddQuest(CollectionQuest cq)
    {
        CollectionQuest newQuest = ScriptableObject.CreateInstance<CollectionQuest>();
        newQuest.Duplicate(cq);

        //add quest kedalam quest yang player miliki       
        collectionQuest.Add(newQuest);
        //memasukkan quest list kedalam ui  
        AddNewQuestToUI(newQuest);
        //mengecek status quest baru
        CheckNewQuestProgress(newQuest);
    }

    public void AddNewQuestToUI(CollectionQuest cq)
    {
        //instantiate prefab quest ui indicator 
        QuestIndicator qi = Instantiate(Quest.instance.questListPrefab, Quest.instance.questContent.transform).GetComponent<QuestIndicator>();
        qi.questText.text = cq.title;
        qi.questID = cq.id;
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

        //check jika questnya sudah selesai atau belum
        newQuest.CheckProgress();
    }

    /// <summary>
    /// function untuk add item ke player inventory
    /// </summary>
    /// <param name="item">item yang ditambah</param>
    public bool AddItem(Item item)
    {
        bool itemExist=false;
        for (int i = 0; i < inventoryItem.Count; i++)
        {
            //jika item yang ditambah sudah ada diinventory
            if (inventoryItem[i].id == item.id) 
            {
                //jika item nya sudah max
                if (inventoryItem[i].quantity == inventoryItem[i].maxQuantityOnInventory) 
                {
                    Debug.Log("Max quantity of " + item.itemName + " = " + inventoryItem[i].quantity + ".");
                    return false;
                }
                //item dijadiin exist supaya ga run function yang dibawah
                itemExist = true;
                //kuantiti ditambah
                inventoryItem[i].quantity++;
                //cek item tersebut ke quest yang exist
                CheckNewItem(inventoryItem[i]); 
                break;
            }
        }

        if (itemExist == false)
        {
            //check apakah inventory masih cukup untuk tambah item baru
            bool thereIsSpace = false; 
            for (int i = 0; i < Inventory.instance.inventoryIndicator.Length; i++)
            {
                // jika ada slot yang kosong
                if (Inventory.instance.inventoryIndicator[i].item == null)
                {
                    thereIsSpace = true;
                    break;
                }
            }
            //jika tidak ada slot
            if (!thereIsSpace) 
            {
                UIManager.instance.warningNotification(null, WarningState.inventoryFull);
                Debug.Log("item space is full");
                return false;
            }

            //item baru ditambah kedalam list player
            Item newItem = ScriptableObject.CreateInstance<Item>();
            newItem.Duplicate(item);

            inventoryItem.Add(newItem);
            //cek item tersebut ke quest yang exist
            CheckNewItem(newItem);
        }
        //item tersebut di refresh kedalam inventory
        //Debug.Log(item.name + " , total = " + inventoryItem.Count);
        Inventory.instance.RefreshInventory();
        return true;
    }

    /// <summary>
    /// function untuk add item ke item box
    /// </summary>
    /// <param name="item">item yang ditambah</param>
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
            Item newItem = ScriptableObject.CreateInstance<Item>();
            newItem.Duplicate(item);

            inventoryBoxItem.Add(newItem);
        }
        //item tersebut di refresh kedalam inventory
        //Debug.Log(item.name + " , total = " + inventoryBoxItem.Count);
        InventoryBox.instance.RefreshInventoryBox();
    }

    /// <summary>
    /// check item baru apakah dia berhubungan dengan quest yang ada
    /// </summary>
    /// <param name="addedItem"></param>
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

    /// <summary>
    /// function untuk memasukkan quest yang sudah selesai
    /// </summary>
    /// <param name="cqc"></param>
    public void AddCollectionQuestComplete(CollectionQuest cqc)
    {
        RemoveQuestList(cqc);
        collectionQuest.Remove(cqc);
        bool colQuestExist = false;

        //karena diperkirakan akan ada repeatable quest
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

    /// <summary>
    /// remove quest yang sudah selesai dari ui
    /// </summary>
    /// <param name="cq"></param>
    public void RemoveQuestList(CollectionQuest cq)
    {
        for (int i = 0; i < Quest.instance.questContent.transform.childCount; i++)
        {
            if (Quest.instance.questContent.transform.GetChild(i).GetComponent<QuestIndicator>().questID == cq.id)
            {
                Destroy(Quest.instance.questContent.transform.GetChild(i).gameObject);
                Quest.instance.RefreshQuestDetail(cq.id);
                break;
            }
        }
    }
}
