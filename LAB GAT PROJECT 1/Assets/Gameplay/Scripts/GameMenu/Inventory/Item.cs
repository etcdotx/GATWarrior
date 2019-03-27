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
    public string imagePath;
    public bool isUsable;
    public bool isOnInventory;
    public bool isOnItemBox;
    public bool isASingleTool;

    public Item(int id, string imagePath, string name, string description, bool isUsable, bool isASingleTool)
    {
        quantity = 1;
        this.id = id;
        this.imagePath = imagePath;
        itemImage = Resources.Load<Sprite>(this.imagePath);
        this.name = name;
        this.description = description;
        this.isUsable = isUsable;
        this.isASingleTool = isASingleTool;
        isOnInventory = false;
        isOnItemBox = false;
    }
}
