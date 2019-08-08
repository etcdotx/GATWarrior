using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    //gameobjectnya
    public GameObject soil;
    //indicator ketika lagi kena raycast
    public GameObject selectedIndicator;
    //buat spawn plantnya
    public GameObject plantLocation;
    //id soilnya
    public int soilID;

    // Update is called once per frame
    private void Start()
    {
        selectedIndicator.SetActive(false);
    }
}