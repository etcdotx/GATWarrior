using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueOption : MonoBehaviour, ISelectHandler, ICancelHandler
{
    public CollectionQuest collectionQuest;
    public bool isShop;
    public bool isTalk;
    public bool isQuest;
    public GameObject questIndicatorNew;
    public GameObject questIndicatorComplete;
    public TextMeshProUGUI optionText;

    public float itemNum;
    public float itemTotal;

    private void Start()
    {
        itemTotal = transform.parent.childCount;
        itemNum = transform.GetSiblingIndex();
    }

    public void OnCancel(BaseEventData eventData)
    {
        Conversation.instance.CancelTalk();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);

        //set the scrollbar position by selected item
        if (itemNum == 1)
            itemNum = 0;

        Conversation.instance.dialogueOptionScrollbar.value = 1.0f - (itemNum / itemTotal);

        if (itemNum == transform.parent.childCount - 1)
            Conversation.instance.dialogueOptionScrollbar.value = 0f;
        else if (itemNum == 0)
            Conversation.instance.dialogueOptionScrollbar.value = 1.0f;
    }

    public void SelectDialogue()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        Conversation.instance.conversationButton.gameObject.SetActive(true);
        Conversation.instance.conversationButton.interactable = true;
        UIManager.instance.eventSystem.SetSelectedGameObject(Conversation.instance.conversationButton.gameObject);

        if (isTalk)
            Conversation.instance.SelectTalkOption();
        else if (isShop)
            Conversation.instance.SelectShopOption();
        else if (isQuest)
            Conversation.instance.SelectQuestOption(collectionQuest);

    }
}
