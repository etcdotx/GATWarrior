using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameDataBase: MonoBehaviour {
    public static GameDataBase instance;

    [Header("SaveSlot")]
    public MainMenuScript mms;
    public int saveSlot;
    public bool[] saveSlotExist;

    [Header("Character Appearance")]
    public GameObject[] genderType;
    public Color32[] skinColor;
    public GameObject[] maleHairType;
    public GameObject[] femaleHairType;
    public Color32[] hairColor;

    [Header("Quest")]
    public CollectionQuest[] colQuestList;

    [Header("Item")]
    public Item[] Consumables;
    public Item[] Ingredients;
    public Item[] Materials;
    public Item[] Plants;
    public Item[] Tools;

    public void Awake()
    {
        //singleton
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        CheckSaveSlot();
        AddItem();
        AddCollectionQuest();  
    }

    void CheckSaveSlot() {
        try
        {
            mms = GameObject.Find("MainMenuScript").GetComponent<MainMenuScript>();
        }
        catch { }
        for (int i = 0; i < saveSlotExist.Length; i++)
        {
            string txt = i.ToString();
            string path = Application.persistentDataPath + "/player" + txt + ".savegame";
            if (File.Exists(path))
            {
                saveSlotExist[i] = true;
                try
                {
                    mms.saveSlotText[i].text = "Exist";
                }
                catch { }
            }
        }
    }

    //source 1 = npc1, location = rumah
    //source 2 = npc2, location = rumah

    #region CollectionQuest
    public void AddCollectionQuest()
    {
        if (QuestDataBase.collectionQuest == null)
            QuestDataBase.collectionQuest = new List<CollectionQuest>();

        addQuestSource1();
    }

    public void addQuestSource1()
    {
        for (int i = 0; i < colQuestList.Length; i++)
        {
            QuestDataBase.collectionQuest.Add(colQuestList[i]);
        }
    }
    #endregion

    #region Item
    void AddItem()
    {
        if (ItemDataBase.item == null)
            ItemDataBase.item = new List<Item>();

        for (int i = 0; i <Consumables.Length; i++)
        {
            ItemDataBase.item.Add(Consumables[i]);
        }
        for (int i = 0; i < Ingredients.Length; i++)
        {
            ItemDataBase.item.Add(Ingredients[i]);
        }
        for (int i = 0; i < Materials.Length; i++)
        {
            ItemDataBase.item.Add(Materials[i]);
        }
        for (int i = 0; i < Plants.Length; i++)
        {
            ItemDataBase.item.Add(Plants[i]);
        }
        for (int i = 0; i < Tools.Length; i++)
        {
            ItemDataBase.item.Add(Tools[i]);
        }
    }
    #endregion
}
