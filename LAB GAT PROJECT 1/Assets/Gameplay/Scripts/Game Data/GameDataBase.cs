using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataBase: MonoBehaviour {

    [Header("Character Appearance")]
    public GameObject[] genderType;
    public Color32[] skinColor;
    public GameObject[] maleHairType;
    public GameObject[] femaleHairType;
    public Color32[] hairColor;

    public void Start()
    {
        AddItem();
        AddCollectionQuest();
        QuestDialog();
        QuestCompleteDialog();        
    }

    //source 1 = npc1, location = rumah
    //source 2 = npc2, location = rumah

    #region CollectionQuest
    public void AddCollectionQuest()
    {
        if (QuestDataBase.collectionQuest == null)
            QuestDataBase.collectionQuest = new List<CollectionQuest>();
        addQuestSource1();
        addQuestSource2();
    }

    public void addQuestSource1()
    {
        CollectionQuest quest1 = new CollectionQuest(1, 1, 2, 5, "GameObject/Quest/Cube", "Collect 5 Cube", "Collect", "Collect 5 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest1);

        CollectionQuest quest3 = new CollectionQuest(1, 3, 4, 7, "GameObject/Quest/Cube", "Collect 7 Cube", "Collect", "Collect 7 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest3);

        CollectionQuest quest4 = new CollectionQuest(1, 4, 5, 8, "GameObject/Quest/Cube", "Gather 8 Cube", "Gather", "Gather mystery cube around the town.", false);
        QuestDataBase.collectionQuest.Add(quest4);

        CollectionQuest quest5 = new CollectionQuest(1, 5, 6, 5, "GameObject/Quest/Cube", "Collect 5 Cube", "Collect", "Collect 5 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest5);

        CollectionQuest quest6 = new CollectionQuest(1, 6, 7, 6, "GameObject/Quest/Cube", "Gather 6 Cube", "Gather", "Gather 6 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest6);

        CollectionQuest quest7 = new CollectionQuest(1, 7, 8, 7, "GameObject/Quest/Cube", "Collect 7 Cube", "Collect", "Collect 7 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest7);

        CollectionQuest quest8 = new CollectionQuest(1, 8, 9, 8, "GameObject/Quest/Cube", "Gather 8 Cube", "Gather", "Gather 8 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest8);

        CollectionQuest quest9 = new CollectionQuest(1, 9, 10, 7, "GameObject/Quest/Cube", "Collect 7 Cube", "Collect", "Collect 7 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest9);

        CollectionQuest quest10 = new CollectionQuest(1, 10, 0, 8, "GameObject/Quest/Cube", "Gather 8 Cube", "Gather", "Gather 8 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest10);
    }

    void addQuestSource2()
    {
        CollectionQuest quest2 = new CollectionQuest(1, 2, 3, 6, "GameObject/Quest/Cube", "Gather 6 Cube", "Gather", "Mystery cube fom the town.", false);
        QuestDataBase.collectionQuest.Add(quest2);
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

        QuestDialog qd5 = new QuestDialog(1, 5, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd5);

        QuestDialog qd6 = new QuestDialog(1, 6, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd6);

        QuestDialog qd7 = new QuestDialog(1, 7, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd7);

        QuestDialog qd8 = new QuestDialog(1, 8, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd8);

        QuestDialog qd9 = new QuestDialog(1, 9, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd9);

        QuestDialog qd10 = new QuestDialog(1, 10, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd10);
    }

    void addDialogSource2()
    {
        QuestDialog qd2_1 = new QuestDialog(1, 2, "Can i ask u again");
        QuestDataBase.questDialog.Add(qd2_1);

        QuestDialog qd2_2 = new QuestDialog(1, 2, "Can u help me to gather some ingredients?");
        QuestDataBase.questDialog.Add(qd2_2);

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
    }
    void addDialogCompleteSource2()
    {
        QuestDialog qcd2_1 = new QuestDialog(1, 2, "Thanks to you!");
        QuestDataBase.questCompleteDialog.Add(qcd2_1);
    }
    #endregion

    #region Item
    void AddItem()
    {
        if (ItemDataBase.item == null)
            ItemDataBase.item = new List<Item>();

        Item Cube = new Item(1, "ItemImage/Cube", "Cube", "A cube", false);
        ItemDataBase.item.Add(Cube);

        Item Sphere = new Item(2, "ItemImage/Sphere", "Sphere", "A sphere", false);
        ItemDataBase.item.Add(Sphere);
    }
    #endregion
}
