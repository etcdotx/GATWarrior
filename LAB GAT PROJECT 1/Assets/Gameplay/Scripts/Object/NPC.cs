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
    public int questDialogListTotal;
    public int questCompleteDialogListTotal;

    [Header("content")]
    public List<CollectionQuest> activeCollectionQuest = new List<CollectionQuest>();
    public List<QuestDialog> questDialogList = new List<QuestDialog>();
    public List<QuestDialog> questCompleteDialogList = new List<QuestDialog>();
    public string optionDialog;
    public List<string> npcDialog = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
    }

    public void GetQuestDialog()
    {
        //Debug.Log(QuestDataBase.questDialog.Count);
        for (int i = 0; i < activeCollectionQuest.Count; i++)
        {
            for (int j = 0; j < QuestDataBase.questDialog.Count; j++)
            {
                if (activeCollectionQuest[i].id == QuestDataBase.questDialog[j].questID)
                {
                    questDialogList.Add(QuestDataBase.questDialog[j]);
                    //Debug.Log(activeCollectionQuest[i].id + "="+QuestDataBase.questDialog[j].questID + "-> "+QuestDataBase.questDialog[j].dialog);
                }
            }
        }
        questDialogListTotal = questDialogList.Count;
    }

    public void GetQuestCompleteDialog()
    {
        for (int i = 0; i < activeCollectionQuest.Count; i++)
        {
            for (int j = 0; j < QuestDataBase.questCompleteDialog.Count; j++)
            {
                if (activeCollectionQuest[i].id == QuestDataBase.questCompleteDialog[j].questID)
                {
                    //Debug.Log(QuestDataBase.questCompleteDialog[j]);
                    questCompleteDialogList.Add(QuestDataBase.questCompleteDialog[j]);
                }
            }
        }
        questCompleteDialogListTotal = questCompleteDialogList.Count;
    }
}
