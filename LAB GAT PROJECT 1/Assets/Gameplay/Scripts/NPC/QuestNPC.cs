using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestNPC : MonoBehaviour
{
    public NPC npc;
    public bool isHaveQuest;


    // Start is called before the first frame update
    void Start()
    {
        npc = gameObject.GetComponent<NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
