using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public static Quest instance;

    [Header("QuestDataBase")]
    //quest active, yaitu yang bisa diambil / sedang dikerjakan
    public List<CollectionQuest> collectionQuestActive = new List<CollectionQuest>();
    
    // npc yang ada ketika saat menjalankan function activate quest
    public GameObject[] npcAvailable;

    [Header("Quest Menu Settings")]
    //gameobject yang dipakai untuk show/.hide questnya
    public GameObject questView;
    //tempat dari list quest yang ada
    public GameObject questContent;
    //untuk scrollquest ketika sedang menselect quest indicator
    public Scrollbar questViewScrollbar;

    [Header("Quest Detail")]
    //prefab dari quest yang akan di spawn
    public GameObject questListPrefab;
    //game object yang berisi detail dari quest
    GameObject questDetail;
    //text deskripsi dari quest
    TextMeshProUGUI questDescription;
    //text tentang objective dari quest
    TextMeshProUGUI questObjective;

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
    
    void Start()
    {
        questView.SetActive(false);
        questDescription.text = "";
        questObjective.text = "";

        //add first quest
        collectionQuestActive.Add(GameDataBase.instance.colQuestList[0]);
        ActivateQuest();
    }

    /// <summary>
    /// quest untuk mengaktifkan activecollectionquest ke npc agar bisa diambil oleh player
    /// dipanggil pada saat quest sudah selesai, sehingga muncul quest baru
    /// </summary>
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

    /// <summary>
    /// function untuk merefresh quest detail sesuai dengan quest indicator yang dipilih
    /// </summary>
    /// <param name="questID">id dari quest</param>
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
