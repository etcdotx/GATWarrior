using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyConfirmation : MonoBehaviour
{
    public static BuyConfirmation instance;

    public GameObject view;

    public Item item;
    public Image itemImage;
    public TextMeshProUGUI itemName;

    [Header("Inventory")]
    public GameObject inventoryQty;

    public int curQty;
    public TextMeshProUGUI curQuantity;

    public int nextQty;
    public TextMeshProUGUI nextQuantity;

    public int maxQty;
    public TextMeshProUGUI maxQuantity;

    [Header("InventoryBox")]
    public GameObject inventoryBoxQty;

    public int curQty2;
    public TextMeshProUGUI curQuantity2;

    public int nextQty2;
    public TextMeshProUGUI nextQuantity2;


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

    public Slider slider;
    public ConfirmationState confirmationState;

    public enum ConfirmationState {
        Buy, SendToBox, Sell
    }
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        view = transform.GetChild(0).gameObject;
        confirmationState = ConfirmationState.Buy;
    }

    private void Start()
    {
        view.SetActive(false);
    }

    public void InitiateBuyConfirmation(Item getItem)
    {
        confirmationState = ConfirmationState.Buy;
        inventoryQty.SetActive(true);
        inventoryBoxQty.SetActive(false);

        item = getItem;
        itemImage.sprite = item.itemImage;
        itemName.text = item.itemName;

        curQty = 0;
        setQty = 1;
        setMaxQty = 1;

        maxQty = item.maxQuantityOnInventory;
        maxQuantity.text = "/"+maxQty.ToString();
        prc = item.price;

        for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
        {
            if (PlayerData.instance.inventoryItem[i].id == item.id)
            {
                curQty = PlayerData.instance.inventoryItem[i].quantity;
                break;
            }
        }

        curGold = PlayerData.instance.gold;
        cGold.text = curGold.ToString();
        setMaxQty = maxQty-curQty;
        if (setMaxQty > (curGold / prc))
        {
            setMaxQty = curGold / prc;
        }            

        curQuantity.text = curQty.ToString();
        RefreshText();
        SetSliderState();
    }

    public void InitiateSendConfirmation(Item getItem)
    {
        confirmationState = ConfirmationState.SendToBox;
        inventoryQty.SetActive(false);
        inventoryBoxQty.SetActive(true);

        item = getItem;
        itemImage.sprite = item.itemImage;
        itemName.text = item.itemName;


        curQty2 = 0;
        setQty = 1;
        setMaxQty = 1;
        prc = item.price;

        for (int i = 0; i < PlayerData.instance.inventoryBoxItem.Count; i++)
        {
            if (PlayerData.instance.inventoryBoxItem[i].id == item.id)
            {
                curQty2 = PlayerData.instance.inventoryBoxItem[i].quantity;
                break;
            }
        }

        curGold = PlayerData.instance.gold;
        cGold.text = curGold.ToString();
        setMaxQty = 99999;
        if (setMaxQty > (curGold / prc))
        {
            setMaxQty = curGold / prc;
        }

        curQuantity2.text = curQty2.ToString();
        RefreshText();
        SetSliderState();
    }

    public void InitiateSellConfirmation(Item getItem)
    {
        confirmationState = ConfirmationState.Sell;

    }

    public void SetSliderState() {
        slider.minValue = 1;
        slider.value = 1;
        slider.maxValue = setMaxQty;
        slider.gameObject.SetActive(true);
        UIManager.instance.eventSystem.SetSelectedGameObject(slider.gameObject);
    }

    public void RefreshText()
    {
        nextQty = curQty + setQty;
        nextQty2 = curQty2 + setQty;
        totalprc = prc * setQty;
        nextGold = curGold - totalprc;

        nGold.text = nextGold.ToString();
        setQuantity.text = setQty.ToString();
        nextQuantity.text = nextQty.ToString();
        nextQuantity2.text = nextQty2.ToString();
        totalPrice.text = totalprc.ToString();
    }

    public void ConfirmBuy() {
        int gold = PlayerData.instance.gold;
        PlayerData.instance.gold -= totalprc;
        Inventory.instance.PlaceItem(item, setQty);
        Shop.instance.gold.text = PlayerData.instance.gold.ToString();
        view.SetActive(false);
    }

    public void ConfirmSend()
    {
        int gold = PlayerData.instance.gold;
        PlayerData.instance.gold -= totalprc;
        InventoryBox.instance.PlaceItem(item, setQty);
        Shop.instance.gold.text = PlayerData.instance.gold.ToString();
        view.SetActive(false);
    }

    public void ConfirmSell() { }
}
