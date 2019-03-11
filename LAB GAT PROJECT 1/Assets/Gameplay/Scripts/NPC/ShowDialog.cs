using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDialog : MonoBehaviour
{
    public Player player;
    public List<string> dialog = new List<string>();
    public int questID;
    public int dialNum;
    public bool isQuestDialog;
    public Text conversation;
    public InputSetup inputSetup;

    public void Start()
    {
        conversation = GameObject.FindGameObjectWithTag("Conversation").GetComponent<Text>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void Update()
    {
        if (GameStatus.isTalking == true)
        {
            if (Input.GetKeyDown(inputSetup.deleteSave))
            {
                NextDialogue();
            }
        }
        else
        {
            conversation.gameObject.SetActive(false);
        }
    }

    public void StartDialog(List<string> dial)
    {
        GetDialog(dial);
        dialNum = 0;
        conversation.gameObject.SetActive(true);
        conversation.text = dialog[dialNum];
    }

    public void StartQuestDialog(List<string> dial, int questID)
    {
        GetDialog(dial);
        isQuestDialog = true;
        this.questID = questID;
        dialNum = 0;
        conversation.gameObject.SetActive(true);
        conversation.text = dialog[dialNum];
    }

    private void GetDialog(List<string> dial)
    {
        dialog.Clear();
        for (int i = 0; i < dial.Count; i++)
        {
            dialog.Add(dial[i]);
        }
    }

    public void NextDialogue() {
        if (dialNum == dialog.Count - 1)
        {
            conversation.gameObject.SetActive(false);
            if (isQuestDialog == true)
            {
                AddQuest();
            }
            GameStatus.isTalking = false;
        }
        else
        {
            dialNum++;
            conversation.text = dialog[dialNum];
        }
    }

    public void AddQuest()
    {
        for (int i = 0; i < QuestDataBase.collectionQuest.Count; i++)
        {
            if (QuestDataBase.collectionQuest[i].id == this.questID)
            {
                player.AddQuest(QuestDataBase.collectionQuest[i]);
                Debug.Log(QuestDataBase.collectionQuest[i]);
            }
        }
    }
}
