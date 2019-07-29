using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    [Header("UI Settings")]
    public GridLayoutGroup gridLayoutGroup;
    public int maxRow;
    public int shopRowIndex;
    public int shopList;
    public int maxShopList;

    public GameObject shopView;
    public GameObject shopViewPort;
    public GameObject shopContent;
    public GameObject shopIndicatorPrefab;
    public List<ShopIndicator> shopIndicator = new List<ShopIndicator>();
    public List<Item> shopItem = new List<Item>();
    public GameObject lastSelected;
    public GameObject buttons;

    [Header("Gold")]
    public TextMeshProUGUI gold;

    [Header("Shop Description")]
    public ItemDescription itemDescription;

    private void Awake()
    {
        //singleton
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        shopView = transform.GetChild(0).Find("ShopView").gameObject;
        shopViewPort = shopView.transform.Find("ShopViewPort").gameObject;
        itemDescription = shopView.transform.Find("ItemDescription").GetComponent<ItemDescription>();
        gold = shopView.transform.Find("GoldIndicator").transform.Find("Gold").GetComponent<TextMeshProUGUI>();
        shopContent = shopViewPort.transform.Find("ShopContent").gameObject;
        buttons = shopView.transform.Find("Buttons").gameObject;
        gridLayoutGroup = shopContent.GetComponent<GridLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        shopView.SetActive(false);
        buttons.SetActive(false);
        maxRow = gridLayoutGroup.constraintCount;
    }

    public void BuySelectedItem(ShopIndicator shopIndicator)
    {
        //check apakah item tersebut di inventory full atau tidak
        for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
        {
            if (PlayerData.instance.inventoryItem[i].id == shopIndicator.item.id)
            {
                if (PlayerData.instance.inventoryItem[i].quantity >= PlayerData.instance.inventoryItem[i].maxQuantityOnInventory)
                {
                    UIManager.instance.warningNotification(shopIndicator.item.itemName, UIManager.WarningState.itemFull);
                    Debug.Log("item full");
                    return;
                }
                else {
                    break;
                }
            }
        }

        //check uangnya
        if (PlayerData.instance.gold >= shopIndicator.item.price)
        {
            lastSelected = shopIndicator.gameObject;
            BuyConfirmation.instance.view.SetActive(true);
            BuyConfirmation.instance.InitiateBuyConfirmation(shopIndicator.item);
        }
        else
        {
            UIManager.instance.warningNotification(shopIndicator.item.itemName, UIManager.WarningState.notEnoughMoney);
            Debug.Log("not enough money");
        }
    }

    public void SendSelectedItem(ShopIndicator shopIndicator)
    {
        if (PlayerData.instance.gold >= shopIndicator.item.price)
        {
            lastSelected = shopIndicator.gameObject;
            BuyConfirmation.instance.view.SetActive(true);
            BuyConfirmation.instance.InitiateSendConfirmation(shopIndicator.item);
        }
        else
        {
            UIManager.instance.warningNotification(shopIndicator.item.itemName, UIManager.WarningState.notEnoughMoney);
            Debug.Log("not enough money");
        }
    }

    public void OpenShop(NPC target)
    {
        UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIState.Shop));
        shopIndicator.Clear();
        shopItem.Clear();
        gold.text = PlayerData.instance.gold.ToString();

        for (int i = 0; i < target.shopItem.Count; i++)
        {
            Item temp = target.shopItem[i];
            Item temp2 = new Item(temp.id, temp.itemImage, temp.itemName,
                temp.description, temp.maxQuantityOnInventory, temp.price, temp.isUsable, 
                temp.isConsumable, temp.isASingleTool, temp.itemType);
            if (temp.itemType != null)
                if (temp.itemType.ToLower().Equals("seed".ToLower()))
                    temp2.plantID = temp.plantID;

            shopItem.Add(temp2);
        }

        AddShopIndicator();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        PlayerStatus.instance.healthIndicator.SetActive(false);
        InventoryBox.instance.inventoryBoxView.SetActive(true);
        //InventoryBox.instance.MarkInventoryBox();
        shopView.SetActive(true);
    }

    void AddShopIndicator()
    {
        for (int i = 0; i < shopItem.Count; i++)
        {
            Instantiate(shopIndicatorPrefab, shopContent.transform);
            shopIndicator.Add(shopContent.transform.GetChild(i).GetComponent<ShopIndicator>());
            shopIndicator[i].item = shopItem[i + (maxRow * shopList)];
            shopIndicator[i].RefreshIndicator();
        }
    }

    public void RemoveShopIndicator()
    {
        shopIndicator.Clear();
        shopItem.Clear();
        gridLayoutGroup.padding.left = 0;
        shopList = 0;
        shopRowIndex = 0;

        for (int i = 0; i < shopContent.transform.childCount; i++)
        {
            GameObject.Destroy(shopContent.transform.GetChild(i).gameObject);
        }
    }
}
