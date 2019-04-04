using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
    public int id; //start dari 1
    public int plantID;
    public int quantity;
    public Sprite itemImage;
    public string name;
    public string description;
    public string imagePath;
    public bool isUsable;
    public bool isOnInventory;
    public bool isOnItemBox;
    public bool isASingleTool;
    public string itemType;

    public Item(int id, string imagePath, string name, string description, bool isUsable, bool isASingleTool, string itemType)
    {
        quantity = 1;
        this.id = id;
        this.imagePath = imagePath;
        itemImage = Resources.Load<Sprite>(this.imagePath);
        this.name = name;
        this.description = description;
        this.isUsable = isUsable;
        this.isASingleTool = isASingleTool;
        this.itemType = itemType;
        isOnInventory = false;
        isOnItemBox = false;
    }

    public void Use() {
        if (itemType != null)
        {
            PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
            if (itemType.ToLower().Equals("potion".ToLower()))
            {
                playerData.curHealth += 100;
                Debug.Log("cur health : "+playerData.curHealth);
                return;
            }
            else if (itemType.ToLower().Equals("plant".ToLower()))
            {
                PlantTrigger plantTrigger = GameObject.FindGameObjectWithTag("PlantTrigger").GetComponent<PlantTrigger>();
                Soils soils = GameObject.FindGameObjectWithTag("Soils").GetComponent<Soils>();

                for (int i = 0; i < soils.soilID.Length; i++)
                {
                    if (plantTrigger.target.GetComponent<Soil>().soilID == soils.soilID[i])
                    {
                        if (soils.state[i] == 0 && soils.canBeHooed[i] == false)
                        {
                            soils.plantID[i] = plantID;
                            soils.state[i] = 1;
                            soils.StartPlanting(soils.soilID[i], plantID);
                            break;
                        }
                    }
                }
                Debug.Log("seed used");
                return;
            }
            else if (itemType.ToLower().Equals("hoe".ToLower()))
            {
                if (playerData.stateNearSoil == true)
                {
                    PlantTrigger plantTrigger = GameObject.FindGameObjectWithTag("PlantTrigger").GetComponent<PlantTrigger>();
                    Soils soils = GameObject.FindGameObjectWithTag("Soils").GetComponent<Soils>();

                    for (int i = 0; i < soils.soilID.Length; i++)
                    {
                        if (plantTrigger.target.GetComponent<Soil>().soilID == soils.soilID[i])
                        {
                            soils.canBeHooed[i] = false;
                            plantTrigger.target.GetComponent<Soil>().soil.GetComponent<MeshRenderer>().material.color = soils.soilHooed;
                            break;
                        }
                    }
                }
                //Debug.Log("hoe used");
                return;
            }
            else if (itemType.ToLower().Equals("waterscoop".ToLower()))
            {
                if (playerData.stateNearSoil == true)
                {
                    PlantTrigger plantTrigger = GameObject.FindGameObjectWithTag("PlantTrigger").GetComponent<PlantTrigger>();
                    Soils soils = GameObject.FindGameObjectWithTag("Soils").GetComponent<Soils>();

                    for (int i = 0; i < soils.soilID.Length; i++)
                    {
                        if (plantTrigger.target.GetComponent<Soil>().soilID == soils.soilID[i])
                        {
                            if (soils.canBeHooed[i] == false)
                            {
                                soils.needWater[i] = false;
                                plantTrigger.target.GetComponent<Soil>().soil.GetComponent<MeshRenderer>().material.color = soils.soilWet;
                                break;
                            }
                        }
                    }
                }
                Debug.Log("hoe used");
                return;
            }
        }
    }

    public bool IsItemUsable()
    {
        if (itemType != null)
        {
            PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
            if (itemType.ToLower().Equals("potion".ToLower()))
            {
                return true;
            }
            if (itemType.ToLower().Equals("plant".ToLower()))
            {
                if(playerData.stateNearSoil==true)
                    return true;
            }
            if (itemType.ToLower().Equals("hoe".ToLower()))
            {
                return true;
            }
            if (itemType.ToLower().Equals("waterscoop".ToLower()))
            {
                return true;
            }
        }

        return false;
    }
}
