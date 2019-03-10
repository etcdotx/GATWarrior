using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDialog : MonoBehaviour
{
    public List<string> dialog = new List<string>();
    public int dialNum;
    public Text conversation;
    public InputSetup inputSetup;

    public void Start()
    {
        conversation = GameObject.FindGameObjectWithTag("Conversation").GetComponent<Text>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
    }

    public void Update()
    {
        if (GameStatus.isTalking == true)
        {
            if (Input.GetKeyDown(inputSetup.deleteSave))
            {
                nextDialogue();
            }
        }
        else {
            conversation.gameObject.SetActive(false);
        }
    }

    public void startDialog(string[] dial)
    {
        GetDialog(dial);
        dialNum = 0;
        conversation.gameObject.SetActive(true);
        conversation.text = dialog[dialNum];
    }

    private void GetDialog(string[] dial)
    {
        dialog.Clear();
        for (int i = 0; i < dial.Length; i++)
        {
            dialog.Add(dial[i]);
        }
    }

    public void nextDialogue() {
        if (dialNum == dialog.Count - 1)
        {
            conversation.gameObject.SetActive(false);
            GameStatus.isTalking = false;
        }
        else
        {
            dialNum++;
            conversation.text = dialog[dialNum];
        }
    }
}
