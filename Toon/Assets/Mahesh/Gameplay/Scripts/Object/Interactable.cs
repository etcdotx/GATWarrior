﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Is interactable")]
    public bool isInteractable;

    [Header("Is Collectable")]
    public bool isCollectable;
    public List<int> itemID = new List<int>();

    [Header("Set interact text")]
    public string interactText;

    [Header("Is showing text")]
    public List<string> readAble;

    [Header("Is talking")]
    public bool isTalking;
}
