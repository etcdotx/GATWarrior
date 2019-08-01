using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentQuestUI : MonoBehaviour
{
    public static CurrentQuestUI instance;
    public GameObject currentQuestPrefab;
    public Transform currentQuestPanel;
    public List<GameObject> currentQuest = new List<GameObject>();
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

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
