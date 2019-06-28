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
    public int minRange;
    public int maxRange;
    public float padding;
    public bool inShop;

    public GameObject shopView;
    public GameObject shopViewPort;
    public GameObject shopContent;

    public GameObject shopIndicatorPrefab;
    public List<ShopIndicator> shopIndicator = new List<ShopIndicator>();
    public List<Item> shopItem = new List<Item>();

    [Header("Gold")]
    public TextMeshProUGUI gold;

    [Header("Shop Description")]
    public ShopDescription shopDescription;

    [Header("Buy")]
    public BuyConfirmation buyConfirmation;
    public SendToBoxConfirmation sendToBoxConfirmation;
    public bool isBuying;
    public bool isSending;

    private void Awake()
    {
        //singleton
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        shopView = transform.GetChild(0).Find("ShopView").gameObject;
        shopViewPort = shopView.transform.Find("ShopViewPort").gameObject;
        shopDescription = shopView.transform.Find("ShopDescription").GetComponent<ShopDescription>();
        buyConfirmation = shopView.transform.Find("BuyConfirmation").GetComponent<BuyConfirmation>();
        sendToBoxConfirmation = shopView.transform.Find("SendToBoxConfirmation").GetComponent<SendToBoxConfirmation>();
        gold = shopView.transform.Find("GoldIndicator").transform.Find("Gold").GetComponent<TextMeshProUGUI>();
        shopContent = shopViewPort.transform.Find("ShopContent").gameObject;
        gridLayoutGroup = shopContent.GetComponent<GridLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        shopView.SetActive(false);
        buyConfirmation.gameObject.SetActive(false);
        sendToBoxConfirmation.gameObject.SetActive(false);
        maxRow = gridLayoutGroup.constraintCount;
        padding = gridLayoutGroup.spacing.x;
        isBuying = false;
    }

    public void BuySelectedItem()
    {
        for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
        {
            if (PlayerData.instance.inventoryItem[i].id == shopIndicator[shopRowIndex].item.id)
            {
                if (PlayerData.instance.inventoryItem[i].quantity < PlayerData.instance.inventoryItem[i].maxQuantityOnInventory)
                {
                    if (PlayerData.instance.gold >= PlayerData.instance.inventoryItem[i].price)
                    {
                        isBuying = true;
                        buyConfirmation.gameObject.SetActive(true);
                        buyConfirmation.InitiateConfirmation(shopIndicator[shopRowIndex].item);
                        return;
                    }
                    else {
                        Debug.Log("not enough money");
                        return;
                    }
                }
                else
                {
                    Debug.Log("item full");
                    return;
                }
            }
        }

        if (PlayerData.instance.gold >= shopIndicator[shopRowIndex].item.price)
        {
            isBuying = true;
            buyConfirmation.gameObject.SetActive(true);
            buyConfirmation.InitiateConfirmation(shopIndicator[shopRowIndex].item);
        }
        else
        {
            Debug.Log("not enough money");
        }
    }

    public void SendSelectedItem()
    {
        if (PlayerData.instance.gold >= shopIndicator[shopRowIndex].item.price)
        {
            isSending = true;
            sendToBoxConfirmation.gameObject.SetActive(true);
            sendToBoxConfirmation.InitiateConfirmation(shopIndicator[shopRowIndex].item);
        }
        else
        {
            Debug.Log("not enough money");
        }
    }

    public void ManageQuantity() {
        if (isBuying)
        {
            if (GameMenuManager.instance.inputAxis.y == -1) // kebawah
            {
                buyConfirmation.DecreaseQuantity();
            }
            else if (GameMenuManager.instance.inputAxis.y == 1)
            {
                buyConfirmation.IncreaseQuantity();
            }
            if (GameMenuManager.instance.inputAxis.x == -1) // kebawah
            {
                buyConfirmation.SetMinQuantity();
            }
            else if (GameMenuManager.instance.inputAxis.x == 1)
            {
                buyConfirmation.SetMaxQuantity();
            }
        }
        else if (isSending)
        {
            if (GameMenuManager.instance.inputAxis.y == -1) // kebawah
            {
                sendToBoxConfirmation.DecreaseQuantity();
            }
            else if (GameMenuManager.instance.inputAxis.y == 1)
            {
                sendToBoxConfirmation.IncreaseQuantity();
            }
            if (GameMenuManager.instance.inputAxis.x == -1) // kebawah
            {
                sendToBoxConfirmation.SetMinQuantity();
            }
            else if (GameMenuManager.instance.inputAxis.x == 1)
            {
                sendToBoxConfirmation.SetMaxQuantity();
            }
        }
    }

    public void ConfirmBuy() {
        buyConfirmation.ConfirmBuy();
        gold.text = PlayerData.instance.gold.ToString();
        buyConfirmation.gameObject.SetActive(false);
        isBuying = false;
        shopDescription.RefreshDescription();
    }

    public void ConfirmSend() {
        sendToBoxConfirmation.ConfirmSend();
        gold.text = PlayerData.instance.gold.ToString();
        sendToBoxConfirmation.gameObject.SetActive(false);
        isSending = false;
        shopDescription.RefreshDescription();
    }

    public void CancelBuy() {
        isBuying = false;
        isSending = false;
        buyConfirmation.gameObject.SetActive(false);
        sendToBoxConfirmation.gameObject.SetActive(false);
    }

    public void ShopSelection()
    {
        if (GameMenuManager.instance.inputAxis.y == -1) // kebawah
        {
            if (shopRowIndex < maxRange-1)
            {
                shopRowIndex++;
            }
            else
            {
                shopRowIndex = minRange;
            }
        }
        else if (GameMenuManager.instance.inputAxis.y == 1) // keatas
        {
            if (shopRowIndex > minRange)
            {
                shopRowIndex--;
            }
            else
            {
                shopRowIndex = maxRange-1;
            }
        }
        if (GameMenuManager.instance.inputAxis.x == -1) // kekiri
        {
            if (maxShopList > 1)
            {
                if (shopList > 0)
                {
                    gridLayoutGroup.padding.left += (int)padding;
                    //shopContent.transform.position += new Vector3(-padding, 0, 0);
                    shopList--;
                    minRange -= maxRow;
                    maxRange = minRange + maxRow;
                    if (maxRange > shopItem.Count)
                        maxRange = shopItem.Count;
                    shopRowIndex = minRange;
                }
                else
                {
                    gridLayoutGroup.padding.left = -(int)padding * (maxShopList - 1);
                    shopList = maxShopList - 1;
                    minRange = shopList * maxRow;
                    maxRange = shopItem.Count;
                    shopRowIndex = minRange;
                }
            }
        }
        else if (GameMenuManager.instance.inputAxis.x == 1) //kekanan
        {
            if (maxShopList > 1)
            {
                if (shopList < maxShopList - 1)
                {
                    gridLayoutGroup.padding.left += -(int)padding;
                    shopList++;
                    minRange += maxRow;
                    maxRange += maxRow;
                    if (maxRange > shopItem.Count)
                        maxRange = shopItem.Count;
                    shopRowIndex = minRange;
                }
                else
                {
                    gridLayoutGroup.padding.left = 0;
                    shopList = 0;
                    minRange = 0;
                    maxRange = maxRow;
                    if (maxRange > shopItem.Count)
                        maxRange = shopItem.Count;
                    shopRowIndex = minRange;
                }
            }
        }
        MarkShopSelection();
    }

    void MarkShopSelection() {
        for (int i = 0; i < shopIndicator.Count; i++)
        {
            shopIndicator[i].selectedIndicator.SetActive(false);
        }
        shopIndicator[shopRowIndex].selectedIndicator.SetActive(true);

        //gua gatau kenapa harus gini biar bisa geser ke next tab
        shopContent.SetActive(false);
        shopContent.SetActive(true);
        //gua gatau kenapa harus gini biar bisa geser ke next tab

        shopDescription.item = shopIndicator[shopRowIndex].item;
        shopDescription.RefreshDescription();
    }

    public void OpenShop(NPC target)
    {
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
        ResetRange();        
        AddShopIndicator();

        GameMenuManager.instance.menuState = GameMenuManager.MenuState.shopMenu;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        inShop = true;
        PlayerStatus.instance.healthIndicator.SetActive(false);
        InventoryBox.instance.inventoryBoxView.SetActive(true);
        InventoryBox.instance.MarkInventoryBox();
        shopView.SetActive(true);
    }

    public void CloseShop()
    {
        RemoveShopIndicator();
        GameMenuManager.instance.menuState = GameMenuManager.MenuState.noMenu;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        inShop = false;
        PlayerStatus.instance.healthIndicator.SetActive(true);
        InventoryBox.instance.inventoryBoxView.SetActive(false);
        shopView.SetActive(false);
    }

    void ResetRange() {
        maxShopList = (shopItem.Count / maxRow) + 1;
        minRange = 0;
        maxRange = maxRow;
        if (maxRange > shopItem.Count)
            maxRange = shopItem.Count;
        shopRowIndex = 0;
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
        MarkShopSelection();
    }

    void RemoveShopIndicator()
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
