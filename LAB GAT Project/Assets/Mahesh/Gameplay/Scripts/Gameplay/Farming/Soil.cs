using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public GameObject soil;
    public GameObject selectedIndicator;
    public GameObject plantLocation;
    public int soilID;

    // Update is called once per frame
    private void Start()
    {
        selectedIndicator.SetActive(false);
    }
}