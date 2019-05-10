using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameDataBase: MonoBehaviour {
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

    [Header("Test")]
    public CollectionQuest[] colQuestList;
    public Item[] itemList;
    public Item[] usableItem;
    public Item[] plantItem;

    public void Awake()
    {
        CheckSaveSlot();
        AddItem();
        AddCollectionQuest();
        QuestDialog();
        QuestCompleteDialog();        
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

    #region QuestDialog
    void QuestDialog()
    {
        if (QuestDataBase.questDialog == null)
            QuestDataBase.questDialog = new List<QuestDialog>();
        addDialogSource1();
        addDialogSource2();
    }

    void addDialogSource1()
    {
        QuestDialog qd1_1 = new QuestDialog(1, 1, "Can i ask u something");
        QuestDataBase.questDialog.Add(qd1_1);

        QuestDialog qd1_2 = new QuestDialog(1, 1, "Can u help me to collect some ingredients?");
        QuestDataBase.questDialog.Add(qd1_2);

        QuestDialog qd3_1 = new QuestDialog(1, 3, "Can i ask u something");
        QuestDataBase.questDialog.Add(qd3_1);

        QuestDialog qd3_2 = new QuestDialog(1, 3, "Can u help me to collect some ingredients?");
        QuestDataBase.questDialog.Add(qd3_2);

        QuestDialog qd4_1 = new QuestDialog(1, 4, "Can i ask u again");
        QuestDataBase.questDialog.Add(qd4_1);

        QuestDialog qd4_2 = new QuestDialog(1, 4, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd4_2);
    }

    void addDialogSource2()
    {
        QuestDialog qd2_1 = new QuestDialog(2, 2, "Can i ask u again");
        QuestDataBase.questDialog.Add(qd2_1);

        QuestDialog qd2_2 = new QuestDialog(2, 2, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd2_2);

        QuestDialog qd5_1 = new QuestDialog(2, 5, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd5_1);

        QuestDialog qd6_1 = new QuestDialog(2, 6, "Boy");
        QuestDataBase.questDialog.Add(qd6_1);

        QuestDialog qd6_2 = new QuestDialog(2, 6, "What the what");
        QuestDataBase.questDialog.Add(qd6_2);

        QuestDialog qd7_1 = new QuestDialog(2, 7, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd7_1);
    }
    #endregion

    #region QuestCompleteDialog

    void QuestCompleteDialog()
    {
        if (QuestDataBase.questCompleteDialog == null)
            QuestDataBase.questCompleteDialog = new List<QuestDialog>();
        addDialogCompleteSource1();
        addDialogCompleteSource2();
    }

    void addDialogCompleteSource1()
    {
        QuestDialog qcd1_1 = new QuestDialog(1, 1, "Thanks!");
        QuestDataBase.questCompleteDialog.Add(qcd1_1);
        QuestDialog qcd3_1 = new QuestDialog(1, 3, "Thanks a lot!");
        QuestDataBase.questCompleteDialog.Add(qcd3_1);
        QuestDialog qcd4_1 = new QuestDialog(1, 4, "Wow!");
        QuestDataBase.questCompleteDialog.Add(qcd4_1);
        QuestDialog qcd4_2 = new QuestDialog(1, 4, "Thanks again!");
        QuestDataBase.questCompleteDialog.Add(qcd4_2);
    }
    void addDialogCompleteSource2()
    {
        QuestDialog qcd2_1 = new QuestDialog(2, 2, "Thanks to you!");
        QuestDataBase.questCompleteDialog.Add(qcd2_1);
        QuestDialog qcd5_1 = new QuestDialog(2, 5, "Thanks to me!");
        QuestDataBase.questCompleteDialog.Add(qcd5_1);
        QuestDialog qcd6_1 = new QuestDialog(2, 6, "Thanks to your mom!");
        QuestDataBase.questCompleteDialog.Add(qcd6_1);
        QuestDialog qcd7_1 = new QuestDialog(2, 7, "Thanks!");
        QuestDataBase.questCompleteDialog.Add(qcd7_1);
    }
    #endregion

    #region Item
    void AddItem()
    {
        if (ItemDataBase.item == null)
            ItemDataBase.item = new List<Item>();

        for (int i = 0; i < itemList.Length; i++)
        {
            ItemDataBase.item.Add(itemList[i]);
        }
        for (int i = 0; i < usableItem.Length; i++)
        {
            ItemDataBase.item.Add(usableItem[i]);
        }
        for (int i = 0; i < plantItem.Length; i++)
        {
            ItemDataBase.item.Add(plantItem[i]);
        }
    }
    #endregion
}
