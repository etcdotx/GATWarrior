using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public EventSystem eventSystem;
    public UIState uiState;

    public enum UIState {
        Gameplay, InventoryAndSave, InventoryAndInventoryBox, Shop, Conversation
    }

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

    #region START GAME
    //GAMEPLAY STATE
    void StartGamePlayState()
    {
        CharacterInteraction.instance.isRaycasting = true;

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
    }

    void StartInventoryAndSaveState()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);

        Inventory.instance.inventoryView.SetActive(true);
        Quest.instance.questView.SetActive(true);
        UsableItem.instance.usableItemView.SetActive(false);

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

        UsableItem.instance.usableItemView.SetActive(true);
        PlayerStatus.instance.healthIndicator.SetActive(true);
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

        PlayerStatus.instance.healthIndicator.SetActive(true);
        UsableItem.instance.usableItemView.SetActive(true);
        Inventory.instance.inventoryView.SetActive(false);
        InventoryBox.instance.inventoryBoxView.SetActive(false);
    }
    #endregion

    public void ShopState() {

    }

    public void StartShopState() {
        PlayerStatus.instance.healthIndicator.SetActive(false);
        UsableItem.instance.usableItemView.SetActive(false);
        eventSystem.SetSelectedGameObject(Shop.instance.shopContent.transform.GetChild(0).gameObject);
    }

    public void ExitShopState() {
        PlayerStatus.instance.healthIndicator.SetActive(true);
        UsableItem.instance.usableItemView.SetActive(true);
        Shop.instance.shopView.SetActive(false);
        Shop.instance.RemoveShopIndicator();
        InventoryBox.instance.inventoryBoxView.SetActive(false);
    }

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
        }
    }

    void StopMovement()
    {
        CharacterInteraction.instance.animator.SetFloat("floatX", 0);
        CharacterInteraction.instance.animator.SetFloat("floatY", 0);
    }
}
