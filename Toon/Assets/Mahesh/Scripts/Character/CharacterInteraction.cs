using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteraction : MonoBehaviour
{
    public static CharacterInteraction instance;

    public Animator animator;
    public Interactable tempInteractable;
    public Interactable gatherTarget;

    public bool hideInteractButton;
    public bool buttonShowed;
    public bool isRaycasting;
    public bool isGathering;

    [Header("RayCast Settings")]
    public float maxRayDistance;
    public Vector3 interactRaycastOffset;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonShowed = false;
        isRaycasting = true;
        hideInteractButton = true;
        isGathering = false;
        HideButton();
    }

    private void Update()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Gameplay && !CharacterInput.instance.combatMode)
        {
            if (!UsableItem.instance.isSelectingItem && isRaycasting)
            {
                InteractionRayCasting();
            }

            if (hideInteractButton)
                HideButton();
            else
                ShowButton();
        }
        else {
            HideButton();
        }
    }

    void InteractionRayCasting()
    {
        Ray ray = new Ray(transform.position + interactRaycastOffset, transform.forward + interactRaycastOffset);
        Debug.DrawLine(transform.position + interactRaycastOffset, transform.position + interactRaycastOffset + transform.forward * maxRayDistance, Color.red);
        RaycastHit hit;

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
        }
    }

    public void TalkToObject(Interactable interactable)
    {
        hideInteractButton = true;
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
    public void CollectObject()
    {
        //mengecek id dari item tersebut
        Item item = gatherTarget.items[0];
        bool itemExist = false;

        for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
        {
            if (PlayerData.instance.inventoryItem[i].id == item.id)
                if (PlayerData.instance.inventoryItem[i].quantity == PlayerData.instance.inventoryItem[i].maxQuantityOnInventory)
                {
                    Debug.Log("you cannot carry more " + item.itemName);
                    return;
                }
                else
                {
                    for (int j = 0; j < ItemDataBase.item.Count; j++)
                    {
                        //jika item yang ada didatabase sesuai dengan item yang diinteract
                        //maka item tersebut dimasukkan kedalam koleksi item player
                        if (ItemDataBase.item[j].id == item.id)
                        {
                            gatherTarget.items.RemoveAt(0);
                            PlayerData.instance.AddItem(ItemDataBase.item[j]);
                            itemExist = true;
                            break;
                        }
                    }
                    break;
                }
        }

        if (!itemExist)
        {
            bool thereIsSpace = false;
            for (int i = 0; i < Inventory.instance.inventoryIndicator.Length; i++)
            {
                if (Inventory.instance.inventoryIndicator[i].item == null)
                {
                    thereIsSpace = true;
                    break;
                }
            }
            if (!thereIsSpace)
            {
                Debug.Log("item space is full");
                return;
            }

            for (int j = 0; j < ItemDataBase.item.Count; j++)
            {
                //jika item yang ada didatabase sesuai dengan item yang diinteract
                //maka item tersebut dimasukkan kedalam koleksi item player
                if (ItemDataBase.item[j].id == item.id)
                {
                    gatherTarget.items.RemoveAt(0);
                    PlayerData.instance.AddItem(ItemDataBase.item[j]);
                    break;
                }
            }
        }

        //gameobject item yang ada di hierarchy dihancurkan
        if (gatherTarget.items.Count == 0)
            Destroy(gatherTarget.gameObject);
    }

    public void ShowButton()
    {
        if (buttonShowed == false)
        {
            InteractableIndicator.instance.interactText.gameObject.SetActive(true);
            InteractableIndicator.instance.interactButton.gameObject.SetActive(true);
            UIManager.instance.eventSystem.SetSelectedGameObject(InteractableIndicator.instance.interactButton.gameObject);
            buttonShowed = true;
        }
    }

    public void HideButton()
    {
        if (buttonShowed == true)
        {
            InteractableIndicator.instance.interactText.gameObject.SetActive(false);
            InteractableIndicator.instance.interactButton.gameObject.SetActive(false);
            buttonShowed = false;
        }
    }

    public IEnumerator Gather()
    {
        isGathering = true;
        yield return new WaitForSeconds(1.5f);
        isGathering = false;
        animator.ResetTrigger("gather");
    }
}
