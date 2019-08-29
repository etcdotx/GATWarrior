using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum UIState
{
    Gameplay, InventoryAndSave, InventoryAndInventoryBox, Shop, Conversation, Timeline
}

public enum WarningState
{
    itemFull, notEnoughMoney, inventoryFull
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public EventSystem eventSystem;
    public UIState uiState;

    [Header("Warning Notification")]
    public TextMeshProUGUI warning;


    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        eventSystem = GetComponent<EventSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiState = UIState.Gameplay;
        warning.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (uiState == UIState.Gameplay)
        {
            GameplayState();
        }
        else if (uiState == UIState.InventoryAndSave)
        {
            InventoryAndSaveState();
        }
        else if (uiState == UIState.Conversation)
        {
            ConversationState();
        }
        else if (uiState == UIState.InventoryAndInventoryBox)
        {
            InventoryAndInventoryBoxState();
        }
        else if (uiState == UIState.Shop)
        {
            ShopState();
        }
    }

    /// <summary>
    /// kondisi yang berhubungan dengan
    /// 1. ketika sedang state tersebut
    /// 2. ketika baru masuk state tersebut
    /// 3. ketika keluar dari state tersebut
    /// </summary>

    #region Gameplay
    //GAMEPLAY STATE
    void StartGamePlayState()
    {
        CharacterInteraction.instance.isRaycasting = true;
        PlayerStatus.instance.healthIndicator.SetActive(true);
        UsableItem.instance.usableItemView.SetActive(true);
        CurrentQuestUI.instance.currentQuestPanel.gameObject.SetActive(true);
    }

    void ExitGamePlayState()
    {
        CharacterInteraction.instance.isRaycasting = false;
    }

    void GameplayState()
    {
        if (Input.GetKeyDown(InputSetup.instance.start))
        {
            StartCoroutine(ChangeState(UIState.InventoryAndSave));
        }
    }

    #endregion

    #region Inventory and Save
    // INVENTORY AND SAVE
    void InventoryAndSaveState()
    {
        if (Input.GetKeyDown(InputSetup.instance.start))
        {
            StartCoroutine(ChangeState(UIState.Gameplay));
        }
    }

    void ExitInventoryAndSaveState()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);

        Inventory.instance.inventoryView.SetActive(false);
        Quest.instance.questView.SetActive(false);
        UsableItem.instance.usableItemView.SetActive(true);
        CurrentQuestUI.instance.currentQuestPanel.gameObject.SetActive(true);
    }

    void StartInventoryAndSaveState()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);

        Inventory.instance.inventoryView.SetActive(true);
        Quest.instance.questView.SetActive(true);
        UsableItem.instance.usableItemView.SetActive(false);
        CurrentQuestUI.instance.currentQuestPanel.gameObject.SetActive(false);
        if (Quest.instance.questContent.transform.childCount > 0)
        {
            eventSystem.SetSelectedGameObject(Quest.instance.questContent.transform.GetChild(0).gameObject);
            Debug.Log(Quest.instance.questContent.transform.GetChild(0).gameObject.name);
        }
        eventSystem.SetSelectedGameObject(Inventory.instance.inventoryViewPort.transform.GetChild(0).gameObject);
    }

    #endregion

    #region Conversation
    //CONVERSATION
    void ConversationState() {
        
    }

    void StartConversationState()
    {
        UsableItem.instance.usableItemView.SetActive(false);
        CurrentQuestUI.instance.currentQuestPanel.gameObject.SetActive(false);
        PlayerStatus.instance.healthIndicator.SetActive(false);
        StopMovement();
    }

    void ExitConversationState()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);

        Conversation.instance.conversationButton.gameObject.SetActive(false);
        Conversation.instance.conversationButton.interactable = false;
        Conversation.instance.conversationButtonText.gameObject.SetActive(false);
        Conversation.instance.conversationText.gameObject.SetActive(false);
    }

    #endregion

    #region Inventory and Inventory Box
    //INVENTORY AND INVENTORY BOX
    void InventoryAndInventoryBoxState() {
    }

    void StartInventoryAndInventoryBoxState()
    {
        StopMovement();
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);

        PlayerStatus.instance.healthIndicator.SetActive(false);
        UsableItem.instance.usableItemView.SetActive(false);
        CurrentQuestUI.instance.currentQuestPanel.gameObject.SetActive(false);
        Inventory.instance.inventoryView.SetActive(true);
        InventoryBox.instance.inventoryBoxView.SetActive(true);
        eventSystem.SetSelectedGameObject(InventoryBox.instance.inventoryBoxViewPort.transform.GetChild(0).gameObject);
        eventSystem.SetSelectedGameObject(Inventory.instance.inventoryViewPort.transform.GetChild(0).gameObject);
    }

    void ExitInventoryAndInventoryBoxState()
    {
        Inventory.instance.temporaryItem = null;
        InventoryBox.instance.temporaryItem = null;

        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);
        Inventory.instance.inventoryView.SetActive(false);
        InventoryBox.instance.inventoryBoxView.SetActive(false);
    }
    #endregion

    #region shop
    public void ShopState() {

    }

    public void StartShopState() {
        PlayerStatus.instance.healthIndicator.SetActive(false);
        UsableItem.instance.usableItemView.SetActive(false);
        CurrentQuestUI.instance.currentQuestPanel.gameObject.SetActive(false);
        eventSystem.SetSelectedGameObject(Shop.instance.shopContent.transform.GetChild(0).gameObject);
    }

    public void ExitShopState() {
        Shop.instance.shopView.SetActive(false);
        Shop.instance.RemoveShopIndicator();
        InventoryBox.instance.inventoryBoxView.SetActive(false);
    }

    #endregion

    #region Timeline

    public void TimelineState() {

    }

    public void StartTimeLineState() {
        Shop.instance.shopView.SetActive(false);
        Inventory.instance.inventoryView.SetActive(false);
        InventoryBox.instance.inventoryBoxView.SetActive(false);
        UsableItem.instance.usableItemView.SetActive(false);
        CurrentQuestUI.instance.currentQuestPanel.gameObject.SetActive(false);
        Quest.instance.questView.SetActive(false);
        PlayerStatus.instance.healthIndicator.SetActive(false);
        CharacterInteraction.instance.isRaycasting = false;
    }

    public void ExitTimeLineState() { }

    #endregion

    /// <summary>
    /// function perpindahan state
    /// </summary>
    /// <param name="nextState">state selanjutnya</param>
    /// <returns></returns>
    public IEnumerator ChangeState(UIState nextState)
    {
        switch (uiState) {
            case UIState.Gameplay:
                ExitGamePlayState();
                break;
            case UIState.Conversation:
                ExitConversationState();
                break;
            case UIState.InventoryAndSave:
                ExitInventoryAndSaveState();
                break;
            case UIState.InventoryAndInventoryBox:
                ExitInventoryAndInventoryBoxState();
                break;
            case UIState.Shop:
                ExitShopState();
                break;
            case UIState.Timeline:
                ExitTimeLineState();
                break;
        }

        yield return new WaitForSeconds(0f);
        uiState = nextState;

        switch (uiState)
        {
            case UIState.Gameplay:
                StartGamePlayState();
                break;
            case UIState.Conversation:
                StartConversationState();
                break;
            case UIState.InventoryAndSave:
                StartInventoryAndSaveState();
                break;
            case UIState.InventoryAndInventoryBox:
                StartInventoryAndInventoryBoxState();
                break;
            case UIState.Shop:
                StartShopState();
                break;
            case UIState.Timeline:
                StartTimeLineState();
                break;
        }
    }

    /// <summary>
    /// function yang membuat karakter diam tergantung dengan state jika dibutuhkan
    /// </summary>
    void StopMovement()
    {
        CharacterInput.instance.animator.SetFloat("floatX", 0);
        CharacterInput.instance.animator.SetFloat("floatY", 0);
    }

    /// <summary>
    /// function untuk menunjukkan warning di game
    /// </summary>
    /// <param name="itemName">item yang berkaitan</param>
    /// <param name="warningState">kondisi warning</param>
    public void WarningNotification(string itemName, WarningState warningState) {
        StopCoroutine("ShowNotif");
        StartCoroutine(ShowNotif(itemName, warningState));
    }

    /// <summary>
    /// ienumerator lanjutan dari warningnotif
    /// akan automatis hilang tergantung durasi
    /// </summary>
    /// <param name="itemName">item yang berkaitan</param>
    /// <param name="warningState">kondisi warning</param>
    /// <returns></returns>
    IEnumerator ShowNotif(string itemName, WarningState warningState)
    {
        if (warningState == WarningState.itemFull)
        {
            warning.text = "You cannot carry more " + itemName + ".";
        }
        else if (warningState == WarningState.notEnoughMoney)
        {
            warning.text = "Not enough money to buy " + itemName + ".";
        }
        else if (warningState == WarningState.inventoryFull)
        {
            warning.text = "Inventory full.";
        }
        warning.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        warning.gameObject.SetActive(false);
    }
}
