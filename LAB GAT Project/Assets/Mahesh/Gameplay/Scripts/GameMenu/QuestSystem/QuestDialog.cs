using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDialog
{
    public string dialog;
    public int questID;
    public int sourceID;

    public QuestDialog(int sourceID, int questID, string dialog)
    {
        this.sourceID = sourceID;
        this.questID = questID;
        this.dialog = dialog;
    }
}
