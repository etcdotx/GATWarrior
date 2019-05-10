using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public PlayerData playerData;
    public InputSetup inputSetup;
    public GameMenuManager gameMenuManager;
    public InventoryBox inventoryBox;
    public Inventory inventory;

    [Header("UI Settings")]
    public int shopRow;
    public int shopRowIndex;
    public bool inShop;

    public GameObject shopUI;
    public GameObject shopView;
    public GameObject shopViewPort;
    public GameObject shopContent;

    public GameObject shopIndicatorPrefab;
    public List<ShopIndicator> shopIndicator = new List<ShopIndicator>();
    public List<Item> shopItem = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        gameMenuManager = GameObject.FindGameObjectWithTag("GameMenuManager").GetComponent<GameMenuManager>();
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();

        shopUI = GameObject.FindGameObjectWithTag("ShopUI");
        shopView = shopUI.transform.Find("ShopView").gameObject;
        shopViewPort = shopView.transform.Find("ShopViewPort").gameObject;
        shopContent = shopViewPort.transform.Find("ShopContent").gameObject;

        shopView.SetActive(false);
        AddShopIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        if (inShop)
        {
            if (Input.GetKeyDown(inputSetup.back))
            {
                CloseShop();
            }
        }
    }

    public void ShopSelection() {
        if (gameMenuManager.inputAxis.y == -1) // kebawah
        {
            if (shopRowIndex < shopRow - 1)
            {
                shopRowIndex++;
            }
            else
            {
                shopRowIndex = 0;
            }
        }
        else if (gameMenuManager.inputAxis.y == 1) // keatas
        {
            if (shopRowIndex > 0)
            {
                shopRowIndex--;
            }
            else
            {
                shopRowIndex = shopRow - 1;
            }
        }
        MarkShopSelection();
    }

    void MarkShopSelection() {
        Debug.Log(shopIndicator[shopRowIndex].itemNameText.text);
    }

    public void OpenShop() {
        inShop = true;
        playerData.healthIndicator.SetActive(false);
        inventory.inventoryView.SetActive(true);
        shopView.SetActive(true);
        GameStatus.PauseGame();
    }

    public void CloseShop()
    {
        GameStatus.ResumeGame();
        inShop = false;
        playerData.healthIndicator.SetActive(true);
        inventory.inventoryView.SetActive(false);
        shopView.SetActive(false);
    }

    void AddShopIndicator() {
        for (int i = 0; i < shopItem.Count; i++)
        {
            Instantiate(shopIndicatorPrefab, shopContent.transform);
            shopIndicator.Add(shopContent.transform.GetChild(i).GetComponent<ShopIndicator>());
            shopIndicator[i].itemNameText.text = shopItem[i].itemName;
            shopIndicator[i].priceText.text = shopItem[i].price.ToString();
            shopIndicator[i].image.sprite = shopItem[i].itemImage;
            shopRow++;
        }
    }
}
