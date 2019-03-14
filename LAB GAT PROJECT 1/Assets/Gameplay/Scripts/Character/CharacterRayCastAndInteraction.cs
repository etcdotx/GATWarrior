using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRayCastAndInteraction : MonoBehaviour
{
    public Player player;
    public GameObject mainCamera;
    public GameObject interactButton;
    public Text interactText;

    public float maxRayDistance;
    public Vector3 raycastOffset;

    public InputSetup inputSetup;
    public ShowDialog showDialog;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        showDialog = GameObject.FindGameObjectWithTag("ShowDialog").GetComponent<ShowDialog>();

        interactButton = GameObject.FindGameObjectWithTag("Interactable").transform.Find("InteractButton").gameObject;
        interactText = GameObject.FindGameObjectWithTag("Interactable").transform.Find("InteractText").GetComponent<Text>();
        HideButton();
    }

    private void FixedUpdate()
    {
        if (GameStatus.IsPaused == false && GameStatus.isTalking == false)
        {
            Ray ray = new Ray(transform.position + raycastOffset, transform.forward + raycastOffset);
            RaycastHit hit;
            Debug.DrawLine(transform.position + raycastOffset, transform.position + raycastOffset + transform.forward * maxRayDistance, Color.red);

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                try
                {
                    Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
                    if (interactable.isInteractable == true)
                    {
                        interactText.text = interactable.interactText;
                        ShowButton();
                        if (Input.GetKeyDown(inputSetup.interact) && interactable.isTalking == true)
                        {
                            TalkToObject(interactable);
                            Debug.Log(hit.collider.gameObject.name + " is talking");
                        }
                        if (Input.GetKeyDown(inputSetup.interact) && interactable.isCollectable == true)
                        {
                            CollectObject(interactable);
                        }
                    }
                }
                catch
                {
                    Debug.Log(hit.collider.gameObject.name + " is not interactable");
                }
            }
            else
            {
               HideButton();
            }
        }

    }

    void TalkToObject(Interactable interactable)
    {
        bool isHavingQuest = false;
        GameStatus.isTalking = true;
        mainCamera.transform.position = interactable.interactView.transform.position;
        mainCamera.transform.rotation = interactable.interactView.transform.rotation;
        HideButton();
        try
        {
            NPC interactedNPC = interactable.gameObject.GetComponent<NPC>();
            isHavingQuest = interactedNPC.isHavingQuest;
            if (isHavingQuest == true)
            {
                showDialog.StartQuestDialog(interactedNPC.questDialogActive, interactedNPC.questIDActive);
                interactedNPC.RefreshQuest();
                //interactedNPC.isHavingQuest = false;
            }
        }
        catch { }
        if (isHavingQuest == false)
        {
            showDialog.StartDialog(interactable.dialog);
        }
    }

    void CollectObject(Interactable interactable) {
        int itemID = interactable.gameObject.GetComponent<Interactable>().itemID;
        if (ItemDataBase.item == null)
            ItemDataBase.item = new List<Item>();

        for (int i = 0; i < ItemDataBase.item.Count; i++)
        {
            if (ItemDataBase.item[i].id == itemID)
            {
                player.AddItem(ItemDataBase.item[i]);
                break;
            }
        }
        Destroy(interactable.gameObject);
    }

    void ShowButton()
    {
        interactText.gameObject.SetActive(true);
        interactButton.gameObject.SetActive(true);
    }

    void HideButton()
    {
        interactText.gameObject.SetActive(false);
        interactButton.gameObject.SetActive(false);

    }
}
