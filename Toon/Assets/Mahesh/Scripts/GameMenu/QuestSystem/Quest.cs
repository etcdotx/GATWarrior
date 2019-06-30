using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public static Quest instance;

    [Header("QuestDataBase")]
    public List<CollectionQuest> collectionQuestActive = new List<CollectionQuest>();
    public GameObject[] npcAvailable;

    [Header("Quest Menu Settings")]
    public GameObject questView;
    public GameObject questContent;
    //public GameObject questCompleteContent;
    public Scrollbar questViewScrollbar;

    [Header("Quest Detail")]
    public GameObject questDetail;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questObjective;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        questView = transform.GetChild(0).Find("QuestView").gameObject;
        questContent = questView.transform.Find("QuestViewPort").transform.Find("QuestContent").gameObject;
        questViewScrollbar = questView.transform.Find("QuestViewScrollbar").GetComponent<Scrollbar>();
        questDetail = questView.transform.Find("QuestDetail").gameObject;
        questDescription = questDetail.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questObjective = questDetail.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        questView.SetActive(false);
        questDescription.text = "";
        questObjective.text = "";

        //add first quest
        collectionQuestActive.Add(QuestDataBase.collectionQuest[0]);
        ActivateQuest();
    }

    public void ActivateQuest()
    {
        npcAvailable = GameObject.FindGameObjectsWithTag("NPC");
        for (int i = 0; i < npcAvailable.Length; i++)
        {
            npcAvailable[i].GetComponent<NPC>().activeCollectionQuest.Clear();
            for (int j = 0; j < collectionQuestActive.Count; j++)
            {
                if (collectionQuestActive[j].sourceID == npcAvailable[i].GetComponent<NPC>().sourceID)
                {
                    CollectionQuest cq = ScriptableObject.CreateInstance<CollectionQuest>();
                    cq.Duplicate(collectionQuestActive[j]);

                    npcAvailable[i].GetComponent<NPC>().activeCollectionQuest.Add(cq);

                }
            }
        }
        for (int i = 0; i < npcAvailable.Length; i++)
        {
            npcAvailable[i].GetComponent<NPC>().activeCollectionQuestTotal = npcAvailable[i].GetComponent<NPC>().activeCollectionQuest.Count;
        }
    }

    public void RefreshQuestDetail(int questID)
    {
        questDescription.text = "";
        questObjective.text = "";
        for (int i = 0; i < PlayerData.instance.collectionQuest.Count; i++)
        {
            if (PlayerData.instance.collectionQuest[i].id == questID)
            {
                if (PlayerData.instance.collectionQuest[i].isComplete == true)
                {
                    questObjective.text = PlayerData.instance.collectionQuest[i].ToString() + "\n" + "Quest Complete";
                }
                else
                    questObjective.text = PlayerData.instance.collectionQuest[i].ToString();

                questDescription.text = PlayerData.instance.collectionQuest[i].description;
                break;
            }
        }
    }

}
