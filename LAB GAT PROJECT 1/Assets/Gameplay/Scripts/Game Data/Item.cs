using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
    public int id; //start dari 1
    public int quantity;
    public Sprite itemImage;
    public string name;
    public string description;
    public bool isUsable;
    public bool isOnInventory;

    public Item(int id, string imagePath, string name, string description, bool isUsable)
    {
        quantity = 1;
        this.id = id;
        itemImage = Resources.Load<Sprite>(imagePath);
        this.name = name;
        this.description = description;
        this.isUsable = isUsable;
        isOnInventory = false;
    }
}
