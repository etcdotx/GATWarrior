using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDialog : MonoBehaviour
{
    [Header("Script List")]
    public Player player;

    [Header("Value")]
    public bool isQuestDialog;
    public int questID;
    public int dialNum;
    public List<string> dialog = new List<string>();
    public Text conversationText;

    public GameObject conversationButton;
    public Text interactText;

    public void Start()
    {
        conversationText = GameObject.FindGameObjectWithTag("Conversation").transform.Find("ConversationText").GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();

        conversationButton = GameObject.FindGameObjectWithTag("Interactable").transform.Find("ConversationButton").gameObject;
        interactText = GameObject.FindGameObjectWithTag("Interactable").transform.Find("InteractText").GetComponent<Text>();
    }

    public void Update()
    {
        if (GameStatus.isTalking == true && InputHolder.isInputHolded == false)
        {
            if (dialNum == dialog.Count - 1)
            {
                interactText.text = "End";
            }
            else
            {
                interactText.text = "Continue";
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                NextDialogue();
            }
        }
    }

    public void StartDialog(List<string> dial)
    {
        dialNum = 0;
        InputHolder.isInputHolded = true;
        GetDialog(dial);
        ShowUI();
    }

    public void StartQuestDialog(List<string> dial, int questID)
    {
        dialNum = 0;
        InputHolder.isInputHolded = true;
        GetDialog(dial);
        ShowUI();
        isQuestDialog = true;
        this.questID = questID;
    }

    void ShowUI()
    {
        conversationText.gameObject.SetActive(true);
        conversationText.text = dialog[dialNum];
        conversationButton.SetActive(true);
        interactText.gameObject.SetActive(true);
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
            EndDialog();
        }
        else
        {
            dialNum++;
            conversationText.text = dialog[dialNum];
        }
    }

    public void EndDialog()
    {
        conversationButton.SetActive(false);
        interactText.gameObject.SetActive(false);
        conversationText.gameObject.SetActive(false);
        if (isQuestDialog == true)
        {
            AddQuest();
        }
        isQuestDialog = false;
        questID = 0;
        dialog.Clear();
        InputHolder.isInputHolded = true;
        GameStatus.isTalking = false;
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
