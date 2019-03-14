using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Player player;
    public int sourceID;

    public bool isHavingQuest;

    public int questIDActive;

    public List<CollectionQuest> collectionQuestList = new List<CollectionQuest>();
    public List<QuestDialog> questDialogList = new List<QuestDialog>();
    public List<string> questDialogActive = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        GetQuestList();
        GetQuestDialog();
        if (questDialogList.Count != 0)
        {
            isHavingQuest = true;
        }
        else
        {
            isHavingQuest = false;
        }
    }

    public void RefreshQuest()
    {
        for (int i = 0; i < collectionQuestList.Count; i++)
        {
            if (collectionQuestList[i].id == questIDActive)
            {
                if (collectionQuestList[i].chainQuestID != 0)
                {
                    questIDActive = collectionQuestList[i].chainQuestID;
                    RefreshQuestDialog();
                    isHavingQuest = true;
                    break;
                }
            }
            else
            {
                isHavingQuest = false;
            }
        }
    }

    public void GetQuestList()
    {
        for (int i = 0; i < QuestDataBase.collectionQuest.Count; i++)
        {
            if (QuestDataBase.collectionQuest[i].sourceID == sourceID)
            {
                collectionQuestList.Add(QuestDataBase.collectionQuest[i]);
            }
        }
    }

    public void GetQuestDialog()
    {
        for (int i = 0; i < QuestDataBase.questDialog.Count; i++)
        {
            if (QuestDataBase.questDialog[i].sourceID == sourceID)
            {
                questDialogList.Add(QuestDataBase.questDialog[i]);
            }
        }
        RefreshQuestDialog();
    }

    public void RefreshQuestDialog()
    {
        questDialogActive.Clear();
        for (int i = 0; i < questDialogList.Count; i++)
        {
            if (questDialogList[i].questID == questIDActive)
            {
                questDialogActive.Add(questDialogList[i].dialog);
            }
        }
    }
}
