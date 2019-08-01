using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteraction : MonoBehaviour
{
    public static CharacterInteraction instance;

    /// <summary>
    /// dipanggil untuk gathering pada saat interacting lewat interactableindicator
    /// dipanggil untuk direset juga animatornya
    /// </summary>
    public Animator animator;

    //target interactable hasil dari raycast, dipanggil di interactableindicator
    public Interactable tempInteractable; 
    //jika dia collectable, dipanggil di interactable indicator
    public Interactable gatherTarget; 
    //untuk hide interactbutton, bisa interact jika false, dipanggil di interactable indicator
    public bool hideInteractButton;
    //kondisi jika buttonnya sudah di show
    public bool buttonShowed;
    //yang mengatur adalah ui manager
    public bool isRaycasting; 

    /// <summary>
    /// kondisi jika sedang dalam kondisi gathering
    /// pada saat sedang gathering, characterinput tidak bisa rotate
    /// </summary>
    public bool isGathering;

    [Header("RayCast Settings")]
    //max jarak raycastnya
    public float maxRayDistance;
    //offset start saat raycasting
    public Vector3 interactRaycastOffset; 

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        //kondisi awal
        buttonShowed = false;
        isRaycasting = true;
        hideInteractButton = true;
        isGathering = false;
        HideButton();
    }

    private void Update()
    {
        //jika sedang dalam mode gameplay, dan tidak dalam kondisi combat
        if (UIManager.instance.uiState == UIState.Gameplay && !CharacterInput.instance.combatMode)
        {
            //jika tidak sedang select item (karena ada button yang sama ketika select item dan menggunakan item)
            //karena select item pencet x (xbox) dan menggunakan item ketika sedang raycast juga mencet x (xbox)
            //raycasting juga hanya bisa dilakukan ketika israycasting true
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

    /// <summary>
    /// function raycasting dari script interaction
    /// </summary>
    void InteractionRayCasting()
    {
        //start raycasting
        Ray ray = new Ray(transform.position + interactRaycastOffset, transform.forward + interactRaycastOffset);
        //agar muncul pada gizmos
        Debug.DrawLine(transform.position + interactRaycastOffset, transform.position + interactRaycastOffset + transform.forward * maxRayDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            CheckInteract(hit);
        }
        else
        {
            //jika raycast tidak menyentuh apa2, maka interact button sudah pasti true
            hideInteractButton = true; 
        }
    }

    /// <summary>
    /// function ini dijalankan ketika raycast dari interaction sudah terkena object
    /// untuk mengecek apakah object tersebut bisa di interact atau tidak
    /// </summary>
    /// <param name="hit"></param>
    public void CheckInteract(RaycastHit hit)
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
            //Debug.Log("target tidak bisa di interact");
        }
    }

    /// <summary>
    /// function berbicara dengan npc
    /// </summary>
    /// <param name="interactable"></param>
    public void TalkToObject(Interactable interactable)
    {
        hideInteractButton = true;
        NPC thisNPC = interactable.gameObject.GetComponent<NPC>();

        //total dialogue option untuk menentukan ada/tidak dan banyaknya opsi ketika berbicara
        int totalDialogueOption = thisNPC.activeCollectionQuest.Count;

        if (thisNPC.isAShop == true)
            totalDialogueOption += 1;

        if (totalDialogueOption != 0)
        {
            Conversation.instance.StartNewDialogue(thisNPC, thisNPC.activeCollectionQuest, thisNPC.npcDialog, thisNPC.optionDialog, true);
        }
        //jika tidak ada opsi dialog
        else
        {
            Conversation.instance.StartNewDialogue(thisNPC, null, thisNPC.npcDialog, null, false);
        }
    }

    /// <summary>
    /// function dipanggil dari animator character, disesuaikan pada titik animasi gathering selesai
    /// </summary>
    public void CollectObject()
    {
        //mengecek id dari item tersebut
        Item item = gatherTarget.items[0];
        bool GatherItem = PlayerData.instance.AddItem(item);

        if (!GatherItem)
        {
            return;
        }

        //gather target item diremove (karena bisa lebih dari 1 item setiap gather)
        gatherTarget.items.RemoveAt(0);
        //gameobject item yang ada di hierarchy dihancurkan
        if (gatherTarget.items.Count == 0)
            Destroy(gatherTarget.gameObject);
    }

    /// <summary>
    /// function untuk menunjukkan jika object tersebut dapat di interact
    /// </summary>
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

    /// <summary>
    /// function untuk menunjukkan jika object tersebut tidak dapat di interact
    /// </summary>
    public void HideButton()
    {
        if (buttonShowed == true)
        {
            InteractableIndicator.instance.interactText.gameObject.SetActive(false);
            InteractableIndicator.instance.interactButton.gameObject.SetActive(false);
            buttonShowed = false;
        }
    }

    /// <summary>
    /// kondisi gathering
    /// </summary>
    public IEnumerator Gather()
    {
        isGathering = true;
        isRaycasting = false;
        hideInteractButton = true;
        yield return new WaitForSeconds(1.5f);
        isGathering = false;
        isRaycasting = true;
        hideInteractButton = false;
        animator.ResetTrigger("gather");
    }

}
