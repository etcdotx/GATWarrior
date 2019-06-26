using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public Inventory inventory;
    public InventoryBox inventoryBox;
    public Quest quest;
    public Shop shop;
    public PlayerData playerData;
    public PlayerStatus playerStatus;
    public InputSetup inputSetup;
    public SoundList soundList;
    
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

    public enum MenuState {
        noMenu,
        startMenu,
        inventoryBoxMenu,
        shopMenu
    }
    public MenuState menuState;

    private void Awake()
    {
        cantOpenMenu = true;
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        quest = GameObject.FindGameObjectWithTag("Quest").GetComponent<Quest>();
        inventoryBox = GameObject.FindGameObjectWithTag("InventoryBox").GetComponent<InventoryBox>();
        shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>();
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        playerStatus = GameObject.FindGameObjectWithTag("PlayerStatus").GetComponent<PlayerStatus>();
        soundList = GameObject.FindGameObjectWithTag("SoundList").GetComponent<SoundList>();

        pointerInputHold = false;
        buttonInputHold = false;

        if (playerData.DEVELOPERMODE == true)
        {
            cantOpenMenu = false;
        }
    }

    public void Start()
    {
        ResetMenu();
        menuState = MenuState.noMenu;
    }

    private void Update()
    {
        //press start
        OpenCloseMenu();

        if (menuState!=MenuState.noMenu)
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
            if (Input.GetKeyDown(inputSetup.openGameMenu)) //tombol start
            {
                soundList.UIAudioSource.PlayOneShot(soundList.OpenInventoryClip);
                if (menuState == MenuState.noMenu)
                    OpenMenu();

                else if (menuState == MenuState.startMenu)
                    CloseMenu();
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
                soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
            }
            else
            {
                StartCoroutine(PointerInputHold());
                if (inventory.isSettingQuantity)
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
                    if (inputAxis.x == 1)
                        inventory.IncreaseQuantityToPut();
                    if (inputAxis.x == -1)
                        inventory.DecreaseQuantityToPut();
                }
                else if (inventoryBox.isSettingQuantity)
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
                    if (inputAxis.x == 1)
                        inventoryBox.IncreaseQuantityToPut();
                    if (inputAxis.x == -1)
                        inventoryBox.DecreaseQuantityToPut();
                }
            }
        }
    }

    void ApplyNavigation()
    {
        if (menuState == MenuState.shopMenu)
        {
            if (menuNumber == 0) //itembox
            {
                if (!shop.isBuying && !shop.isSending)
                    shop.ShopSelection();
                else
                    shop.ManageQuantity();

            }
            else if (menuNumber == 1) //inven
                inventoryBox.InventoryBoxSelection();
        }
        else if (menuState == MenuState.startMenu)
        {
            if (menuNumber == 0) //inven
                inventory.InventorySelection();
            else if (menuNumber == 1) //quest
                quest.QuestSelection();
        }
            
        else if (menuState == MenuState.inventoryBoxMenu)
        {
            if (menuNumber == 0) //inven
                inventory.InventorySelection();
            else if (menuNumber == 1) //itembox
                inventoryBox.InventoryBoxSelection();
        }
    }

    void ButtonInput() {
        if (!buttonInputHold)
        {
            if (menuState == MenuState.shopMenu)
                ButtonOnShop();

            else if (menuState == MenuState.startMenu)
                ButtonOnStartMenu();

            else if (menuState == MenuState.inventoryBoxMenu)
                ButtonOnInventoryBox();
        }
    }

    void ButtonOnStartMenu() {
        if (menuNumber == 0) //inventory
        {
            if (Input.GetKeyDown(inputSetup.select))
            {
                soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                StartCoroutine(ButtonInputHold());
                inventory.InventorySwapping();
            }
        }
        if (menuNumber == 1) // quest
        {

        }

        if (Input.GetKeyDown(inputSetup.back)) //tombol back
        {
            soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
            StartCoroutine(ButtonInputHold());
            if (inventory.isSwapping)
                inventory.ResetInventorySwap();
            else
                CloseMenu();
        }
    }

    void ButtonOnShop() {
        if (menuNumber==0)//shop
        {
            if (Input.GetKeyDown(inputSetup.select))
            {
                soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                StartCoroutine(ButtonInputHold());
                if (!shop.isBuying && !shop.isSending)
                    shop.BuySelectedItem();
                else if (shop.isBuying)
                    shop.ConfirmBuy();
                else if (shop.isSending)
                    shop.ConfirmSend();
            }
            if (Input.GetKeyDown(inputSetup.back)) //tombol back
            {
                soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                StartCoroutine(ButtonInputHold());
                if (!shop.isBuying && !shop.isSending)
                    shop.CloseShop();
                else if(shop.isBuying || shop.isSending)
                    shop.CancelBuy();
            }
            if (Input.GetKeyDown(inputSetup.sendToBox))
            {
                soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                StartCoroutine(ButtonInputHold());
                if (!shop.isBuying && !shop.isSending)
                    shop.SendSelectedItem();
            }
        }
        if (menuNumber == 1)
        {
            if (Input.GetKeyDown(inputSetup.back)) //tombol back
            {
                soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                StartCoroutine(ButtonInputHold());
                shop.CloseShop();
            }
        }
    }

    void ButtonOnInventoryBox() {
        if (menuNumber == 0) //inventory
        {
            if (!inventory.isSettingQuantity)
            {
                if (Input.GetKeyDown(inputSetup.select))
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    inventory.InventorySwapping();
                }
                else if (Input.GetKeyDown(inputSetup.putInventory))
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    inventory.PutIntoInventoryBox();
                }
            }
            else if (inventory.isSettingQuantity)
            {
                if (Input.GetKeyDown(inputSetup.select))
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    inventory.PutIntoInventoryBox();
                }
            }
        }
        if (menuNumber == 1)//itemBox
        {
            if (!inventoryBox.isSettingQuantity)
            {
                if (Input.GetKeyDown(inputSetup.select))
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    inventoryBox.InventoryBoxSwapping();
                }
                else if (Input.GetKeyDown(inputSetup.putInventory))
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    inventoryBox.PutIntoInventory();
                }
            }
            else if(inventoryBox.isSettingQuantity)
            {                
                if (Input.GetKeyDown(inputSetup.select))
                {
                    soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    inventoryBox.PutIntoInventory();
                }
            }
        }

        if (Input.GetKeyDown(inputSetup.back))
        {
            soundList.UIAudioSource.PlayOneShot(soundList.UISelectClip);
            StartCoroutine(ButtonInputHold());
            if (inventory.isSwapping)
            {
                inventory.ResetInventorySwap();
            }
            else if (inventoryBox.isSwapping)
            {
                inventoryBox.ResetInventoryBoxSwap();
            }
            else if (inventory.isSettingQuantity)
            {
                inventory.slider.SetActive(false);
                inventory.isSettingQuantity = false;
            }
            else if (inventoryBox.isSettingQuantity)
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

    void SelectMenu()
    {
        if (menuState == MenuState.shopMenu)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                if (menuNumber < shopPointer.Length - 1)
                    menuNumber++;
                ResetPointer(shopPointer);
                soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (menuNumber > 0)
                    menuNumber--;
                ResetPointer(shopPointer);
                soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
            }
        }
        else if (menuState == MenuState.startMenu && !inventory.isSwapping && !inventory.isSettingQuantity)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                if (menuNumber < menuPointer.Length - 1)
                    menuNumber++;
                ResetPointer(menuPointer);
                soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (menuNumber > 0)
                    menuNumber--;
                ResetPointer(menuPointer);
                soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
            }
        }
        else if (menuState == MenuState.inventoryBoxMenu && !inventoryBox.isSwapping && !inventoryBox.isSettingQuantity)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                if (menuNumber < itemBoxPointer.Length - 1)
                    menuNumber++;
                ResetPointer(itemBoxPointer);
                soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (menuNumber > 0)
                    menuNumber--;
                ResetPointer(itemBoxPointer);
                soundList.UIAudioSource.PlayOneShot(soundList.UINavClip);
            }
        }

    }

    void OpenMenu()
    {
        menuState = MenuState.startMenu;
        playerStatus.healthIndicator.SetActive(false);
        inventory.inventoryView.SetActive(true);
        quest.questView.SetActive(true);
        quest.RefreshQuest();
        ResetMenu();
        Debug.Log("in");
        GameStatus.PauseGame();
    }

    public void OpenInventoryBoxMenu()
    {
        menuState = MenuState.inventoryBoxMenu;
        playerStatus.healthIndicator.SetActive(false);
        inventory.inventoryView.SetActive(true);
        inventoryBox.inventoryBoxView.SetActive(true);
        StartCoroutine("ButtonInputHold");
        ResetMenu();
        GameStatus.PauseGame();
        GameStatus.PauseMove();
    }

    void CloseMenu()
    {
        menuState = MenuState.noMenu;
        playerStatus.healthIndicator.SetActive(true);
        inventory.inventoryView.SetActive(false);
        quest.questView.SetActive(false);
        ResetMenu();
        InputHolder.isInputHolded = true;
        GameStatus.ResumeGame();
    }

    void CloseInventoryBoxMenu()
    {
        menuState = MenuState.noMenu;
        playerStatus.healthIndicator.SetActive(true);
        inventory.inventoryView.SetActive(false);
        inventoryBox.inventoryBoxView.SetActive(false);
        ResetMenu();
        InputHolder.isInputHolded = true;
        GameStatus.ResumeGame();
        GameStatus.ResumeMove();
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
        if (menuState == MenuState.startMenu)
        {
            ResetPointer(menuPointer);
            ResetInventory();
            ResetQuest();
        }
        else if (menuState == MenuState.inventoryBoxMenu)
        {
            ResetPointer(itemBoxPointer);
            ResetInventory();
            ResetInventoryBox();
        }
        else if (menuState == MenuState.shopMenu)
        {
            ResetPointer(shopPointer);
            ResetInventoryBox();
        }
    }

    void ResetInventory() {
        inventory.inventoryColumnIndex = 0;
        inventory.inventoryRowIndex = 0;
        inventory.MarkInventory();
    }

    void ResetInventoryBox()
    {
        inventoryBox.inventoryBoxColumnIndex = 0;
        inventoryBox.inventoryBoxRowIndex = 0;
        inventoryBox.MarkInventoryBox();
    }

    void ResetQuest() {
        quest.questIndex = 0;
        quest.ScrollQuest();
        quest.MarkQuest();
        quest.RefreshQuest();
    }
    #endregion

}
