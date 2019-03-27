using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public bool isGatherable;
    public bool needWater;
    public int plantID;
    public int soilID;
    public int state;
    public int day;

    public bool selected;
    public GameObject selectedIndicator;

    // Update is called once per frame
    private void Start()
    {
        selectedIndicator.SetActive(false);
    }
}