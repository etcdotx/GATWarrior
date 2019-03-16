﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionQuest
{
    public int sourceID;
    public int id;
    public int chainQuestID;
    public int colAmount;
    public int curAmount;
    public GameObject itemToCollect;
    public string title;
    public string verb;
    public string description;
    public bool isComplete;
    public bool isOptional;

    public CollectionQuest(int sourceID, int id, int chainQuestID,int colAmount, string resourcePath, string title, string verb, string description, bool isOptional){
        this.sourceID = sourceID;
        this.id = id;
        this.chainQuestID = chainQuestID;
        this.colAmount = colAmount;
        itemToCollect = Resources.Load(resourcePath) as GameObject;
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

    public override string ToString()
    {
        return curAmount + "/" + colAmount + " " + itemToCollect.name + " " +verb + "ed.";
    }
}