using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDataBase : MonoBehaviour
{
    public static PlantDataBase instance;

    public GameObject[] plantState1;
    public GameObject[] plantState2;
    public GameObject[] plantState3;
    public GameObject[] plantState4;
    public GameObject[] plantState5;
    public int[] plantMaxState;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
}
