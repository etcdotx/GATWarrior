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

        //if (PlayerData.instance.DEVELOPERMODE == true)
        //{
        //    cantOpenMenu = false;
        //}
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
            if (Input.GetKeyDown(InputSetup.instance.start)) //tombol start
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
        }
        else if (menuState == MenuState.startMenu)
        {
        }
            
        else if (menuState == MenuState.inventoryBoxMenu)
        {
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
        }
        if (menuNumber == 1) // quest
        {

        }

        if (Input.GetKeyDown(InputSetup.instance.back)) //tombol back
        {
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
        }
        if (menuNumber == 1)//itemBox
        {
        }

        if (Input.GetKeyDown(InputSetup.instance.back))
        {
            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
            StartCoroutine(ButtonInputHold());
            if (Inventory.instance.isSwapping)
            {
            }
            else if (InventoryBox.instance.isSwapping)
            {
                InventoryBox.instance.CancelSwap();
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
        else if (menuState == MenuState.startMenu && !Inventory.instance.isSwapping)
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
        else if (menuState == MenuState.inventoryBoxMenu && !InventoryBox.instance.isSwapping)
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
        }
        else if (menuState == MenuState.inventoryBoxMenu)
        {
            ResetPointer(itemBoxPointer);
            ResetInventoryBox();
        }
        else if (menuState == MenuState.shopMenu)
        {
            ResetPointer(shopPointer);
            ResetInventoryBox();
        }
    }

    void ResetInventoryBox()
    {
    }
    #endregion

}
