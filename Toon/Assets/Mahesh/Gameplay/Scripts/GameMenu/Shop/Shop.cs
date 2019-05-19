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
    public GridLayoutGroup gridLayoutGroup;
    public int maxRow;
    public int shopRowIndex;
    public int shopList;
    public int maxShopList;
    public int minRange;
    public int maxRange;
    public float padding;
    public bool inShop;

    public GameObject shopUI;
    public GameObject shopView;
    public GameObject shopViewPort;
    public GameObject shopContent;

    public GameObject shopIndicatorPrefab;
    public List<ShopIndicator> shopIndicator = new List<ShopIndicator>();
    public List<Item> shopItem = new List<Item>();

    [Header("Shop Description")]
    public ShopDescription shopDescription;

    private void Awake()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        gameMenuManager = GameObject.FindGameObjectWithTag("GameMenuManager").GetComponent<GameMenuManager>();
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();

        shopUI = GameObject.FindGameObjectWithTag("ShopUI");
        shopView = shopUI.transform.Find("ShopView").gameObject;
        shopViewPort = shopView.transform.Find("ShopViewPort").gameObject;
        shopDescription = shopView.transform.Find("ShopDescription").GetComponent<ShopDescription>();
        shopContent = shopViewPort.transform.Find("ShopContent").gameObject;
        gridLayoutGroup = shopContent.GetComponent<GridLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        shopView.SetActive(false);
        maxRow = gridLayoutGroup.constraintCount;
        padding = gridLayoutGroup.spacing.x;
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

    public void ShopSelection()
    {
        if (gameMenuManager.inputAxis.y == -1) // kebawah
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
        else if (gameMenuManager.inputAxis.y == 1) // keatas
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
        if (gameMenuManager.inputAxis.x == -1) // kekiri
        {
            if (shopList > 0)
            {
                gridLayoutGroup.padding.left += (int)padding;
                //shopContent.transform.position += new Vector3(-padding, 0, 0);
                shopList--;
                minRange -= maxRow;
                maxRange = minRange+ maxRow;
                if (maxRange > shopItem.Count)
                    maxRange = shopItem.Count;
                shopRowIndex = minRange;
            }
            else
            {
                gridLayoutGroup.padding.left = -(int)padding * (maxShopList-1);
                shopList = maxShopList-1;
                minRange = shopList * maxRow;
                maxRange = shopItem.Count;
                shopRowIndex = minRange;
            }
        }
        else if (gameMenuManager.inputAxis.x == 1) //kekanan
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
        MarkShopSelection();
    }

    void MarkShopSelection() {
        for (int i = 0; i < shopIndicator.Count; i++)
        {
            shopIndicator[i].selectedIndicator.SetActive(false);
        }
        shopIndicator[shopRowIndex].selectedIndicator.SetActive(true);
        shopContent.SetActive(false);
        shopContent.SetActive(true);
        shopDescription.item = shopIndicator[shopRowIndex].item;
        shopDescription.RefreshDescription();

    }

    public void OpenShop(NPC target)
    {
        shopIndicator.Clear();
        shopItem.Clear();
        for (int i = 0; i < target.shopItem.Count; i++)
        {
            Item temp = target.shopItem[i];
            Item temp2 = new Item(temp.id, temp.itemImage, temp.itemName,
                temp.description, temp.price, temp.isUsable, temp.isConsumable, temp.isASingleTool, temp.itemType);
            if (temp.itemType != null)
                if (temp.itemType.ToLower().Equals("seed".ToLower()))
                    temp2.plantID = temp.plantID;
            shopItem.Add(temp2);
        }
        ResetRange();        
        AddShopIndicator();

        gameMenuManager.menuState = GameMenuManager.MenuState.shopMenu;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CharacterMovement cm = player.GetComponent<CharacterMovement>();
        cm.canMove = false;

        inShop = true;
        playerData.healthIndicator.SetActive(false);
        inventoryBox.inventoryBoxView.SetActive(true);
        inventoryBox.MarkInventoryBox();
        shopView.SetActive(true);
        GameStatus.PauseGame();
    }

    public void CloseShop()
    {
        RemoveShopIndicator();
        gameMenuManager.menuState = GameMenuManager.MenuState.noMenu;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CharacterMovement cm = player.GetComponent<CharacterMovement>();
        cm.canMove = true;
        GameStatus.ResumeGame();
        inShop = false;
        playerData.healthIndicator.SetActive(true);
        inventoryBox.inventoryBoxView.SetActive(false);
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
