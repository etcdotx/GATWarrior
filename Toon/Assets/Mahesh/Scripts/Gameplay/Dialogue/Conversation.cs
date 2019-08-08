using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Conversation : MonoBehaviour
{
    public static Conversation instance;
    
    //dialogue yang muncul
    Dialogue curDialogue;
    //dialogue dari npc
    Dialogue npcDialog;
    //nomor dari dialogue yang dipilih (karena dialogue berbentuk array)
    int dialNum;
    //npc atau object yang sedang diajak conversation
    NPC target;

    [Header("UI")]
    public GameObject dialogueOptionPrefab;
    public GameObject dialogueOptionView;
    public GameObject dialogueOptionContent;
    public Scrollbar dialogueOptionScrollbar;
    public TextMeshProUGUI conversationText;
    public Button conversationButton;
    public Text conversationButtonText;

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
        conversationButton = transform.GetChild(0).Find("ConversationButton").GetComponent<Button>();
        conversationButtonText = transform.GetChild(0).Find("ConversationButtonText").GetComponent<Text>();
    }

    public void Start()
    {
        dialogueOptionView.SetActive(false);
        conversationText.gameObject.SetActive(false);
        conversationButton.gameObject.SetActive(false);
        conversationButtonText.gameObject.SetActive(false);
        conversationButton.interactable = false;
    }

    /// <summary>
    /// function untuk melanjutkan text yang muncul
    /// </summary>
    public void ContinueTalking()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        NextDialogue();
        if (curDialogue != null)
        {
            if (dialNum == curDialogue.dialogue.Length - 1)
                conversationButtonText.text = "End";
            else if (dialNum < curDialogue.dialogue.Length - 1)
                conversationButtonText.text = "Continue";
        }
    }

    /// <summary>
    /// function untuk memulai percakapan
    /// </summary>
    /// <param name="target">target yang diajak ngomong</param>
    /// <param name="cqList">quest jika dia punya</param>
    /// <param name="npcDialog">dialog pasti dari npc tersebut</param>
    /// <param name="optionDialog">dialog pertanyaan dia jika ada option</param>
    /// <param name="haveDialogOption">jika dia punya opsi dialogue</param>
    public void StartNewDialogue(NPC target, List<CollectionQuest> cqList, Dialogue npcDialog, string optionDialog, bool haveDialogOption)
    {
        this.target = target;
        this.npcDialog = npcDialog;
        ShowUI(haveDialogOption);

        if (haveDialogOption)
        {
            InstantiateDialogueOption(cqList);
            conversationButtonText.text = "Choose";
            conversationText.text = optionDialog;
        }
        else
        {
            curDialogue = this.npcDialog;
            conversationText.text = npcDialog.dialogue[dialNum];
            conversationButton.gameObject.SetActive(true);
            if (dialNum == curDialogue.dialogue.Length - 1)
                conversationButtonText.text = "End";
            else if (dialNum < curDialogue.dialogue.Length - 1)
                conversationButtonText.text = "Continue";
            conversationButton.interactable = true;
            UIManager.instance.eventSystem.SetSelectedGameObject(Conversation.instance.conversationButton.gameObject);
        }
    }

    /// <summary>
    /// function untuk memilih opsi dialogue
    /// </summary>
    public void SelectTalkOption() {
        curDialogue = this.npcDialog;
        conversationText.text = curDialogue.dialogue[dialNum];
        StartSelectedDialogue();
    }

    /// <summary>
    /// function untuk memilih opsi shop
    /// </summary>
    public void SelectShopOption()
    {
        CancelTalk();
        Shop.instance.OpenShop(target);
    }

    /// <summary>
    /// function untuk memilih opsi berjenis quest
    /// </summary>
    /// <param name="dialogueQuest">quest yang dipilih</param>
    public void SelectQuestOption(CollectionQuest dialogueQuest) {
        CheckQuest(dialogueQuest);
        StartSelectedDialogue();
    }

    /// <summary>
    /// function ketika sudah memilih opsi yang memunculkan dialogue
    /// </summary>
    void StartSelectedDialogue()
    {
        dialogueOptionView.SetActive(false);

        if (dialNum == curDialogue.dialogue.Length - 1)
            conversationButtonText.text = "End";
        else if (dialNum < curDialogue.dialogue.Length - 1)
            conversationButtonText.text = "Continue";
    }

    /// <summary>
    /// lanjutan dari selectquestoption
    /// function untuk ngecheck quest yang dipilih
    /// </summary>
    /// <param name="dialogueQuest">quest yang dipilih</param>
    void CheckQuest(CollectionQuest dialogueQuest)
    {
        bool checkExist = false;
        for (int j = 0; j < PlayerData.instance.collectionQuest.Count; j++)
        {
            if (PlayerData.instance.collectionQuest[j].id == dialogueQuest.id)
            {
                checkExist = true;
                if (PlayerData.instance.collectionQuest[j].isComplete)
                {
                    Debug.Log("Quest is complete");
                    SetQuestCompleteDialogue(dialogueQuest);

                    PlayerData.instance.collectionQuest[j].QuestComplete();
                    Inventory.instance.RefreshInventory();


                    RemoveCompleteQuest(dialogueQuest);
                    RemoveQuestFromNpc(dialogueQuest);

                    PlayerData.instance.AddCollectionQuestComplete(PlayerData.instance.collectionQuest[j]);
                    Quest.instance.ActivateQuest();
                    break;
                }
                else {
                    SetQuestDialogue(dialogueQuest);
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
            PlayerData.instance.AddQuest(dialogueQuest);
            SetQuestDialogue(dialogueQuest);
        }
    }

    /// <summary>
    /// jika quest sudah selesai
    /// maka quest akan di remove dari quest yang aktif pada instance Quest
    /// </summary>
    /// <param name="completeQuest">quest yang sudah selesai</param>
    void RemoveCompleteQuest(CollectionQuest completeQuest)
    {
        for (int i = 0; i < Quest.instance.collectionQuestActive.Count; i++)
        {
            if (Quest.instance.collectionQuestActive[i].id == completeQuest.id)
            {
                Quest.instance.collectionQuestActive.RemoveAt(i);
                break;
            }
        }
    }

    /// <summary>
    /// jika quest sudah selesai
    /// maka quest akan di remove dari npc
    /// </summary>
    /// <param name="completeQuest"></param>
    void RemoveQuestFromNpc(CollectionQuest completeQuest) {
        for (int i = 0; i < target.activeCollectionQuest.Count; i++)
        {
            if (target.activeCollectionQuest[i].id == completeQuest.id)
            {
                target.activeCollectionQuest.RemoveAt(i);
                break;
            }
        }
    }

    /// <summary>
    /// function untuk menjadikan curDialogue menjadi start dialogue yang ada pada quest
    /// </summary>
    /// <param name="selectedQuest">quest yang dipilih</param>
    void SetQuestDialogue(CollectionQuest selectedQuest)
    {
        dialogueOptionView.SetActive(false);
        curDialogue = null;
        curDialogue = selectedQuest.startDialogue;
        conversationText.text = curDialogue.dialogue[dialNum];
    }

    /// <summary>
    /// function untuk menjadikan curDialogue menjadi end dialogue yang ada pada quest
    /// </summary>
    /// <param name="selectedQuest">quest yang dipilih</param>
    void SetQuestCompleteDialogue(CollectionQuest selectedQuest)
    {
        dialogueOptionView.SetActive(false);
        curDialogue = null;
        curDialogue = selectedQuest.endDialogue;
        conversationText.text = curDialogue.dialogue[dialNum];
    }

    /// <summary>
    /// function untuk nge spawn dialogue option
    /// </summary>
    /// <param name="cqList">quest list dari target</param>
    void InstantiateDialogueOption(List<CollectionQuest> cqList)
    {
        //for normal dialogue
        DialogueOption talkDialogueOption = Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform).GetComponent<DialogueOption>();
        talkDialogueOption.optionText.text = "Talk";
        talkDialogueOption.dialogueType = DialogueOption.DialogueType.Talk;
        UIManager.instance.eventSystem.SetSelectedGameObject(talkDialogueOption.gameObject);

        //for shop
        if (target.isAShop)
        {
            DialogueOption shopDialogueOption = Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform).GetComponent<DialogueOption>();
            shopDialogueOption.optionText.text = "Buy";
            talkDialogueOption.dialogueType = DialogueOption.DialogueType.Shop;
        }

        //for questdialogue
        for (int i = 0; i < cqList.Count; i++)
        {
            DialogueOption newDialogueOption = Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform).GetComponent<DialogueOption>();
            newDialogueOption.collectionQuest = cqList[i];
            newDialogueOption.optionText.text = cqList[i].title;
            talkDialogueOption.dialogueType = DialogueOption.DialogueType.Quest;
            newDialogueOption.questIndicatorNew.SetActive(true);
            newDialogueOption.questIndicatorComplete.SetActive(false);

            for (int j = 0; j < PlayerData.instance.collectionQuest.Count; j++)
            {
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
            }
        }
    }

    /// <summary>
    /// function untuk show ui
    /// </summary>
    /// <param name="haveOption">jika memiliki opsi</param>
    void ShowUI(bool haveOption)
    {
        conversationText.gameObject.SetActive(true);
        conversationButton.gameObject.SetActive(true);
        conversationButtonText.gameObject.SetActive(true);

        if (haveOption == true)
            dialogueOptionView.SetActive(true);
    }

    /// <summary>
    /// funtion untuk tidak jadi berbicara
    /// </summary>
    public void CancelTalk()
    {
        dialogueOptionView.SetActive(false);
        EndDialog();
    }

    /// <summary>
    /// lanjutan dari continuetalk()
    /// untuk mengganti dialog
    /// </summary>
    public void NextDialogue()
    {
        if (dialNum == curDialogue.dialogue.Length-1)
        {
            EndDialog();
            CurrentQuestUI.instance.Refresh();
        }
        else
        {
            dialNum++;
            conversationText.text = curDialogue.dialogue[dialNum];
        }
    }

    /// <summary>
    /// function ketika dialogue sudah habis
    /// </summary>
    public void EndDialog()
    {
        ClearList();
        UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIState.Gameplay));
    }

    /// <summary>
    /// function untuk mendestroy option
    /// </summary>
    void ClearList()
    {
        dialNum = 0;
        curDialogue = null;
        DestroyOption();
    }

    /// <summary>
    /// lanjutan dari clear list
    /// untuk menghapus option yang ada pada ui ketika selesai berdialogue
    /// </summary>
    void DestroyOption()
    {
        for (int i = 0; i < dialogueOptionContent.transform.childCount; i++)
            Destroy(dialogueOptionContent.transform.GetChild(i).gameObject);
    }
}
