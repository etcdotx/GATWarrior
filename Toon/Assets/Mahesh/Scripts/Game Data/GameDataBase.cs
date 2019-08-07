using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameDataBase: MonoBehaviour {
    public static GameDataBase instance;

    [Header("Debug Mode")]
    public bool DEBUGMODE;

    [Header("SaveSlot")]
    public MainMenuScript mms;
    public int saveSlot;
    public bool[] saveSlotExist;
    public bool newGame;

    [Header("Quest")]
    //Masukkan quest yang dipakai di game
    public CollectionQuest[] colQuestList;
    //dictionary ada supaya bisa dipanggil dengan performance tanpa for
    public Dictionary<int, CollectionQuest> colQuestDictionary = new Dictionary<int, CollectionQuest>();

    [Header("Item")]
    //Masukkan item list yang ada di game
    public Item[] consumable;
    public Item[] materials;
    public Item[] plants;
    public Item[] tools;
    List<Item> itemList = new List<Item>();

    //dictionary ada supaya bisa dipanggil dengan performance tanpa for
    public Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

    public void Awake()
    {
        //singleton
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        CheckSaveSlot();
        AddDictionary();
    }

    private void Start()
    {
        //DEBUGMODE
        if (DEBUGMODE == true)
        {
            saveSlot = 0;
        }

        newGame = false;
    }

    /// <summary>
    /// untuk ngecek jika ada save data atau tidak
    /// </summary>
    void CheckSaveSlot() {
        try
        {
            mms = GameObject.Find("MainMenuScript").GetComponent<MainMenuScript>();
            for (int i = 0; i < saveSlotExist.Length; i++)
            {
                string txt = i.ToString();
                string path = Application.persistentDataPath + "/player" + txt + ".savegame";
                if (File.Exists(path))
                {
                    saveSlotExist[i] = true;
                    mms.saveSlotText[i].text = "Exist";
                }
            }
        }
        catch {
            //Debug.Log("no mainmenuscript");
        }       
    }

    void AddDictionary() {
        AddItem();

        for (int i = 0; i < colQuestList.Length; i++)
        {
            colQuestDictionary.Add(colQuestList[i].id, colQuestList[i]);
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            itemDictionary.Add(itemList[i].id, itemList[i]);
        }
    }

    void AddItem() {
        for (int i = 0; i < consumable.Length; i++)
        {
            itemList.Add(consumable[i]);
        }
        for (int i = 0; i < materials.Length; i++)
        {
            itemList.Add(materials[i]);
        }
        for (int i = 0; i < tools.Length; i++)
        {
            itemList.Add(tools[i]);
        }
        for (int i = 0; i < plants.Length; i++)
        {
            itemList.Add(plants[i]);
        }
    }
}
