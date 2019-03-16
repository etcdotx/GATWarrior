using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Player player;

    [Header("Set source number")]
    public int sourceID;
    
    public bool canGiveQuest;
    public bool isHavingACompleteQuest;
    public bool isHaveTheQuestChainQuest;

    [Header("Set first active quest id")]
    public int questIDActive;
    public List<int> questIDGiven = new List<int>();

    public List<CollectionQuest> collectionQuestList = new List<CollectionQuest>();
    public List<QuestDialog> questDialogList = new List<QuestDialog>();
    public List<QuestDialog> questCompleteDialogList = new List<QuestDialog>();

    public List<string> questDialogActive = new List<string>();
    public List<string> questCompleteDialogActive = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        GetQuestList();
        GetQuestDialog();
        GetQuestCompleteDialog();
        RefreshQuestDialog();
        RefreshQuestCompleteDialog();

        if (questDialogList.Count != 0)
        {
            canGiveQuest = true;
        }
        else
        {
            canGiveQuest = false;
        }
        isHavingACompleteQuest = false;
        isHaveTheQuestChainQuest = false;
    }


    public void CheckQuestProgress()
    {
        bool isTheChainIDExist = false;
        for (int i = 0; i < player.collectionQuestComplete.Count; i++)
        {
            //jika collection quest yang sudah selesai = quest yang aktif
            if (player.collectionQuestComplete[i].id == questIDActive)
            {
                //jika quest yang sedang dijalankan sudah selesai, maka dia memiliki quest yang sudah selesai
                isHavingACompleteQuest = true;

                //memasukkan chain quest list yang akan dikerjakan player nantinya
                try
                {
                    Debug.Log("ok");
                    for (int z = 0; z < player.playerChainQuest.Count; z++)
                    {
                        if (player.playerChainQuest[z] == player.collectionQuestComplete[i].chainQuestID)
                        {
                            Debug.Log("2");
                            isTheChainIDExist = true;
                            break;
                        }
                    }
                    if (isTheChainIDExist == false)
                    {
                        player.playerChainQuest.Add(player.collectionQuestComplete[i].chainQuestID);
                    }
                }
                catch { }
                player.collectionQuestUnusable.Add(player.collectionQuestComplete[i]);
                player.collectionQuestComplete.Remove(player.collectionQuestComplete[i]);

                break;
            }
        }
    }

    public void NextQuestValidation()
    {
        for (int i = 0; i < collectionQuestList.Count; i++)
        {
            //jika quest yang dimiliki npc sesuai dengan quest id yang sedang active
            if (collectionQuestList[i].id == questIDActive)
            {
                if (collectionQuestList[i].chainQuestID != 0)
                {
                    //mengecek jika quest yang sedang aktif memiliki chainquest di npc ini
                    for (int k = 0; k < collectionQuestList.Count; k++)
                    {
                        if (collectionQuestList[i].chainQuestID == collectionQuestList[k].id)
                        {
                            //maka quest yang aktif sekarang menjadi quest chain yg ada di npc ini
                            questIDActive = collectionQuestList[i].chainQuestID;

                            //jika quest yang active memiliki chain quest
                            isHaveTheQuestChainQuest = true;
                            break;
                        }
                        else {
                            isHaveTheQuestChainQuest = false;
                        }
                    }
                    //jika chain quest id tersebut tidak dimiliki npc
                    if (isHaveTheQuestChainQuest == false)
                    {
                        //maka quest id active dimasukkan kedalam quest selanjutnya
                        //namun jika dia tidak punya quest lagi, dia membuat chainnya menjadi 0
                        try
                        {
                            questIDActive = collectionQuestList[i + 1].id;
                        } catch
                        {
                            questIDActive = 0;
                        }
                        //tidak bisa memberi quest, karena baru saja memberi quest
                        canGiveQuest = false;
                    }
                    //mereset string dialog quest baru
                    RefreshQuestDialog();
                    //mereset string dialog complete untuk quest baru
                    RefreshQuestCompleteDialog();
                    break;
                }
            }
        }
    }


    public void RefreshQuestCompleteDialog()
    {
        questCompleteDialogActive.Clear();
        for (int i = 0; i < questCompleteDialogList.Count; i++)
        {
            if (questCompleteDialogList[i].questID == questIDActive)
            {
                questCompleteDialogActive.Add(questCompleteDialogList[i].dialog);
            }
        }
        Debug.Log("In");
    }

    public void RefreshQuestDialog()
    {
        questDialogActive.Clear();
        for (int i = 0; i < questDialogList.Count; i++)
        {
            if (questDialogList[i].questID == questIDActive)
            {
                questDialogActive.Add(questDialogList[i].dialog);
            }
        }
    }

    #region AMBIL DATA DARI DATABASE
    public void GetQuestList()
    {
        for (int i = 0; i < QuestDataBase.collectionQuest.Count; i++)
        {
            if (QuestDataBase.collectionQuest[i].sourceID == sourceID)
            {
                collectionQuestList.Add(QuestDataBase.collectionQuest[i]);
            }
        }
    }

    public void GetQuestDialog()
    {
        for (int i = 0; i < QuestDataBase.questDialog.Count; i++)
        {
            if (QuestDataBase.questDialog[i].sourceID == sourceID)
            {
                questDialogList.Add(QuestDataBase.questDialog[i]);
            }
        }
    }

    public void GetQuestCompleteDialog()
    {
        for (int i = 0; i < QuestDataBase.questCompleteDialog.Count; i++)
        {
            if (QuestDataBase.questCompleteDialog[i].sourceID == sourceID)
            {
                questCompleteDialogList.Add(QuestDataBase.questCompleteDialog[i]);
            }
        }
    }
    #endregion

}
