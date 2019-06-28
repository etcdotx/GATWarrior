using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public CharacterInteraction characterInteraction;

    // Start is called before the first frame update
    void Start()
    {
        characterInteraction = gameObject.GetComponent<CharacterInteraction>();
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
                Conversation.instance.StartNewDialogue(thisNPC, thisNPC.activeCollectionQuest, thisNPC.npcDialog, thisNPC.optionDialog, true);
            }
            else
            {
                Conversation.instance.StartNewDialogue(thisNPC, null, thisNPC.npcDialog, null, false);          
            }
        }
        catch {

        }
    }
}
