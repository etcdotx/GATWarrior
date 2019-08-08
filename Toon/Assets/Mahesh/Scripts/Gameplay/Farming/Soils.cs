using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soils : MonoBehaviour
{
    public static Soils instance;
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

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        FindPlant();
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

    /// <summary>
    /// function pertamakali untuk nyari object
    /// </summary>
    public void FindPlant()
    {
        plantNode = new GameObject[transform.childCount];
        plantNodeSoil = new Soil[transform.childCount];
        plantID = new int[transform.childCount];
        soilID = new int[transform.childCount];
        state = new int[transform.childCount];
        day = new int[transform.childCount];
        needWater = new bool[transform.childCount];
        gatherable = new bool[transform.childCount];
        canBeHooed = new bool[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            plantNode[i] = transform.GetChild(i).gameObject;
            plantNodeSoil[i] = plantNode[i].GetComponent<Soil>();
            plantNodeSoil[i].soilID = i;
            soilID[i] = i;
            needWater[i] = false;
            gatherable[i] = false;
            canBeHooed[i] = true;
        }

        count = 5;
    }

    /// <summary>
    /// function yang dipake ketika player menggunakan item
    /// </summary>
    /// <param name="soilID">soil yang di select</param>
    /// <param name="plantID">plant id yang ditanam</param>
    public void StartPlanting(int soilID, int plantID) {
        for (int i = 0; i < plantNode.Length; i++)
        {
            if (plantNodeSoil[i].soilID == soilID)
            {
                Instantiate(PlantDataBase.instance.plantState1[plantID],
                     plantNodeSoil[i].plantLocation.transform.position, Quaternion.identity ,
                     plantNodeSoil[i].plantLocation.transform);
                break;
            }
        }
    }

    /// <summary>
    /// function supaya plantnya bisa numbuh
    /// currently masih diupdate supaya bisa di debug
    /// </summary>
    /// <param name="soilID"></param>
    public void GrowPlant(int soilID) {
        for (int i = 0; i < plantNode.Length; i++)
        {
            if (plantNodeSoil[i].soilID == soilID)
            {
                if (state[i] == PlantDataBase.instance.plantMaxState[plantID[i]])
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
                    Debug.Log("Plant ID = " + plantID[i] + ", State = " + state[i] + ", Max State = " + PlantDataBase.instance.plantMaxState[plantID[i]]);
                    if (state[i] == 2)
                    {
                        Destroy(plantNodeSoil[i].plantLocation.transform.GetChild(0).gameObject);
                        Instantiate(PlantDataBase.instance.plantState2[plantID[i]],
                             plantNodeSoil[i].plantLocation.transform.position, Quaternion.identity,
                             plantNodeSoil[i].plantLocation.transform);
                        needWater[i] = true;
                        plantNodeSoil[i].soil.GetComponent<MeshRenderer>().material.color = soilNormal;
                    }
                    else if (state[i] == 3)
                    {
                        Destroy(plantNodeSoil[i].plantLocation.transform.GetChild(0).gameObject);
                        Instantiate(PlantDataBase.instance.plantState3[plantID[i]],
                             plantNodeSoil[i].plantLocation.transform.position, Quaternion.identity,
                             plantNodeSoil[i].plantLocation.transform);
                        needWater[i] = true;
                        plantNodeSoil[i].soil.GetComponent<MeshRenderer>().material.color = soilNormal;
                    }
                    else if(state[i] == 4)
                    {
                        Destroy(plantNodeSoil[i].plantLocation.transform.GetChild(0).gameObject);
                        Instantiate(PlantDataBase.instance.plantState4[plantID[i]],
                             plantNodeSoil[i].plantLocation.transform.position, Quaternion.identity,
                             plantNodeSoil[i].plantLocation.transform);
                        needWater[i] = true;
                        plantNodeSoil[i].soil.GetComponent<MeshRenderer>().material.color = soilNormal;
                    }

                    if (state[i] == PlantDataBase.instance.plantMaxState[plantID[i]])
                    {
                        Debug.Log("Seed " + plantID[i] + " on soil " + soilID + " is fully grow");
                    }
                    break;
                }
            }
        }
    }
}
