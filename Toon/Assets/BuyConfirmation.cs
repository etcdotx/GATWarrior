using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyConfirmation : MonoBehaviour
{
    public Item item;
    public Image itemImage;
    public TextMeshProUGUI itemName;

    public int curQty;
    public TextMeshProUGUI curQuantity;

    public int nextQty;
    public TextMeshProUGUI nextQuantity;

    public int maxQty;
    public TextMeshProUGUI maxQuantity;

    public int setQty;
    public int setMaxQty;
    public TextMeshProUGUI setQuantity;

    public int prc;
    public int totalprc;
    public TextMeshProUGUI totalPrice;

    public int curGold;
    public int nextGold;
    public TextMeshProUGUI cGold;
    public TextMeshProUGUI nGold;


    public GameObject increase;
    public GameObject decrease;

    public void InitiateConfirmation(Item getItem, PlayerData playerData) {        
        increase.SetActive(true);
        item = getItem;
        itemImage.sprite = item.itemImage;
        itemName.text = item.itemName;


        curQty = 0;
        setQty = 1;
        setMaxQty = 1;

        maxQty = item.maxQuantityOnInventory;
        maxQuantity.text = "/"+maxQty.ToString();
        prc = item.price;

        for (int i = 0; i < playerData.inventoryItem.Count; i++)
        {
            if (playerData.inventoryItem[i].id == item.id)
            {
                curQty = playerData.inventoryItem[i].quantity;
                break;
            }
        }

        curGold = playerData.gold;
        cGold.text = curGold.ToString();
        setMaxQty = maxQty-curQty;
        if (setMaxQty > (curGold / prc))
        {
            setMaxQty = curGold / prc;
        }            

        curQuantity.text = curQty.ToString();
        decrease.SetActive(false);
        if(setMaxQty==1)
            increase.SetActive(false);

        RefreshText();
    }

    public void RefreshText()
    {
        nextQty = curQty + setQty;
        totalprc = prc * setQty;
        nextGold = curGold - totalprc;

        nGold.text = nextGold.ToString();
        setQuantity.text = setQty.ToString();
        nextQuantity.text = nextQty.ToString();
        totalPrice.text = totalprc.ToString();
    }

    public void IncreaseQuantity() {
        if (setQty < setMaxQty)
            setQty++;
        if (setQty == setMaxQty)
            increase.SetActive(false);
        if (setQty > 1)
            decrease.SetActive(true);

        RefreshText();
    }

    public void DecreaseQuantity() {
        if (setQty > 1)
            setQty--;
        if (setQty == 1)
            decrease.SetActive(false);
        if(setQty<setMaxQty)
            increase.SetActive(true);

        RefreshText();
    }

    public void SetMaxQuantity()
    {
        setQty = setMaxQty;
        if (setMaxQty > 1)
        {
            decrease.SetActive(true);
            increase.SetActive(false);
        }

        RefreshText();
    }

    public void SetMinQuantity() {
        setQty = 1;
        if (setMaxQty > 1)
        {
            decrease.SetActive(false);
            increase.SetActive(true);
        }

        RefreshText();
    }

    public void ConfirmBuy(Inventory inventory, PlayerData playerData) {
        int gold = playerData.gold;
        playerData.gold -= totalprc;
        inventory.PlaceItem(item, setQty);
    }
}
