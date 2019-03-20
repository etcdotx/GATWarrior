using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public CharacterRayCast characterRayCast;
    public InputSetup inputSetup;
    public ShowDialog showDialog;
    // Start is called before the first frame update
    void Start()
    {
        characterRayCast = gameObject.GetComponent<CharacterRayCast>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        showDialog = GameObject.FindGameObjectWithTag("ShowDialog").GetComponent<ShowDialog>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TalkToObject(Interactable interactable) {
        bool isGivingAQuest = false;
        bool isCompletingAQuest = false;
        bool isHavingTheChainQuest = false;
        GameStatus.isTalking = true;
        characterRayCast.HideButton();
        try
        {
            NPC interactedNPC = interactable.gameObject.GetComponent<NPC>();

            //mengecek apakah quest yang sedang dijalankan sudah selesai apa belum
            interactedNPC.CheckQuestProgress();

            for (int i = 0; i < playerData.playerChainQuest.Count; i++)
            {
                //jika quest npc yang aktif ada di list questchain player
                if (playerData.playerChainQuest[i] == interactedNPC.questIDActive)
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

            for (int i = 0; i < playerData.playerChainQuest.Count; i++)
            {
                //jika quest yang sedang berjalan diplayer ada di npc tersebut
                if (playerData.playerChainQuest[i] == interactedNPC.questIDActive)
                {
                    isHavingTheChainQuest = true;
                    break;
                }
            }

            //Debug.Log(interactedNPC.canGiveQuest);
            //Debug.Log(interactedNPC.isHavingACompleteQuest);
            //Debug.Log(isHavingTheChainQuest);
            //jika memiliki misi dan bisa memberikan misi
            if (interactedNPC.canGiveQuest == true)
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
                        Debug.Log(interactedNPC.questIDActive);
                        playerData.AddQuest(QuestDataBase.collectionQuest[i]);
                    }
                }

                interactedNPC.canGiveQuest = false;
                interactedNPC.CheckQuestProgress();
                Debug.Log(interactedNPC.isHavingACompleteQuest);
                Debug.Log("quest dialog");
            }
            else if (interactedNPC.canGiveQuest == false && interactedNPC.isHavingACompleteQuest == true && isHavingTheChainQuest == true)
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
        catch (UnityException ex)
        {
            Debug.Log(ex);
            Debug.Log(interactable + " is not a NPC");
        }
        if (isGivingAQuest == false && isCompletingAQuest == false)
        {
            showDialog.StartDialog(interactable.dialog);
            Debug.Log("Normal dialog");
        }
    }
}
