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
        if (Input.GetKeyDown(InputSetup.instance.openGameMenu))
        {
            Inventory.instance.inventoryView.SetActive(true);
            Quest.instance.questView.SetActive(true);
            UsableItem.instance.usableItemView.SetActive(false);
            StartCoroutine(ChangeState(UIState.InventoryAndSave));
            eventSystem.SetSelectedGameObject(inventoryFirstSelect);
            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);
        }
    }

    void InventoryAndSaveState()
    {
        if (Input.GetKeyDown(InputSetup.instance.openGameMenu))
        {
            Inventory.instance.inventoryView.SetActive(false);
            Quest.instance.questView.SetActive(false);
            UsableItem.instance.usableItemView.SetActive(true);
            StartCoroutine(ChangeState(UIState.Gameplay));
            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);
        }
    }

    void ConversationState() {
        
    }

    public IEnumerator ChangeState(UIState nextState) {
        yield return new WaitForSeconds(0.2f);
        uiState = nextState;
    }
}
