using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool isAShop;

    [Header("Set source number")]
    public int sourceID;
    public int activeCollectionQuestTotal;

    [Header("Content")]
    public List<CollectionQuest> activeCollectionQuest = new List<CollectionQuest>();
    public string optionDialog;
    public Dialogue npcDialog;

    [Header("Shop")]
    public List<Item> shopItem = new List<Item>();
}
