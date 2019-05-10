using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public int id; //start dari 1
    public Sprite itemImage;
    public string itemName;
    public string description;

    [Header("Quantity must be 1")]
    public int quantity;
    public int price;

    [Header("If usable, fill the item type")]
    public bool isUsable;
    public bool isConsumable;
    public bool isASingleTool;
    public string itemType;

    [Header("If item type = plant ")]
    public int plantID;

    [Header("Do not fill below this")]
    public bool isOnInventory;
    public bool isOnItemBox;

    public Item(int id, Sprite itemImage, string itemName, string description, bool isUsable, bool isConsumable, bool isASingleTool, string itemType)
    {
        quantity = 1;
        this.id = id;
        this.itemImage = itemImage;
        this.itemName = itemName;
        this.description = description;
        this.isUsable = isUsable;
        this.isConsumable = isConsumable;
        this.isASingleTool = isASingleTool;
        this.itemType = itemType;
        isOnInventory = false;
        isOnItemBox = false;
    }

    public void Use() {
        if (itemType != null)
        {
            if (itemType.ToLower().Equals("tool".ToLower()))
            {
                UseTool();
            }
            else if (itemType.ToLower().Equals("consumable".ToLower()))
            {
                UseConsumable();
            }
            else if (itemType.ToLower().Equals("seed".ToLower()))
            {
                UseSeed();
            }
        }
    }

    void UseTool()
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        if (itemName.ToLower().Equals("hoe".ToLower()))
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
            return;
        }
        else if (itemName.ToLower().Equals("waterscoop".ToLower()))
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
            return;
        }
    }

    void UseConsumable()
    {
        PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        if (itemName.ToLower().Equals("potion".ToLower()))
        {
            playerData.curHealth += 20;
            playerData.RefreshHp();
            Debug.Log("cur health : " + playerData.curHealth);
            return;
        }
    }

    void UseSeed() {
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
        return;
    }

    public bool IsItemUsable()
    {
        if (itemType != null)
        {
            PlayerData playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
            if (itemType.ToLower().Equals("consumable".ToLower()))
            {
                return true;
            }
            if (itemType.ToLower().Equals("seed".ToLower()))
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
