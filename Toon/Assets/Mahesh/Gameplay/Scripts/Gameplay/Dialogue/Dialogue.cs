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
    public Quest quest;
    public Inventory inventory;

    [Header("Util")]
    public NPC target;
    public List<string> dialog = new List<string>();
    public List<CollectionQuest> colQuestList = new List<CollectionQuest>();
    public List<string> interactDialogList = new List<string>();
    public bool inputHold;
    public bool haveDialogOption;
    public bool isAQuestDialog;
    public int questID;
    public int dialNum;

    [Header("UI")]
    public GameObject dialogueOptionPrefab;
    public GameObject dialogueUI;
    public GameObject dialogueOptionView;
    public GameObject dialogueOptionContent;
    public Scrollbar dialogueOptionScrollbar;
    public GameObject dialogueButton;
    public TextMeshProUGUI dialogueText;
    public Text interactText;
    public List<GameObject> dialogOptionList = new List<GameObject>();

    [Header("Input")]
    public Vector3 inputAxis;
    public int nonDialogueIndex;
    public int dialogueOptionIndex;
    public int dialogueOptionMaxIndex;

    public void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        quest = GameObject.FindGameObjectWithTag("Quest").GetComponent<Quest>();

        dialogueUI = GameObject.FindGameObjectWithTag("DialogueUI");
        dialogueOptionView = dialogueUI.transform.Find("DialogueOptionView").gameObject;
        dialogueOptionContent = dialogueOptionView.transform.Find("DialogueOptionViewPort").Find("DialogueOptionContent").gameObject;
        dialogueOptionScrollbar = dialogueOptionView.transform.Find("DialogueOptionScrollbar").GetComponent<Scrollbar>();
        dialogueText = dialogueUI.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();

        dialogueButton = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("DialogueButton").gameObject;
        interactText = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("InteractText").GetComponent<Text>();

        dialogueOptionView.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        haveDialogOption = false;
        isAQuestDialog = false;
        nonDialogueIndex = 0;
    }

    public void Update()
    {
        GetInputAxis();
        if (GameStatus.isTalking == true && inputHold == false)
        {
            if (haveDialogOption == true)
            {
                if (inputAxis.y == 1 || inputAxis.y == -1)
                {
                    StartCoroutine(InputHold());
                    ChooseDialogue();
                }
                if (Input.GetKeyDown(inputSetup.interact))
                {
                    ConfirmDialogSelection();
                    if (dialNum == dialog.Count - 1)
                        interactText.text = "End";
                    else
                        interactText.text = "Continue";
                }
                if (Input.GetKeyDown(inputSetup.back))
                {
                    CancelTalk();
                }
            }
            else if (haveDialogOption == false)
            {
                Talking();
            }
        }
    }

    void Talking()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            NextDialogue();
            StartCoroutine(InputHold());
            if (dialNum == dialog.Count - 1)
                interactText.text = "End";
            else
                interactText.text = "Continue";
        }
    }


    public void StartNewDialogue(NPC target, List<CollectionQuest> cqList, List<string> npcDialog, string optionDialog, bool haveDialogOption)
    {
        this.target = target;
        this.haveDialogOption = haveDialogOption;
        ShowUI(haveDialogOption);
        StartCoroutine(InputHold());

        //add normal dialogue
        for (int i = 0; i < npcDialog.Count; i++)
        {
            string a = npcDialog[i];
            interactDialogList.Add(a);
        }
        if (haveDialogOption == true)
        {
            InstantiateDialogueOption(cqList);
            interactText.text = "Choose";
            dialogueText.text = optionDialog;
        }
        else
        {
            dialog = interactDialogList;
            dialogueText.text = dialog[dialNum];
        }
        ScrollOption();
    }

    void ConfirmDialogSelection()
    {
        StartCoroutine(InputHold());
        SelectDialogue();
        if (isAQuestDialog == true)
        {
            CheckQuest();
        }
        else
        {
            dialog = interactDialogList;
            dialogueText.text = dialog[dialNum];
        }

        dialogueOptionView.SetActive(false);
        haveDialogOption = false;
    }

    void SelectDialogue()
    {
        if (dialogueOptionIndex <= nonDialogueIndex-1)
        {
            isAQuestDialog = false;
        }
        else
        {
            isAQuestDialog = true;
        }
    }

    void CheckQuest()
    {
        bool checkExist = false;
        for (int i = 0; i < target.activeCollectionQuest.Count; i++)
        {
            if (target.activeCollectionQuest[i].id == dialogueOptionContent.transform.GetChild(dialogueOptionIndex).GetComponent<DialogueOption>().questID)
            {
                for (int j = 0; j < playerData.collectionQuest.Count; j++)
                {
                    if (playerData.collectionQuest[j].id == target.activeCollectionQuest[i].id)
                    {
                        checkExist = true;
                        if (playerData.collectionQuest[j].isComplete == true)
                        {
                            Debug.Log("Quest is complete");
                            SetQuestCompleteDialogue();
                            playerData.collectionQuest[j].QuestComplete();
                            inventory.RefreshInventory();
                            target.activeCollectionQuest.RemoveAt(i);
                            for (int b = 0; b < quest.collectionQuestActive.Count; b++)
                                if (quest.collectionQuestActive[b].id == playerData.collectionQuest[j].id)
                                {
                                    quest.collectionQuestActive.RemoveAt(b);
                                    break;
                                }
                            playerData.AddCollectionQuestComplete(playerData.collectionQuest[j]);
                            quest.ActivateQuest();
                            break;
                        }
                        SetQuestDialogue();
                    }
                    else
                    {
                        Debug.Log("gada quest");
                        checkExist = false;
                    }
                }

                if (checkExist == false)
                {
                    playerData.AddQuest(target.activeCollectionQuest[i]);
                    SetQuestDialogue();
                }
                break;
            }
        }
    }

    void SetQuestDialogue()
    {
        dialog.Clear();
        for (int i = 0; i < target.questDialogList.Count; i++)
            if (target.questDialogList[i].questID == dialogueOptionContent.transform.GetChild(dialogueOptionIndex).GetComponent<DialogueOption>().questID)
                dialog.Add(target.questDialogList[i].dialog);

        dialogueText.text = dialog[dialNum];
    }

    void SetQuestCompleteDialogue()
    {
        dialog.Clear();
        for (int i = 0; i < target.questCompleteDialogList.Count; i++)
            if (target.questCompleteDialogList[i].questID == dialogueOptionContent.transform.GetChild(dialogueOptionIndex).GetComponent<DialogueOption>().questID)
                dialog.Add(target.questCompleteDialogList[i].dialog);

        Debug.Log(dialog.Count);
        Debug.Log(dialog[dialNum]);
        dialogueText.text = dialog[dialNum];
    }

    void DestroyOption()
    {
        for (int i = 0; i < dialogueOptionContent.transform.childCount; i++)
            Destroy(dialogueOptionContent.transform.GetChild(i).gameObject);
    }

    void InstantiateDialogueOption(List<CollectionQuest> cqList)
    {
        int index = -1;
        int cqIndex = 0;

        for (int i = 0; i < cqList.Count; i++)
        {
            CollectionQuest newCol = new CollectionQuest(cqList[i].sourceID, cqList[i].id, cqList[i].chainQuestID,
                cqList[i].colAmount, cqList[i].itemToCollect, cqList[i].title, cqList[i].verb,
                cqList[i].description, cqList[i].isOptional);
            colQuestList.Add(newCol);
        }

        //for normal dialogue
        Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform);
        dialogueOptionContent.transform.GetChild(nonDialogueIndex).GetComponent<DialogueOption>().optionText.text = "Talk";
        index++;
        nonDialogueIndex++;
        //for shop

        if (target.isAShop == true)
        {
            Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform);
            dialogueOptionContent.transform.GetChild(nonDialogueIndex).GetComponent<DialogueOption>().optionText.text = "Buy";
            index++;
            nonDialogueIndex++;
            Debug.Log("in");
        }

        //for questdialogue
        for (int i = 0; i < colQuestList.Count; i++)
        {
            Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform);
            index++;
        }

        for (int i = nonDialogueIndex; i < dialogueOptionContent.transform.childCount; i++)
        {
            dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().optionText.text = colQuestList[cqIndex].title;
            dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().questID = colQuestList[cqIndex].id;
            dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().questIndicatorNew.SetActive(true);
            dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().questIndicatorComplete.SetActive(false);
            for (int j = 0; j < playerData.collectionQuest.Count; j++)
                if (playerData.collectionQuest[j].id == colQuestList[cqIndex].id)
                {
                    playerData.collectionQuest[j].CheckProgress();
                    if (playerData.collectionQuest[j].isComplete == true)
                    {
                        dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().questIndicatorNew.SetActive(false);
                        dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().questIndicatorComplete.SetActive(true);
                        break;
                    }
                    else
                        break;
                }

            cqIndex++;
        }

        //set cursor
        dialogueOptionMaxIndex = index;
        for (int i = 1; i <= dialogueOptionMaxIndex; i++)
            dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().cursor.SetActive(false);
    }

    #region UTILITY
    IEnumerator InputHold()
    {
        inputHold = true;
        yield return new WaitForSeconds(0.15f);
        inputHold = false;
    }
    void ShowUI(bool haveOption)
    {
        dialogueText.gameObject.SetActive(true);
        dialogueButton.SetActive(true);

        if (haveOption == true)
            dialogueOptionView.SetActive(true);
    }
    void GetInputAxis()
    {
        inputAxis.y = Input.GetAxisRaw("D-Pad Up");
        inputAxis.x = Input.GetAxisRaw("D-Pad Right");
    }
    void ChooseDialogue()
    {
        if (inputAxis.y == -1)
            if (dialogueOptionIndex < dialogueOptionMaxIndex)
                dialogueOptionIndex++;
        if (inputAxis.y == 1)
            if (dialogueOptionIndex > 0)
                dialogueOptionIndex--;
        ResetCursor();
        ScrollOption();
    }
    void CancelTalk()
    {
        StartCoroutine(InputHold());
        dialogueOptionView.SetActive(false);
        haveDialogOption = false;
        EndDialog();
    }
    void ResetCursor()
    {
        for (int i = 0; i <= dialogueOptionMaxIndex; i++)
            dialogueOptionContent.transform.GetChild(i).GetComponent<DialogueOption>().cursor.SetActive(false);

        dialogueOptionContent.transform.GetChild(dialogueOptionIndex).GetComponent<DialogueOption>().cursor.SetActive(true);
    }
    void ScrollOption()
    {
        //max =2 min=0
        if (dialogueOptionIndex == dialogueOptionMaxIndex)
            dialogueOptionScrollbar.value = 0;
        else
        {
            float a = (float)dialogueOptionMaxIndex;
            float b = (float)dialogueOptionIndex;
            float c = a - b;
            float d = c / a;
            dialogueOptionScrollbar.value = c / a;
        }
    }
    public void NextDialogue()
    {
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
        //quest.ActivateQuest();
        DestroyOption();
        ClearList();
        dialogueButton.SetActive(false);
        interactText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        //mereset class ini
        GameStatus.isTalking = false;
        InputHolder.isInputHolded = true;
    }
    void ClearList()
    {
        isAQuestDialog = false;
        dialNum = 0;
        nonDialogueIndex = 0;
        dialogueOptionIndex = 0;
        dialogueOptionMaxIndex = 0;
        dialog.Clear();
        interactDialogList.Clear();
        colQuestList.Clear();
    }
    #endregion
}
