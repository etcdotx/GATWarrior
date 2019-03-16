using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDialog : MonoBehaviour
{
    [Header("Script List")]
    public Player player;

    [Header("Value")]
    public bool inputHold;
    public bool isQuestDialog;
    public bool isQuestCompleteDialog;
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
        if (GameStatus.isTalking == true && inputHold == false)
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
                StartCoroutine(InputHold());
            }
        }
    }

    public void StartDialog(List<string> dial)
    {
        dialNum = 0;
        StartCoroutine(InputHold());
        GetDialog(dial);
        ShowUI();
    }

    public void StartQuestDialog(List<string> dial, int questID)
    {
        //memulai dialog untuk quest
        dialNum = 0;
        StartCoroutine(InputHold());
        //memasukkan dialog yang dikirim kedalam list dialog
        GetDialog(dial);
        //menunjukkan ui dialog
        ShowUI();
        //menunjukkan bahwa dialog ini adalah dialog quest
        isQuestDialog = true;
        //data quest id dari dialog ini
        this.questID = questID;
    }

    public void StartQuestCompleteDialog(List<string> dial, int questID)
    {
        dialNum = 0;
        StartCoroutine(InputHold());
        GetDialog(dial);
        ShowUI();
        isQuestCompleteDialog = true;
    }

    void ShowUI()
    {
        conversationText.gameObject.SetActive(true);
        conversationText.text = dialog[dialNum];
        conversationButton.SetActive(true);
        interactText.gameObject.SetActive(true);
    }

    //memasukkan dialog yang dikirim kedalam list dialog
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

        //jika ini adalah dialog untuk quest, maka quest tersebut dimasukkan ke player
        if (isQuestDialog == true)
        {
            //AddQuest();
        }

        //jika ini adalah percakapan untuk memberitahu bahwa quest ini selesai
        if (isQuestCompleteDialog == true)
        {
            // Debug.Log("Quest complete!");
        }

        //mereset class ini
        isQuestDialog = false;
        isQuestCompleteDialog = false;
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
                //jika quest id tersebut sesuai dengan yang didatabase, maka player memasukkan quest tersebut
                player.AddQuest(QuestDataBase.collectionQuest[i]);
            }
        }
    }

    IEnumerator InputHold()
    {
        inputHold = true;
        yield return new WaitForSeconds(0.15f);
        inputHold = false;
    }
}
