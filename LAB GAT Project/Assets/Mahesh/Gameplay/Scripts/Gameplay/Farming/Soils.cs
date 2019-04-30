using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soils : MonoBehaviour
{
    public PlantDataBase plantDataBase;
    public GameObject plantNodes;
    public GameObject[] plantNode;
    public Soil[] plantNodeSoil;
    public int[] plantID;
    public int[] soilID;
    public int[] state;
    public int[] day;
    public bool[] needWater;
    public bool[] gatherable;
    public bool[] canBeHooed;

    public Color32 soilNormal;
    public Color32 soilHooed;
    public Color32 soilWet;

    public float count;

    // Start is called before the first frame update
    void Start()
    {
        plantDataBase = GameObject.FindGameObjectWithTag("PlantDataBase").GetComponent<PlantDataBase>();
        plantNodes = GameObject.FindGameObjectWithTag("PlantNodes");
        plantNode = new GameObject[plantNodes.transform.childCount];
        plantNodeSoil = new Soil[plantNodes.transform.childCount];
        plantID = new int[plantNodes.transform.childCount];
        soilID = new int[plantNodes.transform.childCount];
        state = new int[plantNodes.transform.childCount];
        day = new int[plantNodes.transform.childCount];
        needWater = new bool[plantNodes.transform.childCount];
        gatherable = new bool[plantNodes.transform.childCount];
        canBeHooed = new bool[plantNodes.transform.childCount];

        for (int i = 0; i < plantNodes.transform.childCount; i++)
        {
            plantNode[i] = plantNodes.transform.GetChild(i).gameObject;
            plantNodeSoil[i] = plantNode[i].GetComponent<Soil>();
            plantNodeSoil[i].soilID = i;
            soilID[i] = i;
            needWater[i] = false;
            gatherable[i] = false;
            canBeHooed[i] = true;
        }

        count = 5;
    }

    private void Update()
    {
        if (count > 0)
        {
            count -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < plantNode.Length; i++)
            {
                if (plantID[i] != 0)
                {
                    GrowPlant(soilID[i]);
                }
            }
            count = 5;
        }

    }

    public void StartPlanting(int soilID, int plantID) {
        for (int i = 0; i < plantNode.Length; i++)
        {
            if (plantNodeSoil[i].soilID == soilID)
            {
                Instantiate(plantDataBase.plantState1[plantID],
                     plantNodeSoil[i].plantLocation.transform.position, Quaternion.identity ,
                     plantNodeSoil[i].plantLocation.transform);
                break;
            }
        }
    }

    public void GrowPlant(int soilID) {
        for (int i = 0; i < plantNode.Length; i++)
        {
            if (plantNodeSoil[i].soilID == soilID)
            {
                if (state[i] == plantDataBase.plantMaxState[plantID[i]])
                {
                    if (plantNodeSoil[i].plantLocation.transform.childCount == 0)
                    {
                        state[i] = 0;
                        plantID[i] = 0;
                        canBeHooed[i] = true;
                    }
                    Debug.Log("seed " + plantID + " cant grow more");
                    break;
                }
                else
                {
                    if (needWater[i] == false)
                    {
                        state[i]++;
                    }
                    Debug.Log("Plant ID = " + plantID[i] + ", State = " + state[i] + ", Max State = " + plantDataBase.plantMaxState[plantID[i]]);
                    if (state[i] == 2)
                    {
                        Destroy(plantNodeSoil[i].plantLocation.transform.GetChild(0).gameObject);
                        Instantiate(plantDataBase.plantState2[plantID[i]],
                             plantNodeSoil[i].plantLocation.transform.position, Quaternion.identity,
                             plantNodeSoil[i].plantLocation.transform);
                        needWater[i] = true;
                        plantNodeSoil[i].soil.GetComponent<MeshRenderer>().material.color = soilNormal;
                    }
                    else if (state[i] == 3)
                    {
                        Destroy(plantNodeSoil[i].plantLocation.transform.GetChild(0).gameObject);
                        Instantiate(plantDataBase.plantState3[plantID[i]],
                             plantNodeSoil[i].plantLocation.transform.position, Quaternion.identity,
                             plantNodeSoil[i].plantLocation.transform);
                        needWater[i] = true;
                        plantNodeSoil[i].soil.GetComponent<MeshRenderer>().material.color = soilNormal;
                    }
                    else if(state[i] == 4)
                    {
                        Destroy(plantNodeSoil[i].plantLocation.transform.GetChild(0).gameObject);
                        Instantiate(plantDataBase.plantState4[plantID[i]],
                             plantNodeSoil[i].plantLocation.transform.position, Quaternion.identity,
                             plantNodeSoil[i].plantLocation.transform);
                        needWater[i] = true;
                        plantNodeSoil[i].soil.GetComponent<MeshRenderer>().material.color = soilNormal;
                    }

                    if (state[i] == plantDataBase.plantMaxState[plantID[i]])
                    {
                        Debug.Log("Seed " + plantID[i] + " on soil " + soilID + " is fully grow");
                    }
                    break;
                }
            }
        }
    }
}
