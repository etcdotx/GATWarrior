using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Database/Item")]
public class Item : ScriptableObject
{
    public int id; //start dari 1
    public Sprite itemImage;
    public string itemName;
    public string description;

    [Header("Quantity must be 1")]
    public int quantity;
    public int maxQuantityOnInventory;

    [Header("Price")]
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

    public Item(int id, Sprite itemImage, string itemName, string description, int maxQuantityOnInventory,int price, bool isUsable, bool isConsumable, bool isASingleTool, string itemType)
    {
        quantity = 1;
        this.id = id;
        this.itemImage = itemImage;
        this.itemName = itemName;
        this.description = description;
        this.maxQuantityOnInventory = maxQuantityOnInventory;
        this.price = price;
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
        if (itemName.ToLower().Equals("hoe".ToLower()))
        {
            if (PlayerData.instance.stateNearSoil == true)
            {
                for (int i = 0; i < Soils.instance.soilID.Length; i++)
                {
                    if (PlantTrigger.instance.target.GetComponent<Soil>().soilID == Soils.instance.soilID[i])
                    {
                        Soils.instance.canBeHooed[i] = false;
                        PlantTrigger.instance.target.GetComponent<Soil>().soil.GetComponent<MeshRenderer>().material.color = Soils.instance.soilHooed;
                        break;
                    }
                }
            }
            return;
        }
        else if (itemName.ToLower().Equals("waterscoop".ToLower()))
        {
            if (PlayerData.instance.stateNearSoil == true)
            {
                for (int i = 0; i < Soils.instance.soilID.Length; i++)
                {
                    if (PlantTrigger.instance.target.GetComponent<Soil>().soilID == Soils.instance.soilID[i])
                    {
                        if (Soils.instance.canBeHooed[i] == false)
                        {
                            Soils.instance.needWater[i] = false;
                            PlantTrigger.instance.target.GetComponent<Soil>().soil.GetComponent<MeshRenderer>().material.color = Soils.instance.soilWet;
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
        if (itemName.ToLower().Equals("potion".ToLower()))
        {
            quantity--;
            PlayerStatus.instance.curHealth += 20;
            PlayerStatus.instance.RefreshHp();
            return;
        }
    }

    void UseSeed() {
        for (int i = 0; i < Soils.instance.soilID.Length; i++)
        {
            if (PlantTrigger.instance.target.GetComponent<Soil>().soilID == Soils.instance.soilID[i])
            {
                if (Soils.instance.state[i] == 0 && Soils.instance.canBeHooed[i] == false)
                {
                    Soils.instance.plantID[i] = plantID;
                    Soils.instance.state[i] = 1;
                    Soils.instance.StartPlanting(Soils.instance.soilID[i], plantID);
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
            if (itemType.ToLower().Equals("consumable".ToLower()))
            {
                return true;
            }
            if (itemName.ToLower().Equals("hoe".ToLower()) || itemType.ToLower().Equals("waterscoop".ToLower()) || 
                itemType.ToLower().Equals("seed".ToLower()))
            {
                if (PlayerData.instance.stateNearSoil == true)
                    return true;
            }
        }

        return false;
    }
}
