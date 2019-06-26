using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collection Quest", menuName = "Database/Collection Quest")]
public class CollectionQuest : ScriptableObject
{
    public int sourceID;
    public int id;
    public List<int> chainQuestID = new List<int>();
    public int colAmount;
    public int curAmount;
    public Item itemToCollect;
    public string title;
    public string verb;
    public string resourcePath;
    public string description;
    public bool isComplete;
    public bool isOptional;

    public Dialogue startDialogue;
    public Dialogue endDialogue;

    public CollectionQuest(int sourceID, int id, List<int> chainQuestID, int colAmount, Item itemToCollect, 
        string title, string verb, string description, bool isOptional, Dialogue startDialogue, Dialogue endDialogue)
    {
        this.sourceID = sourceID;
        this.id = id;
        this.chainQuestID = chainQuestID;
        this.colAmount = colAmount;
        this.itemToCollect = itemToCollect;
        this.title = title;
        this.verb = verb;
        this.description = description;
        this.isOptional = isOptional;
        this.startDialogue = startDialogue;
        this.endDialogue = endDialogue;
        //CheckProgress();
    }

    public void Duplicate(CollectionQuest cq) {
        this.sourceID = cq.sourceID;
        this.id = cq.id;
        this.chainQuestID = cq.chainQuestID;
        this.colAmount = cq.colAmount;
        this.itemToCollect = cq.itemToCollect;
        this.title = cq.title;
        this.verb = cq.verb;
        this.description = cq.description;
        this.isOptional = cq.isOptional;
        this.startDialogue = cq.startDialogue;
        this.endDialogue = cq.endDialogue;
    }

    public string GetGameObjectName() {
        return itemToCollect.name;
    }

    public void CheckProgress()
    {
        bool itemExist=false;
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        for (int i = 0; i < playerData.inventoryItem.Count; i++)
        {
            if (playerData.inventoryItem[i].id == itemToCollect.id)
            {
                curAmount = playerData.inventoryItem[i].quantity;
                itemExist = true;
                break;
            }
        }
        if (itemExist == false)
        {
            curAmount = 0;
        }

        if (curAmount >= colAmount)
        {
            isComplete = true;
            Debug.Log(title + " quest is complete");
        }
        else
        {
            isComplete = false;
        }
    }

    public void QuestComplete()
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        for (int i = 0; i < playerData.inventoryItem.Count; i++)
        {
            if (playerData.inventoryItem[i].id == itemToCollect.id)
            {
                playerData.inventoryItem[i].quantity -= colAmount;
                Debug.Log("innn");
                break;
            }
        }

        try
        {
            for (int i = 0; i < chainQuestID.Count; i++)
                for (int j = 0; j < QuestDataBase.collectionQuest.Count; j++)
                    if (chainQuestID[i] == QuestDataBase.collectionQuest[j].id)
                    {
                        GameObject.FindGameObjectWithTag("Quest").GetComponent<Quest>().collectionQuestActive.Add(QuestDataBase.collectionQuest[j]);
                        break;
                    }
        }
        catch {
            Debug.Log("This quest doesnt have chain quest");
        }
    }

    public override string ToString()
    {
        return curAmount + "/" + colAmount + " " + itemToCollect.itemName + " " +verb + "ed.";
    }
}
