using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public Inventory inventory;
    public Quest quest;
    public InventoryBox inventoryBox;
    public Shop shop;
    public PlayerData playerData;
    public InputSetup inputSetup;

    public bool isOpen;
    public bool cantOpenMenu;

    [Header("Input Settings")]
    public Vector3 inputAxis;
    public bool pointerInputHold;
    public bool buttonInputHold;

    [Header("Menu Pointer")]
    public int menuNumber;//0 inventory, 1 quest    
    public Color32 normalColor;
    public Color32 markColor;
    public Color32 selectedColor;
    public GameObject[] menuPointer;
    public GameObject[] itemBoxPointer;
    public GameObject[] shopPointer;

    public void Start()
    {
        cantOpenMenu = true;
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        quest = GameObject.FindGameObjectWithTag("Quest").GetComponent<Quest>();
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();
        shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>();
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        pointerInputHold = false;
        buttonInputHold = false;

        if (playerData.DEVELOPERMODE == true)
        {
            cantOpenMenu = false;
        }

        ResetMenu();
    }

    private void Update()
    {
        //press start
        if(!shop.inShop)
            OpenCloseMenu();

        if ((GameStatus.IsPaused && isOpen) || shop.inShop)
        {
            GetInputAxis();
            SelectMenu();
            PointerInput();
            ButtonInput();
        }
    }

    void OpenCloseMenu() {
        if (!GameStatus.isTalking && !cantOpenMenu)
        {
            if (Input.GetKeyDown(inputSetup.openGameMenu))
            {
                if (inventory.inventoryView.activeSelf == false && quest.questView.activeSelf == false)
                {
                    OpenMenu();
                }
                //gabisa pake else, soalnya kalo buka item box, inventoryview.activeselfnya nyala
                else if (inventory.inventoryView.activeSelf == true && quest.questView.activeSelf == true && inventory.isSwapping == false)
                {
                    CloseMenu();
                }
            }
            if (Input.GetKeyDown(inputSetup.back))
            {                
                if (inventory.inventoryView.activeSelf == true && quest.questView.activeSelf == true && inventory.isSwapping == false)
                {
                    CloseMenu();
                }
                else if (inventory.inventoryView.activeSelf == true && quest.questView.activeSelf == true && inventory.isSwapping == true)
                {
                    inventory.ResetInventorySwap();
                }
            }
        }
    }

    void PointerInput() {
        if (!pointerInputHold && 
            (inputAxis.y == 1 || inputAxis.y == -1 || inputAxis.x == 1 || inputAxis.x == -1))
        {
            if (!inventory.isSettingQuantity && !inventoryBox.isSettingQuantity)
            {
                StartCoroutine(PointerInputHold());
                ApplyNavigation();
            }
            else if (inventory.isSettingQuantity)
            {
                StartCoroutine(PointerInputHold());
                if (inventory.isSettingQuantity)
                {
                    if (inputAxis.x == 1)
                        inventory.IncreaseQuantityToPut();
                    if (inputAxis.x == -1)
                        inventory.DecreaseQuantityToPut();
                }
                else if (inventoryBox.isSettingQuantity)
                {
                    if (inputAxis.x == 1)
                        inventoryBox.IncreaseQuantityToPut();
                    if (inputAxis.x == -1)
                        inventoryBox.DecreaseQuantityToPut();
                }
            }
        }
    }

    void ButtonInput() {
        if (!buttonInputHold)
        {
            if (shop.inShop)
                ButtonOnShop();

            else if (inventoryBox.isItemBoxOpened == false)
                ButtonOnStartMenu();

            else if (inventoryBox.isItemBoxOpened == true)
                ButtonOnInventoryBox();
        }
    }

    void ButtonOnStartMenu() {
        if (menuNumber == 0) //inventory
        {
            inventory.InventorySwapping();
        }
        if (menuNumber == 1) // quest
        {

        }
    }

    void ButtonOnShop() {

    }

    void ButtonOnInventoryBox() {
        if (menuNumber == 0) //inventory
        {
            Debug.Log("in0");
            if (inventory.isSettingQuantity == false)
            {
                inventory.InventorySwapping();
            }
            inventory.PutInventory();
        }
        if (menuNumber == 1)//itemBox
        {
            Debug.Log("in1");
            if (inventoryBox.isSettingQuantity == false)
            {
                inventoryBox.InventoryBoxSwapping();
            }
            inventoryBox.PutInventory();
        }

        if (Input.GetKeyDown(inputSetup.back))
        {
            if (inventoryBox.isItemBoxOpened == true)
            {
                if (inventory.isSwapping == true)
                {
                    inventory.ResetInventorySwap();
                }
                else if (inventoryBox.isSwapping == true)
                {
                    inventoryBox.ResetInventoryBoxSwap();
                }
                else if (inventory.isSettingQuantity == true)
                {
                    inventory.slider.SetActive(false);
                    inventory.isSettingQuantity = false;
                }
                else if (inventoryBox.isSettingQuantity == true)
                {
                    inventoryBox.slider.SetActive(false);
                    inventoryBox.isSettingQuantity = false;
                }
                else
                {
                    CloseInventoryBoxMenu();
                }
            }
        }
    }

    void OpenMenu() {
        playerData.healthIndicator.SetActive(false);
        inventory.inventoryView.SetActive(true);
        quest.questView.SetActive(true);
        quest.RefreshQuest();
        isOpen = true;
        ResetMenu();
        GameStatus.PauseGame();
    }

    void CloseMenu()
    {
        playerData.healthIndicator.SetActive(true);
        inventory.inventoryView.SetActive(false);
        quest.questView.SetActive(false);
        isOpen = false;
        ResetMenu();
        InputHolder.isInputHolded = true;
        GameStatus.ResumeGame();
    }

    public void OpenInventoryBoxMenu() {
        playerData.healthIndicator.SetActive(false);
        inventory.inventoryView.SetActive(true);
        inventory.MarkInventory();
        inventoryBox.inventoryBoxView.SetActive(true);
        inventoryBox.MarkInventoryBox();
        inventoryBox.isItemBoxOpened = true;
        StartCoroutine("ButtonInputHold");
        isOpen = true;
        ResetMenu();
        GameStatus.PauseGame();
        GameStatus.PauseMove();
    }

    void CloseInventoryBoxMenu()
    {
        playerData.healthIndicator.SetActive(true);
        inventory.inventoryView.SetActive(false);
        inventoryBox.inventoryBoxView.SetActive(false);
        inventoryBox.isItemBoxOpened = false;
        isOpen = false;
        ResetMenu();
        InputHolder.isInputHolded = true;
        GameStatus.ResumeGame();
        GameStatus.ResumeMove();
    }

    void SelectMenu()
    {
        if (shop.inShop)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                if (menuNumber < shopPointer.Length - 1)
                    menuNumber++;
                ResetPointer(shopPointer);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (menuNumber > 0)
                    menuNumber--;
                ResetPointer(shopPointer);
            }
        }
        else {
            if (inventoryBox.isItemBoxOpened == false && inventory.isSwapping == false)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button5))
                {
                    if (menuNumber < menuPointer.Length - 1)
                        menuNumber++;
                    ResetPointer(menuPointer);
                }
                if (Input.GetKeyDown(KeyCode.Joystick1Button4))
                {
                    if (menuNumber > 0)
                        menuNumber--;
                    ResetPointer(menuPointer);
                }
            }
            else if (inventoryBox.isItemBoxOpened && !inventory.isSwapping && !inventoryBox.isSwapping &&
                !inventory.isSettingQuantity && !inventoryBox.isSettingQuantity)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button5))
                {
                    if (menuNumber < itemBoxPointer.Length - 1)
                        menuNumber++;
                    ResetPointer(itemBoxPointer);
                }
                if (Input.GetKeyDown(KeyCode.Joystick1Button4))
                {
                    if (menuNumber > 0)
                        menuNumber--;
                    ResetPointer(itemBoxPointer);
                }
            }
        }
    }

    void ApplyNavigation()
    {
        if (shop.inShop)
        {
            if (menuNumber == 0) //inven
                inventory.InventorySelection();
            else if (menuNumber == 1) //itembox
                shop.ShopSelection();
        }
        else {
            if (!inventoryBox.isItemBoxOpened)
            {
                if (menuNumber == 0) //inven
                    inventory.InventorySelection();
                else if (menuNumber == 1) //quest
                    quest.QuestSelection();
            }
            else if (inventoryBox.isItemBoxOpened)
            {
                if (menuNumber == 0) //inven
                    inventory.InventorySelection();
                else if (menuNumber == 1) //itembox
                    inventoryBox.InventoryBoxSelection();
            }
        }
    }

    #region UTIL
    void GetInputAxis()
    {
        inputAxis.y = Input.GetAxisRaw("D-Pad Up");
        inputAxis.x = Input.GetAxisRaw("D-Pad Right");
    }
    public IEnumerator PointerInputHold()
    {
        pointerInputHold = true;
        yield return new WaitForSeconds(0.15f);
        pointerInputHold = false;
    }
    public IEnumerator ButtonInputHold()
    {
        buttonInputHold = true;
        yield return new WaitForSeconds(0.15f);
        buttonInputHold = false;
    }

    public void ResetPointer(GameObject[] pointer)
    {
        for (int i = 0; i < pointer.Length; i++)
        {
            pointer[i].SetActive(false);
        }
        pointer[menuNumber].SetActive(true);
    }
    public void ResetMenu()
    {
        menuNumber = 0;
        ResetPointer(menuPointer);
        ResetPointer(shopPointer);
        ResetPointer(itemBoxPointer);

        //reset inventory pointer
        inventory.inventoryColumnIndex = 0;
        inventory.inventoryRowIndex = 0;
        inventory.MarkInventory();
        inventoryBox.MarkInventoryBox();

        //reset quest pointer
        quest.questIndex = 0;
        quest.ScrollQuest();
        quest.MarkQuest();
        quest.RefreshQuest();
    }
    #endregion

}
