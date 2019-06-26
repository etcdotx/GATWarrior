using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public PlayerData playerData;

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

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
    }
}
