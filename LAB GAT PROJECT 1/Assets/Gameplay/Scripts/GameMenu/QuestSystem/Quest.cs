using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public PlayerData playerData;
    public GameMenuManager gameMenuManager;

    [Header("QuestDataBase")]
    public List<CollectionQuest> collectionQuestActive = new List<CollectionQuest>();
    public GameObject[] npcAvailable;

    [Header("Quest Menu Settings")]
    public int questIndex;
    public int questMaxIndex;
    public GameObject questView;
    public GameObject questContent;
    public Scrollbar questViewScrollbar;

    [Header("Quest Detail")]
    public GameObject questDetail;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questObjective;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        gameMenuManager = GameObject.FindGameObjectWithTag("GameMenuManager").GetComponent<GameMenuManager>();

        collectionQuestActive.Add(QuestDataBase.collectionQuest[0]);

        questView = GameObject.FindGameObjectWithTag("QuestUI").transform.Find("QuestView").gameObject;
        questContent = questView.transform.Find("QuestViewPort").transform.Find("QuestContent").gameObject;
        questViewScrollbar = questView.transform.Find("QuestViewScrollbar").GetComponent<Scrollbar>();
        questDetail = questView.transform.Find("QuestDetail").gameObject;
        questIndex = 0;
        questMaxIndex = questContent.transform.childCount - 1;

        questDescription = questDetail.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questObjective = questDetail.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        questDescription.text = "";
        questObjective.text = "";

        ActivateQuest();
    }

    public void ActivateQuest()
    {
        npcAvailable = GameObject.FindGameObjectsWithTag("NPC");
        for (int i = 0; i < collectionQuestActive.Count; i++)
        {
            for (int j = 0; j < npcAvailable.Length; j++)
            {
                if (collectionQuestActive[i].sourceID == npcAvailable[j].GetComponent<NPC>().sourceID)
                {
                    npcAvailable[j].GetComponent<NPC>().activeCollectionQuest.Add(collectionQuestActive[i]);
                }
                npcAvailable[j].GetComponent<NPC>().GetQuestDialog();
                npcAvailable[j].GetComponent<NPC>().GetQuestCompleteDialog();
            }
        }
    }

    public void QuestSelection()
    {
        if (gameMenuManager.inputAxis.y == -1)
        {
            if (questIndex < questMaxIndex)
            {
                questIndex += 1;
            }
            else
            {
                questIndex = 0;
            }
            //Debug.Log("in");
        }
        else if (gameMenuManager.inputAxis.y == 1)
        {
            if (questIndex > 0)
            {
                questIndex -= 1;
            }
            else
            {
                questIndex = questMaxIndex;
            }
            //Debug.Log("in");
        }
        ScrollQuest();
        MarkQuest();
        RefreshQuest();
    }

    public void ScrollQuest()
    {
        if (questIndex == questMaxIndex)
        {
            questViewScrollbar.value = 0;
        }
        else
        {
            float a = (float)questMaxIndex;
            float b = (float)questIndex;
            float c = a - b;
            float d = c / a;
            questViewScrollbar.value = c / a;
        }
    }

    public void MarkQuest()
    {
        try
        {
            for (int i = 0; i <= questMaxIndex; i++)
            {
                questContent.transform.GetChild(i).GetComponent<Image>().color = gameMenuManager.normalColor;
            }
            questContent.transform.GetChild(questIndex).GetComponent<Image>().color = gameMenuManager.selectedColor;
        }
        catch
        {
        }
    }

    public void RefreshQuest()
    {
        try
        {
            for (int i = 0; i < playerData.collectionQuest.Count; i++)
            {
                if (playerData.collectionQuest[i].id == questContent.transform.GetChild(questIndex).GetComponent<QuestIndicator>().questID)
                {
                    questDescription.text = playerData.collectionQuest[i].description;
                    questObjective.text = playerData.collectionQuest[i].ToString(); //newCol.description + newCol.ToString();
                    break;
                }
                else
                {
                    questDescription.text = "";
                }
            }
        }
        catch
        {
        }
    }

}
