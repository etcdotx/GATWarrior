using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentQuestUI : MonoBehaviour
{
    public static CurrentQuestUI instance;
    //prefab current quest yang akan di spawn
    public GameObject currentQuestPrefab;
    //panel tempat kumpulan prefab
    public Transform currentQuestPanel;
    //list prefab yang sudah dispawn
    public List<GameObject> currentQuest = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    /// <summary>
    /// function yang dipanggil ketika ada quest baru atau ada progress baru dari quest
    /// </summary>
    public void Refresh() {
        foreach (GameObject g in currentQuest)
            Destroy(g);

        currentQuest.Clear();

        for (int i = 0; i < PlayerData.instance.collectionQuest.Count; i++)
        {
            TextMeshProUGUI temp = Instantiate(currentQuestPrefab, currentQuestPanel).GetComponent<TextMeshProUGUI>();
            temp.text = PlayerData.instance.collectionQuest[i].ToString();
            currentQuest.Add(temp.gameObject);
        }
    }
}
