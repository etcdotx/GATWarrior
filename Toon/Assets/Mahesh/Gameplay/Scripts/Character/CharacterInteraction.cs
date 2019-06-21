using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteraction : MonoBehaviour
{
    public PlayerData playerData;
    public Animator animator;
    public CharacterMovement cm;
    public UsableItem usableItem;
    public SoundList soundList;

    public Talk talk;
    public Conversation conversation;
    public Collect collect;
    public GameMenuManager gameMenuManager;
    public GameObject interactButton;
    public Text interactText;

    public float maxRayDistance;
    public Vector3 interactRaycastOffset;

    public InputSetup inputSetup;
    public InventoryBox inventoryBox;
    public Inventory inventory;
    
    public bool hideInteractButton;
    public bool buttonInputHold;

    private void Awake()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        cm = GetComponent<CharacterMovement>();
        usableItem = GameObject.FindGameObjectWithTag("UsableItem").GetComponent<UsableItem>();
        animator = GetComponent<Animator>();
        talk = gameObject.GetComponent<Talk>();
        collect = gameObject.GetComponent<Collect>();
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        gameMenuManager = GameObject.FindGameObjectWithTag("GameMenuManager").GetComponent<GameMenuManager>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        conversation = GameObject.FindGameObjectWithTag("Conversation").GetComponent<Conversation>();
        soundList = GameObject.FindGameObjectWithTag("SoundList").GetComponent<SoundList>();

        interactButton = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("InteractButton").gameObject;
        interactText = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("InteractText").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hideInteractButton = true;
        HideButton();
    }

    private void Update()
    {
        if (!GameStatus.IsPaused && !conversation.isTalking && !InputHolder.isInputHolded
            && gameMenuManager.menuState == GameMenuManager.MenuState.noMenu && !usableItem.isSelectingItem)
        {
            if(!buttonInputHold)
                InteractionRayCasting();
        }
        else
        {
            hideInteractButton = true;
        }

        if (!hideInteractButton)
        {
            ShowButton();
        }
        else if (hideInteractButton && !conversation.isTalking)
        {
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
            try
            {
                //Debug.Log(hit.collider.gameObject);
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
                //mengecek apakah object tersebut interactable
                if (interactable.isInteractable)
                {
                    //menunjukkan ui interact
                    interactText.text = interactable.interactText;
                    hideInteractButton = false;
                    //jika object tersebut bisa berbicara
                    if (Input.GetKeyDown(inputSetup.interact) && interactable.isTalking)
                    {
                        StartCoroutine(ButtonInputHold());
                        cm.canMove = false;
                        animator.SetBool("isWalk", false);
                        talk.TalkToObject(interactable);
                    }
                    //jika object tersebut bisa dimasukkan kedalam koleksi
                    else if (Input.GetKeyDown(inputSetup.interact) && interactable.isCollectable)
                    {
                        StartCoroutine(ButtonInputHold());
                        //mengambil item tersebut
                        collect.CollectObject(interactable);
                    }
                    //jika object tersebut adalah inventorybox
                    else if (Input.GetKeyDown(inputSetup.interact) && interactable.gameObject.tag == "GameObject_InventoryBox" 
                        && gameMenuManager.menuState == GameMenuManager.MenuState.noMenu)
                    {
                        StartCoroutine(ButtonInputHold());
                        animator.SetBool("isWalk", false);
                        gameMenuManager.OpenInventoryBoxMenu();
                        soundList.UIAudioSource.PlayOneShot(soundList.OpenInventoryClip);
                    }
                }
            }
            catch
            {
                //Debug.Log(hit.collider.gameObject + "not interactable");
                hideInteractButton = true;
            }
        }
        else
        {
            hideInteractButton = true;
        }
    }

    public void ShowButton()
    {
        interactText.gameObject.SetActive(true);
        interactButton.gameObject.SetActive(true);
    }

    public void HideButton()
    {
        interactText.gameObject.SetActive(false);
        interactButton.gameObject.SetActive(false);
    }

    public IEnumerator ButtonInputHold()
    {
        buttonInputHold = true;
        yield return new WaitForSeconds(0.15f);
        buttonInputHold = false;
    }
}
