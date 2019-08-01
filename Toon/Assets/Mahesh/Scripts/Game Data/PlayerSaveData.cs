using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    /// <summary>
    /// list save data untuk player
    /// 1. quest
    /// 2. quest yang sudah selesai
    /// 3. item di inventory
    /// 4. item di inventorybox
    /// 5. gold
    /// 
    /// list save data untuk npc
    /// 1. quest yang npc punya
    /// 2. item yang npc punya
    /// </summary>
    

    //player
    public List<int> collectionQuestID = new List<int>();
    public List<int> collectionQuestCompleteID = new List<int>();
    public List<int> inventoryItemID = new List<int>();
    public List<int> inventoryBoxItemID = new List<int>();
    public int gold;

    public PlayerSaveData()
    {
        Debug.Log("in");
        //add collection quest
        for (int i = 0; i < PlayerData.instance.collectionQuest.Count; i++)
        {
            collectionQuestID.Add(PlayerData.instance.collectionQuest[i].id);
        }

        //add collectionquestcomplete
        for (int i = 0; i < PlayerData.instance.collectionQuestComplete.Count; i++)
        {
            collectionQuestCompleteID.Add(PlayerData.instance.collectionQuestComplete[i].id);
        }
        
        //add inventory item
        for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
        {
            inventoryItemID.Add(PlayerData.instance.inventoryItem[i].id);
        }

        //add inventory item complete
        for (int i = 0; i < PlayerData.instance.inventoryBoxItem.Count; i++)
        {
            inventoryBoxItemID.Add(PlayerData.instance.inventoryBoxItem[i].id);
        }

        //add gold
        gold = PlayerData.instance.gold;
    }
}
