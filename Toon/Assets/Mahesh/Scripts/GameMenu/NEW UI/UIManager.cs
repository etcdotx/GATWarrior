using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public UIState uiState;
    public EventSystem eventSystem;
    public GameObject inventoryFirstSelect;

    public enum UIState {
        Gameplay ,InventoryAndSave, InventoryAndInventoryBox, Shop, Conversation
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
    }

    void GameplayState()
    {
        if (Input.GetKeyDown(InputSetup.instance.start))
        {
            ExitGamePlayState();
            StartCoroutine(ChangeState(UIState.InventoryAndSave));

            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);

            Inventory.instance.inventoryView.SetActive(true);
            Quest.instance.questView.SetActive(true);
            UsableItem.instance.usableItemView.SetActive(false);

            if (Quest.instance.questContent.transform.childCount > 0)
            {
                eventSystem.SetSelectedGameObject(Quest.instance.questContent.transform.GetChild(0).gameObject);
                Debug.Log(Quest.instance.questContent.transform.GetChild(0).gameObject.name);
            }

            eventSystem.SetSelectedGameObject(inventoryFirstSelect);
        }
    }

    void InventoryAndSaveState()
    {
        if (Input.GetKeyDown(InputSetup.instance.start))
        {
            StartGamePlayState();
            StartCoroutine(ChangeState(UIState.Gameplay));

            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);

            Inventory.instance.inventoryView.SetActive(false);
            Quest.instance.questView.SetActive(false);
            UsableItem.instance.usableItemView.SetActive(true);
        }
    }

    public void StartGamePlayState() {
        CharacterInteraction.instance.isRaycasting = true;
    }

    public void ExitGamePlayState() {
        CharacterInteraction.instance.isRaycasting = false;
    }

    void ConversationState() {
        
    }

    public void StartConversationState()
    {
        UsableItem.instance.usableItemView.SetActive(false);

    }

    public void ExitConversationState()
    {
        UsableItem.instance.usableItemView.SetActive(true);
    }

    public IEnumerator ChangeState(UIState nextState) {
        yield return new WaitForSeconds(0.2f);
        uiState = nextState;
    }
}
