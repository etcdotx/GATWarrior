using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueOption : MonoBehaviour, ISelectHandler, ICancelHandler
{
    public enum DialogueType
    {
        Shop,Talk,Quest
    }

    //quest yang ada di dialogue
    public CollectionQuest collectionQuest;
    //jenis dialoguenya
    public DialogueType dialogueType;
    //indicator quest kalau questnya belum selesai
    public GameObject questIndicatorNew;
    //indicator quest kalau questnya sudah selesai
    public GameObject questIndicatorComplete;
    //judul yang ada pada dialogue
    public TextMeshProUGUI optionText;

    //urutan keberapa pada hierarchy
    float siblingIndex;
    //ada berapa sibling dari parentnya
    public float totalSibling;

    private void Start()
    {
        totalSibling = transform.parent.childCount;
        siblingIndex = transform.GetSiblingIndex();
    }

    //kalau tidak jadi memilih option
    public void OnCancel(BaseEventData eventData)
    {
        Conversation.instance.CancelTalk();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);

        //set the scrollbar position by selected item
        if (siblingIndex == 1)
            siblingIndex = 0;

        Conversation.instance.dialogueOptionScrollbar.value = 1.0f - (siblingIndex / totalSibling);

        if (siblingIndex == transform.parent.childCount - 1)
            Conversation.instance.dialogueOptionScrollbar.value = 0f;
        else if (siblingIndex == 0)
            Conversation.instance.dialogueOptionScrollbar.value = 1.0f;
    }

    /// <summary>
    /// ada pada button
    /// function jika opsi dipilih
    /// </summary>
    public void SelectDialogue()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        Conversation.instance.conversationButton.gameObject.SetActive(true);
        Conversation.instance.conversationButton.interactable = true;
        UIManager.instance.eventSystem.SetSelectedGameObject(Conversation.instance.conversationButton.gameObject);

        if (dialogueType == DialogueType.Talk)
            Conversation.instance.SelectTalkOption();
        else if (dialogueType == DialogueType.Shop)
            Conversation.instance.SelectShopOption();
        else if (dialogueType == DialogueType.Quest)
            Conversation.instance.SelectQuestOption(collectionQuest);
    }
}
