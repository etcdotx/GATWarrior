using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Conversation : MonoBehaviour
{
    public static Conversation instance;

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
    public GameObject dialogueOptionView;
    public GameObject dialogueOptionContent;
    public Scrollbar dialogueOptionScrollbar;
    public TextMeshProUGUI conversationText;

    [Header("Input")]
    public Vector3 inputAxis;
    public int dialogueOptionIndex;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        dialogueOptionView = transform.GetChild(0).Find("DialogueOptionView").gameObject;
        dialogueOptionContent = dialogueOptionView.transform.Find("DialogueOptionViewPort").Find("DialogueOptionContent").gameObject;
        dialogueOptionScrollbar = dialogueOptionView.transform.Find("DialogueOptionScrollbar").GetComponent<Scrollbar>();
        conversationText = transform.GetChild(0).Find("ConversationText").GetComponent<TextMeshProUGUI>();
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
        //GetInputAxis();
        if (inputHold == false && isTalking)
        {
            if (haveDialogOption == true)
            {
                if (inputAxis.y == 1 || inputAxis.y == -1)
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
                    StartCoroutine(InputHold());
                    ChooseDialogue();
                }
                if (Input.GetKeyDown(InputSetup.instance.continueTalk))
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                    ConfirmDialogSelection();
                    if (dialogue != null)
                    {
                        if (dialNum == dialogue.dialogue.Length - 1)
                            InteractableIndicator.instance.interactText.text = "End";
                        else
                            InteractableIndicator.instance.interactText.text = "Continue";
                    }
                }
                if (Input.GetKeyDown(InputSetup.instance.back))
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
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
            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
            NextDialogue();
            StartCoroutine(InputHold());
            if (dialogue != null)
            {
                if (dialNum == dialogue.dialogue.Length - 1)
                    InteractableIndicator.instance.interactText.text = "End";
                else if (dialNum < dialogue.dialogue.Length - 1)
                    InteractableIndicator.instance.interactText.text = "Continue";
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
            InteractableIndicator.instance.interactText.text = "Choose";
            conversationText.text = optionDialog;
        }
        else
        {
            dialogue = this.npcDialog;
            conversationText.text = npcDialog.dialogue[dialNum];
        }
        //ScrollOption();
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
            Shop.instance.OpenShop(target);
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
        for (int j = 0; j < PlayerData.instance.collectionQuest.Count; j++)
        {
            if (PlayerData.instance.collectionQuest[j].id == dialogueOptionList[dialogueOptionIndex].collectionQuest.id)
            {
                checkExist = true;
                if (PlayerData.instance.collectionQuest[j].isComplete)
                {
                    Debug.Log("Quest is complete");
                    SetQuestCompleteDialogue();
                    PlayerData.instance.collectionQuest[j].QuestComplete();
                    Inventory.instance.RefreshInventory();
                    for (int b = 0; b < Quest.instance.collectionQuestActive.Count; b++)
                    {
                        if (Quest.instance.collectionQuestActive[b].id == PlayerData.instance.collectionQuest[j].id)
                        {
                            Quest.instance.collectionQuestActive.RemoveAt(b);
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
                    PlayerData.instance.AddCollectionQuestComplete(PlayerData.instance.collectionQuest[j]);
                    Quest.instance.ActivateQuest();
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
            PlayerData.instance.AddQuest(dialogueOptionList[dialogueOptionIndex].collectionQuest);
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

            for (int j = 0; j < PlayerData.instance.collectionQuest.Count; j++)
                if (PlayerData.instance.collectionQuest[j].id == newDialogueOption.collectionQuest.id)
                {
                    PlayerData.instance.collectionQuest[j].CheckProgress();
                    if (PlayerData.instance.collectionQuest[j].isComplete)
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
        InteractableIndicator.instance.interactText.gameObject.SetActive(true);
        conversationText.gameObject.SetActive(true);
        InteractableIndicator.instance.interactButton.gameObject.SetActive(true);

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
        isTalking = false;

        DestroyOption();
        ClearList();
        InteractableIndicator.instance.interactButton.gameObject.SetActive(false);
        InteractableIndicator.instance.interactText.gameObject.SetActive(false);
        conversationText.gameObject.SetActive(false);
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
