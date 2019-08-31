using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType {
    consumable, tool, seed, material
}

[CreateAssetMenu(fileName = "New Item", menuName = "Database/Item")]
public class Item : ScriptableObject
{
    public ItemType itemType;
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

    [Header("If item type = plant ")]
    public int plantID;

    [Header("Do not fill below this")]
    public bool isOnInventory; //untuk nanti di refresh di inventory
    public bool isOnInventoryBox; //untuk nanti di refresh di inventorybox

    public void Duplicate(Item item)
    {
        quantity = 1;
        this.id = item.id;
        this.itemImage = item.itemImage;
        this.itemName = item.itemName;
        this.description = item.description;
        this.maxQuantityOnInventory = item.maxQuantityOnInventory;
        this.price = item.price;
        this.isUsable = item.isUsable;
        this.isConsumable = item.isConsumable;
        this.isASingleTool = item.isASingleTool;
        this.itemType = item.itemType;
        this.plantID = item.plantID;
        isOnInventory = false;
        isOnInventoryBox = false;
    }

    /// <summary>
    /// Function untuk menggunakan item
    /// </summary>
    public void Use()
    {
        if (itemType == ItemType.tool) //jika item jenis tool
        {
            UseTool();
        }
        else if (itemType == ItemType.consumable)//jika item jenis tool
        {
            UseConsumable();
        }
        else if (itemType == ItemType.seed)//jika item jenis tool
        {
            UseSeed();
        }
    }

    void UseTool()
    {
        if (itemName.ToLower().Equals("hoe".ToLower()))
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
            return;
        }
        else if (itemName.ToLower().Equals("waterscoop".ToLower()))
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
        if (itemName.ToLower().Equals("hi potion".ToLower()))
        {
            quantity--;
            PlayerStatus.instance.curHealth += 50;
            PlayerStatus.instance.RefreshHp();
            return;
        }
        if (itemName.ToLower().Equals("Apple".ToLower()))
        {
            quantity--;
            PlayerStatus.instance.curHealth += 10;
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
        if (itemType == ItemType.consumable && PlayerStatus.instance.curHealth < PlayerStatus.instance.maxHealth)
        {
            return true;
        }
        if (itemType == ItemType.tool)
        {
            if (itemName.ToLower().Equals("hoe".ToLower()) || itemName.ToLower().Equals("waterscoop".ToLower()) ||
                itemName.ToLower().Equals("seed".ToLower()))
            {
                if (PlantTrigger.instance.target != null)
                    return true;
            }
        }

        return false;
    }
}
