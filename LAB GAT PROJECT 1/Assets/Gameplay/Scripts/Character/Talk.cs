using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public CharacterRayCast characterRayCast;
    public InputSetup inputSetup;
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        characterRayCast = gameObject.GetComponent<CharacterRayCast>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        dialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Dialogue>();
    }

    public void TalkToObject(Interactable interactable)
    {
        try
        {
            NPC thisNPC = interactable.gameObject.GetComponent<NPC>();
            int totalDialogueOption = thisNPC.activeCollectionQuest.Count;
            if (totalDialogueOption != 0)
            {
                dialogue.showDialogueOption(thisNPC.activeCollectionQuest);
            }
        }
        catch {

        }
    }

    void DoSingleDialoge()
    {

    }
}
