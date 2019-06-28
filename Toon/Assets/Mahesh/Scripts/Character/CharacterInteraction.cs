using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteraction : MonoBehaviour
{
    public static CharacterInteraction instance;

    public Animator animator;
    public Talk talk;
    public Collect collect;
    public Interactable tempInteractable;

    public bool hideInteractButton;

    [Header("RayCast Settings")]
    public bool isRaycasting;
    public float maxRayDistance;
    public Vector3 interactRaycastOffset;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        animator = GetComponent<Animator>();
        talk = gameObject.GetComponent<Talk>();
        collect = gameObject.GetComponent<Collect>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isRaycasting = false;
        hideInteractButton = true;
        HideButton();
    }

    private void Update()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Gameplay)
        {
            if (!UsableItem.instance.isSelectingItem)
            {
                InteractionRayCasting();
            }

            if (isRaycasting)
            {
                if (hideInteractButton)
                    HideButton();
                else
                    ShowButton();
            }
        }
    }

    void InteractionRayCasting()
    {
        Ray ray = new Ray(transform.position + interactRaycastOffset, transform.forward + interactRaycastOffset);
        Debug.DrawLine(transform.position + interactRaycastOffset, transform.position + interactRaycastOffset + transform.forward * maxRayDistance, Color.red);
        RaycastHit hit;
        isRaycasting = true;

        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            Interact(hit);
        }
        else
        {
            hideInteractButton = true;
        }
    }

    public void Interact(RaycastHit hit)
    {
        try
        {
            tempInteractable = hit.collider.gameObject.GetComponent<Interactable>();
            if (tempInteractable.isInteractable)
            {
                InteractableIndicator.instance.interactText.text = tempInteractable.interactText;
                hideInteractButton = false;
            }
        }
        catch
        {
            hideInteractButton = true;
        }
    }

    public void TalkToObject(Interactable interactable)
    {
        HideButton();
        NPC thisNPC = interactable.gameObject.GetComponent<NPC>();
        int totalDialogueOption = thisNPC.activeCollectionQuest.Count;
        if (thisNPC.isAShop == true)
            totalDialogueOption += 1;

        if (totalDialogueOption != 0)
        {
            Conversation.instance.StartNewDialogue(thisNPC, thisNPC.activeCollectionQuest, thisNPC.npcDialog, thisNPC.optionDialog, true);
        }
        else
        {
            Conversation.instance.StartNewDialogue(thisNPC, null, thisNPC.npcDialog, null, false);
        }
    }

    public void ShowButton()
    {
        InteractableIndicator.instance.interactText.gameObject.SetActive(true);
        InteractableIndicator.instance.interactButton.gameObject.SetActive(true);
        UIManager.instance.eventSystem.SetSelectedGameObject(InteractableIndicator.instance.interactButton.gameObject);
    }

    public void HideButton()
    {
        InteractableIndicator.instance.interactText.gameObject.SetActive(false);
        InteractableIndicator.instance.interactButton.gameObject.SetActive(false);
    }
}
