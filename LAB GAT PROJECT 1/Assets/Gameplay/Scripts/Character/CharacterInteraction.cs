using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteraction : MonoBehaviour
{
    public PlayerData playerData;
    public Talk talk;
    public Collect collect;
    public GameMenuManager gameMenuManager;
    public GameObject interactButton;
    public Text interactText;

    public float maxRayDistance;
    public Vector3 interactRaycastOffset;

    public InputSetup inputSetup;
    public Dialogue dialogue;
    public InventoryBox inventoryBox;
    public Inventory inventory;

    public bool OtherHideButton;
    public bool InteractHideButton;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        talk = gameObject.GetComponent<Talk>();
        collect = gameObject.GetComponent<Collect>();
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        gameMenuManager = GameObject.FindGameObjectWithTag("GameMenuManager").GetComponent<GameMenuManager>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        dialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Dialogue>();

        interactButton = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("InteractButton").gameObject;
        interactText = GameObject.FindGameObjectWithTag("InteractableUI").transform.Find("InteractText").GetComponent<Text>();
        OtherHideButton = true;
        InteractHideButton = true;
        HideButton();
    }

    private void Update()
    {
        if (GameStatus.IsPaused == false && GameStatus.isTalking == false && InputHolder.isInputHolded==false)
        {
            InteractionRayCasting();
        }
        if (InteractHideButton == false || OtherHideButton == false)
        {
            ShowButton();
        }
        else if (InteractHideButton == true && OtherHideButton == true)
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
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
                //mengecek apakah object tersebut interactable
                if (interactable.isInteractable == true)
                {
                    //menunjukkan ui interact
                    interactText.text = interactable.interactText;
                    InteractHideButton = false;
                    //jika object tersebut bisa berbicara
                    if (Input.GetKeyDown(inputSetup.interact) && interactable.isTalking == true)
                    {
                        GameStatus.isTalking = true;
                        talk.TalkToObject(interactable);
                    }
                    //jika object tersebut bisa dimasukkan kedalam koleksi
                    if (Input.GetKeyDown(inputSetup.interact) && interactable.isCollectable == true)
                    {
                        //mengambil item tersebut
                        collect.CollectObject(interactable);
                    }
                    //jika object tersebut adalah inventorybox
                    if (Input.GetKeyDown(inputSetup.interact) && interactable.gameObject.tag == "GameObject_InventoryBox" && inventoryBox.isItemBoxOpened == false)
                    {
                        inventory.inventoryView.SetActive(true);
                        inventoryBox.inventoryBoxView.SetActive(true);
                        inventoryBox.isItemBoxOpened = true;
                        gameMenuManager.StartCoroutine("ButtonInputHold");
                        gameMenuManager.isOpen = true;
                        gameMenuManager.ResetMenu();
                        GameStatus.PauseGame();
                        GameStatus.PauseMove();
                        HideButton();
                    }
                }
            }
            catch
            {
                InteractHideButton = true;
            }
        }
        else
        {
            InteractHideButton = true;
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
}
