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
    }

    public void AddCollectionQuest()
    {
        if (QuestDataBase.collectionQuest == null)
            QuestDataBase.collectionQuest = new List<CollectionQuest>();

        CollectionQuest quest1 = new CollectionQuest(1, 1, 2, 5, "Quest/Cube", "Collect 5 Cube", "Collect", "Collect 5 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest1);

        CollectionQuest quest2 = new CollectionQuest(1, 2, 3, 6, "Quest/Cube", "Collect 6 Cube", "Collect", "Collect 6 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest2);

        CollectionQuest quest3 = new CollectionQuest(1, 3, 4, 7, "Quest/Cube", "Collect 7 Cube", "Collect", "Collect 7 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest3);

        CollectionQuest quest4 = new CollectionQuest(1, 4, 5, 8, "Quest/Cube", "Collect 8 Cube", "Collect", "Collect 8 cube in town", false);
        QuestDataBase.collectionQuest.Add(quest4);

        //Debug.Log(QuestDataBase.collectionQuest.Count);
    }

    public void AddItem()
    {
        if (ItemDataBase.item == null)
            ItemDataBase.item = new List<Item>();

        Item Cube = new Item(1, "Item/Cube", "Cube", "A cube", false);
        ItemDataBase.item.Add(Cube);
    }
}
