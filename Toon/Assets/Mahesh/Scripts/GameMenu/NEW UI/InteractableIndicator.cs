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

    /// <summary>
    /// dipanggil dari button gameobject ketika suatu object memeliki script interactable ...
    /// ... dan terkena raycast dari player
    /// </summary>
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
                    UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIState.Conversation));
                }
                else if (tempInteractable.isCollectable)
                {
                    CharacterInteraction.instance.gatherTarget = tempInteractable;
                    CharacterInteraction.instance.animator.SetTrigger("gather");
                    UIManager.instance.StartCoroutine(CharacterInteraction.instance.Gather());
                }
                else if (tempInteractable.isItemBox)
                {
                    UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIState.InventoryAndInventoryBox));
                }
            }
        } catch { }
    }
}
