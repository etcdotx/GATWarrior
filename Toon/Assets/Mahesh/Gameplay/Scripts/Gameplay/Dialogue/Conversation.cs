using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Conversation : MonoBehaviour
{
    [Header("Script List")]
    public PlayerData playerData;
    public InputSetup inputSetup;
    public SoundList soundList;
    public Shop shop;
    public Quest quest;
    public Inventory inventory;

    [Header("Util")]
    public NPC target;
    public Dialogue dialogue;
    public List<CollectionQuest> colQuestList = new List<CollectionQuest>();
    public List<DialogueOption> dialogueOptionList = new List<DialogueOption>();
    public DialogueOption selectDialogueOption;
    public Dialogue npcDialog;
    public bool isTalking;
    public bool inputHold;
    public bool haveDialogOption;
    public int questID;
    public int dialNum;

    [Header("UI")]
    public GameObject dialogueOptionPrefab;
    public GameObject ConversationUI;
    public GameObject dialogueOptionView;
    public GameObject dialogueOptionContent;
    public Scrollbar dialogueOptionScrollbar;
    public GameObject dialogueButton;
    public TextMeshProUGUI conversationText;
    public Text interactText;

    [Header("Input")]
    public Vector3 inputAxis;
    public int dialogueOptionIndex;

    private void Awake()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        quest = GameObject.FindGameObjectWithTag("Quest").GetComponent<Quest>();
        shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>();

        ConversationUI = GameObject.FindGameObjectWithTag("ConversationUI");
        dialogueOptionView = ConversationUI.transform.Find("DialogueOptionView").gameObject;
        dialogueOptionContent = dialogueOptionView.transform.Find("DialogueOptionViewPort").Find("DialogueOptionContent").gameObject;
        dialogueOptionScrollbar = dialogueOptionView.transform.Find("DialogueOptionScrollbar").GetComponent<Scrollbar>();
        conversationText = ConversationUI.transform.Find("ConversationText").GetComponent<TextMeshProUGUI>();

        dialogueButton = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("DialogueButton").gameObject;
        interactText = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("InteractText").GetComponent<Text>();

        soundList = GameObject.FindGameObjectWithTag("SoundList").GetComponent<SoundList>();
    }

    public void Start()
    {
        dialogueOptionView.SetActive(false);
        conversationText.gameObject.SetActive(false);
        isTalking = false;
        haveDialogOption = false;
    }

    public void Update()
    {
        GetInputAxis();
        if (inputHold == false && isTalking)
        {
            if (haveDialogOption == true)
            {
                if (inputAxis.y == 1 || inputAxis.y == -1)
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
                    StartCoroutine(InputHold());
                    ChooseDialogue();
                }
                if (Input.GetKeyDown(inputSetup.continueTalk))
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                    ConfirmDialogSelection();
                    if (dialogue != null)
                    {
                        if (dialNum == dialogue.dialogue.Length - 1)
                            interactText.text = "End";
                        else
                            interactText.text = "Continue";
                    }
                }
                if (Input.GetKeyDown(inputSetup.back))
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
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
            soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
            NextDialogue();
            StartCoroutine(InputHold());
            if (dialogue != null)
            {
                if (dialNum == dialogue.dialogue.Length - 1)
                    interactText.text = "End";
                else if (dialNum < dialogue.dialogue.Length - 1)
                    interactText.text = "Continue";
            }
        }
    }


    public void StartNewDialogue(NPC target, List<CollectionQuest> cqList, Dialogue npcDialog, string optionDialog, bool haveDialogOption)
    {
        isTalking = true;
        this.target = target;
        this.npcDialog = npcDialog;
        this.haveDialogOption = haveDialogOption;
        ShowUI(haveDialogOption);
        StartCoroutine(InputHold());

        if (haveDialogOption)
        {
            InstantiateDialogueOption(cqList);
            interactText.text = "Choose";
            conversationText.text = optionDialog;
        }
        else
        {
            dialogue = this.npcDialog;
            conversationText.text = npcDialog.dialogue[dialNum];
        }
        ScrollOption();
    }

    void ConfirmDialogSelection()
    {
        StartCoroutine(InputHold());
        SelectDialogue();
    }

    void SelectDialogue()
    {
        if (dialogueOptionList[dialogueOptionIndex].isTalk)
        {
            dialogue = this.npcDialog;
            conversationText.text = dialogue.dialogue[dialNum];
            dialogueOptionView.SetActive(false);
            haveDialogOption = false;
        }
        else if (dialogueOptionList[dialogueOptionIndex].isShop)
        {
            CancelTalk();
            shop.OpenShop(target);
        }
        else if (dialogueOptionList[dialogueOptionIndex].isQuest)
        {
            CheckQuest();
            dialogueOptionView.SetActive(false);
            haveDialogOption = false;
        }
    }

    void CheckQuest()
    {
        bool checkExist = false;
        for (int j = 0; j < playerData.collectionQuest.Count; j++)
        {
            if (playerData.collectionQuest[j].id == dialogueOptionList[dialogueOptionIndex].collectionQuest.id)
            {
                checkExist = true;
                if (playerData.collectionQuest[j].isComplete)
                {
                    Debug.Log("Quest is complete");
                    SetQuestCompleteDialogue();
                    playerData.collectionQuest[j].QuestComplete();
                    inventory.RefreshInventory();
                    for (int b = 0; b < quest.collectionQuestActive.Count; b++)
                    {
                        if (quest.collectionQuestActive[b].id == playerData.collectionQuest[j].id)
                        {
                            quest.collectionQuestActive.RemoveAt(b);
                            break;
                        }
                    }
                    for (int i = 0; i < target.activeCollectionQuest.Count; i++)
                    {
                        if (target.activeCollectionQuest[i].id == dialogueOptionList[dialogueOptionIndex].collectionQuest.id)
                        {
                            target.activeCollectionQuest.RemoveAt(i);
                            break;
                        }
                    }
                    playerData.AddCollectionQuestComplete(playerData.collectionQuest[j]);
                    quest.ActivateQuest();
                    break;
                }
                else {
                    SetQuestDialogue();
                }
                break;
            }
            else
            {
                Debug.Log("gada quest");
                checkExist = false;
            }
        }

        if (!checkExist)
        {
            playerData.AddQuest(dialogueOptionList[dialogueOptionIndex].collectionQuest);
            SetQuestDialogue();
        }
    }

    void SetQuestDialogue()
    {
        dialogue = null;
        dialogue = dialogueOptionContent.transform.GetChild(dialogueOptionIndex).GetComponent<DialogueOption>().collectionQuest.startDialogue;
        conversationText.text = dialogue.dialogue[dialNum];
    }

    void SetQuestCompleteDialogue()
    {
        dialogue = null;
        dialogue = dialogueOptionContent.transform.GetChild(dialogueOptionIndex).GetComponent<DialogueOption>().collectionQuest.endDialogue;
        conversationText.text = dialogue.dialogue[dialNum];
    }

    void DestroyOption()
    {
        for (int i = 0; i < dialogueOptionContent.transform.childCount; i++)
            Destroy(dialogueOptionContent.transform.GetChild(i).gameObject);
    }

    void InstantiateDialogueOption(List<CollectionQuest> cqList)
    {
        for (int i = 0; i < cqList.Count; i++)
        {
            CollectionQuest newCol = ScriptableObject.CreateInstance<CollectionQuest>();
            newCol.Duplicate(cqList[i]);

            colQuestList.Add(newCol);
        }

        //for normal dialogue
        DialogueOption talkDialogueOption = Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform).GetComponent<DialogueOption>();
        talkDialogueOption.optionText.text = "Talk";
        talkDialogueOption.isTalk = true;
        dialogueOptionList.Add(talkDialogueOption);
        //for shop

        if (target.isAShop)
        {
            DialogueOption shopDialogueOption = Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform).GetComponent<DialogueOption>();
            shopDialogueOption.optionText.text = "Buy";
            shopDialogueOption.isShop = true;
            dialogueOptionList.Add(shopDialogueOption);
        }

        //for questdialogue
        for (int i = 0; i < colQuestList.Count; i++)
        {
            DialogueOption newDialogueOption = Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform).GetComponent<DialogueOption>();
            newDialogueOption.collectionQuest = colQuestList[i];
            newDialogueOption.optionText.text = colQuestList[i].title;
            newDialogueOption.isQuest = true;
            newDialogueOption.questIndicatorNew.SetActive(true);
            newDialogueOption.questIndicatorComplete.SetActive(false);

            for (int j = 0; j < playerData.collectionQuest.Count; j++)
                if (playerData.collectionQuest[j].id == newDialogueOption.collectionQuest.id)
                {
                    playerData.collectionQuest[j].CheckProgress();
                    if (playerData.collectionQuest[j].isComplete)
                    {
                        newDialogueOption.questIndicatorNew.SetActive(false);
                        newDialogueOption.questIndicatorComplete.SetActive(true);
                        break;
                    }
                    else
                        break;
                }
            dialogueOptionList.Add(newDialogueOption);
        }

        //set cursor
        for (int i = 1; i < dialogueOptionList.Count; i++)
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
        interactText.gameObject.SetActive(true);
        conversationText.gameObject.SetActive(true);
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
            if (dialogueOptionIndex < dialogueOptionList.Count-1)
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
        for (int i = 0; i < dialogueOptionList.Count; i++)
            dialogueOptionList[i].cursor.SetActive(false);

        dialogueOptionList[dialogueOptionIndex].cursor.SetActive(true);
    }
    void ScrollOption()
    {
        //max =2 min=0
        if (dialogueOptionIndex == dialogueOptionList.Count-1)
            dialogueOptionScrollbar.value = 0;
        else
        {
            float a = (float)dialogueOptionList.Count;
            float b = (float)dialogueOptionIndex;
            float c = a - b;
            float d = c / a;
            dialogueOptionScrollbar.value = c / a;
        }
    }
    public void NextDialogue()
    {
        if (dialNum == dialogue.dialogue.Length-1)
        {
            EndDialog();
        }
        else
        {
            dialNum++;
            conversationText.text = dialogue.dialogue[dialNum];
        }
    }
    public void EndDialog()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CharacterMovement cm = player.GetComponent<CharacterMovement>();
        cm.canMove = true;
        isTalking = false;

        DestroyOption();
        ClearList();
        dialogueButton.SetActive(false);
        interactText.gameObject.SetActive(false);
        conversationText.gameObject.SetActive(false);
        //mereset class ini
        InputHolder.isInputHolded = true;
    }
    void ClearList()
    {
        dialNum = 0;
        dialogueOptionIndex = 0;
        dialogue = null;
        dialogueOptionList.Clear();
        colQuestList.Clear();
    }
    #endregion
}
