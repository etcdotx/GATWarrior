using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRayCastAndInteraction : MonoBehaviour
{
    public Player player;
    public MenuManager menuManager;
    public GameObject mainCamera;
    public GameObject interactButton;
    public Text interactText;

    public float maxRayDistance;
    public Vector3 raycastOffset;

    public InputSetup inputSetup;
    public ShowDialog showDialog;
    public ItemBox itemBox;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        itemBox = GameObject.FindGameObjectWithTag("ItemBoxScript").GetComponent<ItemBox>();
        menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
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
            RayCasting();
        }

    }

    void RayCasting()
    {
        Ray ray = new Ray(transform.position + raycastOffset, transform.forward + raycastOffset);
        RaycastHit hit;
        Debug.DrawLine(transform.position + raycastOffset, transform.position + raycastOffset + transform.forward * maxRayDistance, Color.red);

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
                    ShowButton();
                    //jika object tersebut bisa berbicara
                    if (Input.GetKeyDown(inputSetup.interact) && interactable.isTalking == true)
                    {
                        TalkToObject(interactable);
                    }
                    //jika object tersebut bisa dimasukkan kedalam koleksi
                    if (Input.GetKeyDown(inputSetup.interact) && interactable.isCollectable == true)
                    {
                        //mengambil item tersebut
                        CollectObject(interactable);
                    }
                    if (Input.GetKeyDown(inputSetup.interact) && interactable.gameObject.tag == "ItemBox" && itemBox.isItemBoxOpened==false)
                    {
                        player.inventoryView.SetActive(true);
                        itemBox.itemBoxView.SetActive(true);
                        itemBox.isItemBoxOpened = true;
                        menuManager.StartCoroutine("ButtonInputHold");
                        menuManager.isOpen = true;
                        menuManager.ResetMenu();
                        GameStatus.PauseGame();
                        HideButton();
                    }
                }
            }
            catch
            {
                //Debug.Log(ex);
                //Debug.Log(hit.collider.gameObject.name + " is not interactable");
            }
        }
        else
        {
            HideButton();
        }
    }

    void TalkToObject(Interactable interactable)
    {
        bool isGivingAQuest = false;
        bool isCompletingAQuest = false;
        bool isHavingTheChainQuest = false;
        GameStatus.isTalking = true;
        mainCamera.transform.position = interactable.interactView.transform.position;
        mainCamera.transform.rotation = interactable.interactView.transform.rotation;
        HideButton();
        try
        {
            NPC interactedNPC = interactable.gameObject.GetComponent<NPC>();

            //mengecek apakah quest yang sedang dijalankan sudah selesai apa belum
            interactedNPC.CheckQuestProgress();

            for (int i = 0; i < player.playerChainQuest.Count; i++)
            {
                //jika quest npc yang aktif ada di list questchain player
                if (player.playerChainQuest[i] == interactedNPC.questIDActive)
                {
                    //bisa memberi quest
                    interactedNPC.canGiveQuest = true;
                    break;
                }
            }

            for (int i = 0; i < interactedNPC.questIDGiven.Count; i++)
            {
                if (interactedNPC.questIDGiven[i] == interactedNPC.questIDActive)
                {
                    //tidak memberi quest
                    interactedNPC.canGiveQuest = false;
                    break;
                }
            }

            for (int i = 0; i < player.playerChainQuest.Count; i++)
            {
                //jika quest yang sedang berjalan diplayer ada di npc tersebut
                if (player.playerChainQuest[i] == interactedNPC.questIDActive)
                {
                    isHavingTheChainQuest = true;
                    break;
                }
            }

            Debug.Log(interactedNPC.canGiveQuest);
            Debug.Log(interactedNPC.isHavingACompleteQuest);
            Debug.Log(isHavingTheChainQuest);
            //jika memiliki misi dan bisa memberikan misi
            if (interactedNPC.canGiveQuest==true)
            {
                isGivingAQuest = true;
                //dibuat false dulu, nanti di cek lagi
                interactedNPC.isHavingACompleteQuest = false;
                //memunculkan dialog quest untuk tersebut
                showDialog.StartQuestDialog(interactedNPC.questDialogActive, interactedNPC.questIDActive);
                //membuat npc tidak bisa memberi quest
                interactedNPC.questIDGiven.Add(interactedNPC.questIDActive);

                for (int i = 0; i < QuestDataBase.collectionQuest.Count; i++)
                {
                    if (QuestDataBase.collectionQuest[i].id == interactedNPC.questIDActive)
                    {
                        player.AddQuest(QuestDataBase.collectionQuest[i]);
                    }
                }

                interactedNPC.canGiveQuest = false;
                interactedNPC.CheckQuestProgress();
                Debug.Log(interactedNPC.isHavingACompleteQuest);
                Debug.Log("quest dialog");
            }
            else if (interactedNPC.canGiveQuest==false && interactedNPC.isHavingACompleteQuest==true && isHavingTheChainQuest == true)
            {
                isCompletingAQuest = true;
                //membuat npc tidak memiliki quest yang sudah selesai, karena baru saja menyelesaikan quest
                interactedNPC.isHavingACompleteQuest = false;

                if (interactedNPC.isHaveTheQuestChainQuest == true)
                {
                    interactedNPC.canGiveQuest = true;
                }
                //memunculkan dialog tentang menyelesaikan quest
                Debug.Log("Complete quest dialog");
                showDialog.StartQuestCompleteDialog(interactedNPC.questCompleteDialogActive, interactedNPC.questIDActive);
                //mereset data npc untuk quest selanjutnya
                interactedNPC.NextQuestValidation();
            }
        }
        catch(UnityException ex) {
            Debug.Log(ex);
            Debug.Log(interactable + " is not a NPC");
        }
        if (isGivingAQuest == false && isCompletingAQuest == false)
        {
            showDialog.StartDialog(interactable.dialog);
            Debug.Log("Normal dialog");
        }
    }


    void CollectObject(Interactable interactable)
    {
        //mengecek id dari item tersebut
        int itemID = interactable.gameObject.GetComponent<Interactable>().itemID;

        for (int i = 0; i < ItemDataBase.item.Count; i++)
        {
            //jika item yang ada didatabase sesuai dengan item yang diinteract
            //maka item tersebut dimasukkan kedalam koleksi item player
            if (ItemDataBase.item[i].id == itemID)
            {
                player.AddItem(ItemDataBase.item[i]);
                break;
            }
        }
        //gameobject item yang ada di hierarchy dihancurkan
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
