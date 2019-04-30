using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public CharacterInteraction characterInteraction;
    public InputSetup inputSetup;
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        characterInteraction = gameObject.GetComponent<CharacterInteraction>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        dialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Dialogue>();
    }

    public void TalkToObject(Interactable interactable)
    {
        try
        {
            NPC thisNPC = interactable.gameObject.GetComponent<NPC>();
            int totalDialogueOption = thisNPC.activeCollectionQuest.Count;
            if (thisNPC.isAShop == true)
                totalDialogueOption += 1;

            if (totalDialogueOption != 0)
            {
                dialogue.StartNewDialogue(thisNPC, thisNPC.activeCollectionQuest, thisNPC.npcDialog, thisNPC.optionDialog, true);
            }
            else
            {
                dialogue.StartNewDialogue(thisNPC, null, thisNPC.npcDialog, null, false);          
            }
        }
        catch {

        }
    }
}
