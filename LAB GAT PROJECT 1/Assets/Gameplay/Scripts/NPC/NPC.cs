using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Player player;
    public int sourceID;

    public bool isHavingQuest;

    public List<int> questID = new List<int>();
    public int questIDActive;

    public List<questDialog> questDialogList = new List<questDialog>();
    public List<string> questDialogActive = new List<string>();

    public class questDialog {
        public string dialog;
        public int questID;

        public questDialog(int questID, string dialog)
        {
            this.dialog = dialog;
            this.questID = questID;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        for (int i = 0; i < QuestDataBase.collectionQuest.Count; i++)
        {
            if (QuestDataBase.collectionQuest[i].sourceID == this.sourceID)
            {
                questID.Add(QuestDataBase.collectionQuest[i].id);
            }
            isHavingQuest = true;
        }
        AddQuestDialog();
        GetQuestDialog();
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

    public void AddQuestDialog()
    {
        questDialog qd1 = new questDialog(1, "Can i ask u something");
        questDialogList.Add(qd1);
        questDialog qd2 = new questDialog(1, "Can u help me to collect some ingredients?");
        questDialogList.Add(qd2);
    }

    public void GetQuestDialog()
    {
        for (int i = 0; i < questDialogList.Count; i++)
        {
            if (questDialogList[i].questID == questIDActive)
            {
                questDialogActive.Add(questDialogList[i].dialog);
            }
        }
    }
}
