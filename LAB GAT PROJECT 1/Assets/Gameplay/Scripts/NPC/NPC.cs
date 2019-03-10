using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Player player;
    public int sourceID;

    public string[] questDialog;

    public int[] questID;
    public int[] questIDActive;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void GiveQuest(int questID) {
        //id nya berguna untuk tau quest id yg mana yg bakal dikasih dari GameDatabase Quest
        for (int i = 0; i < QuestDataBase.collectionQuest.Count; i++) {
            if (QuestDataBase.collectionQuest[i].id == questID)
            {
                player.AddQuest(QuestDataBase.collectionQuest[i]);
                break;
            }               
        }
    }
}
