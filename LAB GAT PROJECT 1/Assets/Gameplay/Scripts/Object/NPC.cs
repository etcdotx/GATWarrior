using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public PlayerData playerData;

    [Header("Set source number")]
    public int sourceID;    

    [Header("Set first active quest id")]
    public List<CollectionQuest> activeCollectionQuest = new List<CollectionQuest>();
    public List<QuestDialog> questDialogList = new List<QuestDialog>();
    public List<QuestDialog> questCompleteDialogList = new List<QuestDialog>();

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
    }

    public void GetQuestDialog()
    {
        for (int i = 0; i < activeCollectionQuest.Count; i++)
        {
            for (int j = 0; j < QuestDataBase.questDialog.Count; j++)
            {
                if (activeCollectionQuest[i].sourceID == QuestDataBase.questDialog[j].sourceID)
                {
                    questDialogList.Add(QuestDataBase.questDialog[j]);
                }
            }
        }
    }

    public void GetQuestCompleteDialog()
    {
        for (int i = 0; i < activeCollectionQuest.Count; i++)
        {
            for (int j = 0; j < QuestDataBase.questCompleteDialog.Count; j++)
            {
                if (activeCollectionQuest[i].sourceID == QuestDataBase.questCompleteDialog[j].sourceID)
                {
                    questCompleteDialogList.Add(QuestDataBase.questCompleteDialog[j]);
                }
            }
        }
    }
}
