using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionQuest
{
    public int sourceID;
    public int id;
    public List<int> chainQuestID = new List<int>();
    public int colAmount;
    public int curAmount;
    public GameObject itemToCollect;
    public string title;
    public string verb;
    public string resourcePath;
    public string description;
    public bool isComplete;
    public bool isOptional;

    public CollectionQuest(int sourceID, int id, List<int> chainQuestID, int colAmount, string resourcePath, string title, string verb, string description, bool isOptional){
        this.sourceID = sourceID;
        this.id = id;
        this.chainQuestID = chainQuestID;
        this.colAmount = colAmount;
        this.resourcePath = resourcePath;
        itemToCollect = Resources.Load(this.resourcePath) as GameObject;
        this.title = title;
        this.verb = verb;
        this.description = description;
        this.isOptional = isOptional;
        CheckProgress();
    }

    public string GetGameObjectName() {
        return itemToCollect.name;
    }

    public void CheckProgress()
    {
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

    public void QuestComplete() {
        try
        {
            for (int i = 0; i < chainQuestID.Count; i++)
                for (int j = 0; j < QuestDataBase.collectionQuest.Count; j++)
                    if (chainQuestID[i] == QuestDataBase.collectionQuest[j].id)
                    {
                        GameObject.FindGameObjectWithTag("Quest").GetComponent<Quest>().collectionQuestActive.Add(QuestDataBase.collectionQuest[j]);
                        break;
                    }

            PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
            for (int i = 0; i < playerData.inventoryItem.Count; i++)
            {
                if (playerData.inventoryItem[i].name == itemToCollect.name)
                {
                    playerData.inventoryItem[i].quantity -= colAmount;
                    break;
                }
            }
        }
        catch {
            Debug.Log("This quest doesnt have chain quest");
        }
    }

    public override string ToString()
    {
        return curAmount + "/" + colAmount + " " + itemToCollect.name + " " +verb + "ed.";
    }
}
