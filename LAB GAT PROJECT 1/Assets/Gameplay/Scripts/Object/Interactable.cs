using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteractable;
    public bool isCollectable;
    public int itemID;
    public string interactText;
    public bool isTalking;
    public List<string> dialog;
}
