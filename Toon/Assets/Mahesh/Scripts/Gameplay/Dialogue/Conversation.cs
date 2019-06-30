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
    public Dialogue npcDialog;
    public int dialNum;

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

    public void ContinueTalking()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        NextDialogue();
        if (dialogue != null)
        {
            if (dialNum == dialogue.dialogue.Length - 1)
                conversationButtonText.text = "End";
            else if (dialNum < dialogue.dialogue.Length - 1)
                conversationButtonText.text = "Continue";
        }
    }

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
            dialogue = this.npcDialog;
            conversationText.text = npcDialog.dialogue[dialNum];
        }
    }

    public void SelectTalkOption() {
        dialogue = this.npcDialog;
        conversationText.text = dialogue.dialogue[dialNum];
        StartSelectedDialogue();
    }

    public void SelectShopOption()
    {
        CancelTalk();
        Shop.instance.OpenShop(target);
    }
    public void SelectQuestOption(CollectionQuest dialogueQuest) {
        CheckQuest(dialogueQuest);
        StartSelectedDialogue();
    }

    void StartSelectedDialogue()
    {
        dialogueOptionView.SetActive(false);

        if (dialNum == dialogue.dialogue.Length - 1)
            conversationButtonText.text = "End";
        else if (dialNum < dialogue.dialogue.Length - 1)
            conversationButtonText.text = "Continue";
    }

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


                    RemoveActiveQuest(dialogueQuest);
                    RemoveNPCQuest(dialogueQuest);

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

    void RemoveActiveQuest(CollectionQuest dialogueQuest)
    {
        for (int i = 0; i < Quest.instance.collectionQuestActive.Count; i++)
        {
            if (Quest.instance.collectionQuestActive[i].id == dialogueQuest.id)
            {
                Quest.instance.collectionQuestActive.RemoveAt(i);
                break;
            }
        }
    }

    void RemoveNPCQuest(CollectionQuest dialogueQuest) {
        for (int i = 0; i < target.activeCollectionQuest.Count; i++)
        {
            if (target.activeCollectionQuest[i].id == dialogueQuest.id)
            {
                target.activeCollectionQuest.RemoveAt(i);
                break;
            }
        }
    }

    void SetQuestDialogue(CollectionQuest dialogueQuest)
    {
        dialogueOptionView.SetActive(false);
        dialogue = null;
        dialogue = dialogueQuest.startDialogue;
        conversationText.text = dialogue.dialogue[dialNum];
    }

    void SetQuestCompleteDialogue(CollectionQuest dialogueQuest)
    {
        dialogueOptionView.SetActive(false);
        dialogue = null;
        dialogue = dialogueQuest.endDialogue;
        conversationText.text = dialogue.dialogue[dialNum];
    }

    void DestroyOption()
    {
        for (int i = 0; i < dialogueOptionContent.transform.childCount; i++)
            Destroy(dialogueOptionContent.transform.GetChild(i).gameObject);
    }

    void InstantiateDialogueOption(List<CollectionQuest> cqList)
    {
        //for normal dialogue
        DialogueOption talkDialogueOption = Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform).GetComponent<DialogueOption>();
        talkDialogueOption.optionText.text = "Talk";
        talkDialogueOption.isTalk = true;
        UIManager.instance.eventSystem.SetSelectedGameObject(talkDialogueOption.gameObject);

        //for shop
        if (target.isAShop)
        {
            DialogueOption shopDialogueOption = Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform).GetComponent<DialogueOption>();
            shopDialogueOption.optionText.text = "Buy";
            shopDialogueOption.isShop = true;
        }

        //for questdialogue
        for (int i = 0; i < cqList.Count; i++)
        {
            DialogueOption newDialogueOption = Instantiate(dialogueOptionPrefab, dialogueOptionContent.transform).GetComponent<DialogueOption>();
            newDialogueOption.collectionQuest = cqList[i];
            newDialogueOption.optionText.text = cqList[i].title;
            newDialogueOption.isQuest = true;
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

    void ShowUI(bool haveOption)
    {
        conversationText.gameObject.SetActive(true);
        conversationButton.gameObject.SetActive(true);
        conversationButtonText.gameObject.SetActive(true);

        if (haveOption == true)
            dialogueOptionView.SetActive(true);
    }

    public void CancelTalk()
    {
        dialogueOptionView.SetActive(false);
        EndDialog();
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
        DestroyOption();
        ClearList();
        UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIManager.UIState.Gameplay));
    }

    void ClearList()
    {
        dialNum = 0;
        dialogue = null;
        DestroyOption();
    }
}
