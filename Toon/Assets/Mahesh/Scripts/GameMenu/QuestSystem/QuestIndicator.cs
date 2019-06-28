using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestIndicator : MonoBehaviour, ISelectHandler, ICancelHandler
{
    public Text questText;
    public int questID;
    public float itemNum;
    public float itemTotal;
    public Button button;
    public Navigation navigation;

    // Start is called before the first frame update
    void Start()
    {
        itemTotal = transform.parent.childCount;
        itemNum = transform.GetSiblingIndex();
        button = GetComponent<Button>();
        navigation = button.navigation;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //set the scrollbar position by selected item
        if (itemNum == 1)
            itemNum = 0;
        Quest.instance.questViewScrollbar.value = 1.0f - (itemNum / itemTotal);
        if (itemNum == transform.parent.childCount - 1)
            Quest.instance.questViewScrollbar.value = 0f;
        else if (itemNum == 0)
            Quest.instance.questViewScrollbar.value = 1.0f;

        Quest.instance.RefreshQuestDetail(questID);
        Debug.Log(this.gameObject.name + " was selected");
    }

    public void OnCancel(BaseEventData eventData)
    {
        Inventory.instance.inventoryView.SetActive(false);
        Quest.instance.questView.SetActive(false);
        UsableItem.instance.usableItemView.SetActive(true);
        UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIManager.UIState.Gameplay));
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);
    }

}
