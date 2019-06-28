using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager instance;
    
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
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        cantOpenMenu = true;

        pointerInputHold = false;
        buttonInputHold = false;

        if (PlayerData.instance.DEVELOPERMODE == true)
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
        if (!cantOpenMenu)
        {
            if (Input.GetKeyDown(InputSetup.instance.openGameMenu)) //tombol start
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.OpenInventoryClip);
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
            if (!Inventory.instance.isSettingQuantity && !InventoryBox.instance.isSettingQuantity)
            {
                StartCoroutine(PointerInputHold());
                ApplyNavigation();
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
            }
            else
            {
                StartCoroutine(PointerInputHold());
                if (Inventory.instance.isSettingQuantity)
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
                    if (inputAxis.x == 1)
                        Inventory.instance.IncreaseQuantityToPut();
                    if (inputAxis.x == -1)
                        Inventory.instance.DecreaseQuantityToPut();
                }
                else if (InventoryBox.instance.isSettingQuantity)
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
                    if (inputAxis.x == 1)
                        InventoryBox.instance.IncreaseQuantityToPut();
                    if (inputAxis.x == -1)
                        InventoryBox.instance.DecreaseQuantityToPut();
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
                if (!Shop.instance.isBuying && !Shop.instance.isSending)
                    Shop.instance.ShopSelection();
                else
                    Shop.instance.ManageQuantity();

            }
            else if (menuNumber == 1) //inven
                InventoryBox.instance.InventoryBoxSelection();
        }
        else if (menuState == MenuState.startMenu)
        {
            if (menuNumber == 0) //inven
                Inventory.instance.InventorySelection();
        }
            
        else if (menuState == MenuState.inventoryBoxMenu)
        {
            if (menuNumber == 0) //inven
                Inventory.instance.InventorySelection();
            else if (menuNumber == 1) //itembox
                InventoryBox.instance.InventoryBoxSelection();
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
            if (Input.GetKeyDown(InputSetup.instance.select))
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                StartCoroutine(ButtonInputHold());
                Inventory.instance.InventorySwapping();
            }
        }
        if (menuNumber == 1) // quest
        {

        }

        if (Input.GetKeyDown(InputSetup.instance.back)) //tombol back
        {
            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
            StartCoroutine(ButtonInputHold());
            if (Inventory.instance.isSwapping)
                Inventory.instance.ResetInventorySwap();
            else
                CloseMenu();
        }
    }

    void ButtonOnShop() {
        if (menuNumber==0)//shop
        {
            if (Input.GetKeyDown(InputSetup.instance.select))
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                StartCoroutine(ButtonInputHold());
                if (!Shop.instance.isBuying && !Shop.instance.isSending)
                    Shop.instance.BuySelectedItem();
                else if (Shop.instance.isBuying)
                    Shop.instance.ConfirmBuy();
                else if (Shop.instance.isSending)
                    Shop.instance.ConfirmSend();
            }
            if (Input.GetKeyDown(InputSetup.instance.back)) //tombol back
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                StartCoroutine(ButtonInputHold());
                if (!Shop.instance.isBuying && !Shop.instance.isSending)
                    Shop.instance.CloseShop();
                else if(Shop.instance.isBuying || Shop.instance.isSending)
                    Shop.instance.CancelBuy();
            }
            if (Input.GetKeyDown(InputSetup.instance.sendToBox))
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                StartCoroutine(ButtonInputHold());
                if (!Shop.instance.isBuying && !Shop.instance.isSending)
                    Shop.instance.SendSelectedItem();
            }
        }
        if (menuNumber == 1)
        {
            if (Input.GetKeyDown(InputSetup.instance.back)) //tombol back
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                StartCoroutine(ButtonInputHold());
                Shop.instance.CloseShop();
            }
        }
    }

    void ButtonOnInventoryBox() {
        if (menuNumber == 0) //inventory
        {
            if (!Inventory.instance.isSettingQuantity)
            {
                if (Input.GetKeyDown(InputSetup.instance.select))
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    Inventory.instance.InventorySwapping();
                }
                else if (Input.GetKeyDown(InputSetup.instance.putInventory))
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    Inventory.instance.PutIntoInventoryBox();
                }
            }
            else if (Inventory.instance.isSettingQuantity)
            {
                if (Input.GetKeyDown(InputSetup.instance.select))
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    Inventory.instance.PutIntoInventoryBox();
                }
            }
        }
        if (menuNumber == 1)//itemBox
        {
            if (!InventoryBox.instance.isSettingQuantity)
            {
                if (Input.GetKeyDown(InputSetup.instance.select))
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    InventoryBox.instance.InventoryBoxSwapping();
                }
                else if (Input.GetKeyDown(InputSetup.instance.putInventory))
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    InventoryBox.instance.PutIntoInventory();
                }
            }
            else if(InventoryBox.instance.isSettingQuantity)
            {                
                if (Input.GetKeyDown(InputSetup.instance.select))
                {
                    SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                    StartCoroutine(ButtonInputHold());
                    InventoryBox.instance.PutIntoInventory();
                }
            }
        }

        if (Input.GetKeyDown(InputSetup.instance.back))
        {
            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
            StartCoroutine(ButtonInputHold());
            if (Inventory.instance.isSwapping)
            {
                Inventory.instance.ResetInventorySwap();
            }
            else if (InventoryBox.instance.isSwapping)
            {
                InventoryBox.instance.ResetInventoryBoxSwap();
            }
            else if (Inventory.instance.isSettingQuantity)
            {
                Inventory.instance.slider.SetActive(false);
                Inventory.instance.isSettingQuantity = false;
            }
            else if (InventoryBox.instance.isSettingQuantity)
            {
                InventoryBox.instance.slider.SetActive(false);
                InventoryBox.instance.isSettingQuantity = false;
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
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (menuNumber > 0)
                    menuNumber--;
                ResetPointer(shopPointer);
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
            }
        }
        else if (menuState == MenuState.startMenu && !Inventory.instance.isSwapping && !Inventory.instance.isSettingQuantity)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                if (menuNumber < menuPointer.Length - 1)
                    menuNumber++;
                ResetPointer(menuPointer);
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (menuNumber > 0)
                    menuNumber--;
                ResetPointer(menuPointer);
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
            }
        }
        else if (menuState == MenuState.inventoryBoxMenu && !InventoryBox.instance.isSwapping && !InventoryBox.instance.isSettingQuantity)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                if (menuNumber < itemBoxPointer.Length - 1)
                    menuNumber++;
                ResetPointer(itemBoxPointer);
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (menuNumber > 0)
                    menuNumber--;
                ResetPointer(itemBoxPointer);
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
            }
        }

    }

    void OpenMenu()
    {
        menuState = MenuState.startMenu;
        PlayerStatus.instance.healthIndicator.SetActive(false);
        Inventory.instance.inventoryView.SetActive(true);
        Quest.instance.questView.SetActive(true);
        ResetMenu();
        Debug.Log("in");
    }

    public void OpenInventoryBoxMenu()
    {
        menuState = MenuState.inventoryBoxMenu;
        PlayerStatus.instance.healthIndicator.SetActive(false);
        Inventory.instance.inventoryView.SetActive(true);
        InventoryBox.instance.inventoryBoxView.SetActive(true);
        StartCoroutine("ButtonInputHold");
        ResetMenu();
    }

    void CloseMenu()
    {
        menuState = MenuState.noMenu;
        PlayerStatus.instance.healthIndicator.SetActive(true);
        Inventory.instance.inventoryView.SetActive(false);
        Quest.instance.questView.SetActive(false);
        ResetMenu();
    }

    void CloseInventoryBoxMenu()
    {
        menuState = MenuState.noMenu;
        PlayerStatus.instance.healthIndicator.SetActive(true);
        Inventory.instance.inventoryView.SetActive(false);
        InventoryBox.instance.inventoryBoxView.SetActive(false);
        ResetMenu();
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
        Inventory.instance.inventoryColumnIndex = 0;
        Inventory.instance.inventoryRowIndex = 0;
        Inventory.instance.MarkInventory();
    }

    void ResetInventoryBox()
    {
        InventoryBox.instance.inventoryBoxColumnIndex = 0;
        InventoryBox.instance.inventoryBoxRowIndex = 0;
        InventoryBox.instance.MarkInventoryBox();
    }
    #endregion

}
