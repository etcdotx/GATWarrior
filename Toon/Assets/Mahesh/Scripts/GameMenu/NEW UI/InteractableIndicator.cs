using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractableIndicator : MonoBehaviour
{ 
    public static InteractableIndicator instance;

    public Button interactButton;
    public Text interactText;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void Interact()
    {
        try
        {
            Interactable tempInteractable = CharacterInteraction.instance.tempInteractable;
            if (!CharacterInteraction.instance.hideInteractButton)
            {
                if (tempInteractable.isTalking)
                {
                    CharacterInteraction.instance.TalkToObject(tempInteractable);

                    StartCoroutine(UIManager.instance.ChangeState(UIManager.UIState.Conversation));
                    UIManager.instance.ExitGamePlayState();
                    UIManager.instance.StartConversationState();
                }
                else if (tempInteractable.isCollectable)
                {
                    CharacterInteraction.instance.gatherTarget = tempInteractable;
                    CharacterInteraction.instance.animator.SetTrigger("gather");
                    StartCoroutine(CharacterInteraction.instance.Gather());
                }
                else if (tempInteractable.isItemBox)
                {
                    GameMenuManager.instance.OpenInventoryBoxMenu();
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);
                }
            }
        } catch { }
    }
}
