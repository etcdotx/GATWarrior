using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Interactable tempInteractable = CharacterInteraction.instance.tempInteractable;
        if (!CharacterInteraction.instance.hideInteractButton)
        {
            if (tempInteractable.isTalking)
            {
                CharacterInteraction.instance.TalkToObject(tempInteractable);

                UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIManager.UIState.Conversation));
                UIManager.instance.ExitGamePlayState();
            }
            else if (tempInteractable.isCollectable)
            {
                CharacterInteraction.instance.collect.CollectObject(tempInteractable);
            }
            else if (tempInteractable.isItemBox)
            {
                GameMenuManager.instance.OpenInventoryBoxMenu();
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);
            }
        }
    }
}
