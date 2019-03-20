using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("Script List")]
    public PlayerData playerData;
    public InputSetup inputSetup;

    [Header("Util")]
    public List<string> dialog = new List<string>();
    public bool inputHold;
    public bool haveDialogOption;
    public int questID;
    public int dialNum;

    [Header("UI")]
    public GameObject dialogueOptionPrefab;
    public GameObject dialogueUI;
    public GameObject dialogueOptionView;
    public GameObject dialogueOptionContent;
    public GameObject conversationButton;
    public TextMeshProUGUI dialogueText;
    public Text interactText;
    public List<GameObject> dialogOptionList = new List<GameObject>();

    [Header("Input")]
    public Vector3 inputAxis;
    public int dialogueOptionIndex;
    public int dialogueOptionMaxIndex;

    public void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();

        dialogueUI = GameObject.FindGameObjectWithTag("DialogueUI");
        dialogueOptionView = dialogueUI.transform.Find("DialogueOptionView").gameObject;
        dialogueOptionContent = dialogueOptionView.transform.Find("DialogueOptionViewPort").Find("DialogueOptionContent").gameObject;
        dialogueText = dialogueUI.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();

        conversationButton = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("ConversationButton").gameObject;
        interactText = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("InteractText").GetComponent<Text>();

        haveDialogOption = false;
        dialogueUI.SetActive(false);
    }

    

    public void Update()
    {
        GetInputAxis();
        if (haveDialogOption == true && inputHold==false)
        {
            if (inputAxis.y == 1 || inputAxis.y == -1 )
            {
                StartCoroutine(InputHold());
                ChooseDialogue();
            }
            if (Input.GetKeyDown(inputSetup.select))
            {
                //SelectDialogue();
            }
        }
        //if (GameStatus.isTalking == true && inputHold == false)
        //{
        //    if (dialNum == dialog.Count - 1)
        //    {
        //        interactText.text = "End";
        //    }
        //    else
        //    {
        //        interactText.text = "Continue";
        //    }
        //    if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        //    {
        //        NextDialogue();
        //        StartCoroutine(InputHold());
        //    }
        //}
    }

    void GetInputAxis()
    {
        inputAxis.y = Input.GetAxisRaw("D-Pad Up");
        inputAxis.x = Input.GetAxisRaw("D-Pad Right");
    }

    void ChooseDialogue()
    {
        if (inputAxis.y == -1)
        {
            if (dialogueOptionIndex < dialogueOptionMaxIndex)
            {
                dialogueOptionIndex++;
            }
        }
        if (inputAxis.y == 1)
        {
            if (dialogueOptionIndex > 0)
            {
                dialogueOptionIndex--;
            }
        }
        ResetCursor();
    }

    void ResetCursor() {
        for (int i = 0; i <= dialogueOptionMaxIndex; i++)
        {
            dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().cursor.SetActive(false);
        }
        dialogueOptionContent.transform.GetChild(dialogueOptionIndex).GetComponent<DialogueOption>().cursor.SetActive(true);
    }

    public void showDialogueOption(List<CollectionQuest> cqList)
    {
        int index=-1;
        int cqIndex = 0;
        GameStatus.isTalking = true;
        dialogueUI.SetActive(true);
        StartCoroutine(InputHold());
        haveDialogOption = true;

        //for normal dialogue
        Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform);
        dialogueOptionContent.transform.GetChild(0).GetComponent<DialogueOption>().optionText.text = "Talk";
        index++;

        for (int i = 0; i < cqList.Count; i++)
        {
            //for questdialogue
            Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform);
            index++;
        }
        for (int i = index; i < dialogueOptionContent.transform.childCount; i++)
        {
            dialogueOptionContent.transform.GetChild(index).GetComponent<DialogueOption>().optionText.text = cqList[cqIndex].title;
            cqIndex++;
        }
        dialogueOptionMaxIndex = index;
        for (int i = 1; i <= dialogueOptionMaxIndex; i++)
        {
            dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().cursor.SetActive(false);
        }
    }

    public void StartDialog(List<string> dial)
    {
        dialNum = 0;
        GetDialog(dial);
        ShowUI();
        StartCoroutine(InputHold());
    }

    void ShowUI()
    {
        dialogueText.gameObject.SetActive(true);
        dialogueText.text = dialog[dialNum];
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
            dialogueText.text = dialog[dialNum];
        }
    }

    public void EndDialog()
    {
        conversationButton.SetActive(false);
        interactText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);

        //mereset class ini
        questID = 0;
        dialog.Clear();
        InputHolder.isInputHolded = true;
        GameStatus.isTalking = false;
    }

    IEnumerator InputHold()
    {
        inputHold = true;
        yield return new WaitForSeconds(0.15f);
        inputHold = false;
    }
}
