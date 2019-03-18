using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public ItemBox itemBox;
    public Player player;
    public InputSetup inputSetup;
    public bool isOpen;

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

    [Header("Quest Menu Settings")]
    public int questIndex;
    public int questMaxIndex;
    public GameObject questContent;
    public Scrollbar questViewScrollbar;

    [Header("Quest Detail")]
    public GameObject questDetail;
    public TextMeshProUGUI questDescription;

    [Header("Inventory Settings")]
    public int inventoryColumn;
    public int inventoryRow;
    public int inventoryColumnIndex;
    public int inventoryRowIndex;
    public int inventoryViewPortChildCount;
    public GameObject inventoryView;
    public GameObject inventoryViewPort;
    public GameObject[,] inventoryPos; //5 column
    public InventoryItem invenSwap1;
    public InventoryItem invenSwap2;
    public bool isSwapping;
    public Item temporaryItem;
    public GameObject slider;
    public Slider inventoryQuantitySlider;
    public bool isSettingQuantity;

    public void Start()
    {
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        itemBox = GameObject.FindGameObjectWithTag("ItemBoxScript").GetComponent<ItemBox>();
        player = GameObject.Find("Player").GetComponent<Player>();
        pointerInputHold = false;
        buttonInputHold = false;

        //quest
        questIndex = 0;
        questMaxIndex = questContent.transform.childCount - 1;

        questDescription = questDetail.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questDescription.text = "";

        ScrollQuest();
        ResetPointer();

        //inventory
        inventoryView = GameObject.FindGameObjectWithTag("Inventory").transform.Find("InventoryView").gameObject;
        inventoryViewPort = inventoryView.transform.Find("InventoryViewPort").gameObject;
        slider = inventoryView.transform.Find("Slider").gameObject;
        slider.SetActive(false);
        inventoryQuantitySlider = slider.transform.Find("InventoryQuantitySlider").GetComponent<Slider>();

        inventoryViewPortChildCount = inventoryViewPort.transform.childCount;
        inventoryColumn = inventoryViewPort.GetComponent<GridLayoutGroup>().constraintCount;
        inventoryRow = inventoryViewPortChildCount / inventoryColumn;
        inventoryPos = new GameObject[inventoryRow, inventoryColumn];
        inventoryColumnIndex = 0;
        inventoryRowIndex = 0;
        int c = 0;
        for (int i = 0; i < inventoryRow; i++)
        {
            for (int j = 0; j < inventoryColumn; j++)
            {
                inventoryPos[i, j] = inventoryViewPort.transform.GetChild(c).gameObject;
                c++;
            }
        }
    }

    private void Update()
    {
        if (GameStatus.IsPaused == true && isOpen == true)
        {
            GetScrollInput();
            if (pointerInputHold == false)
            {
                SelectMenu();
                if ((inputAxis.y == 1 || inputAxis.y == -1 || inputAxis.x == 1 || inputAxis.x == -1) && isSettingQuantity == false)
                {
                    StartCoroutine(PointerInputHold());
                    ApplyInput();
                }
                else if (( inputAxis.x == 1 || inputAxis.x == -1) && isSettingQuantity == true)
                {
                    StartCoroutine(PointerInputHold());
                    if (inputAxis.x == 1)
                    {
                        inventoryQuantitySlider.value += 1;
                        inventoryQuantitySlider.transform.Find("Text").GetComponent<Text>().text = inventoryQuantitySlider.value.ToString();
                    }
                    if (inputAxis.x == -1)
                    {
                        inventoryQuantitySlider.value -= 1;
                        inventoryQuantitySlider.transform.Find("Text").GetComponent<Text>().text = inventoryQuantitySlider.value.ToString();
                    }
                }
            }
            if (buttonInputHold == false)
            {
                if (itemBox.isItemBoxOpened == false)
                {
                    if (menuNumber == 0) //inventory
                    {
                        InventorySwapping();
                    }
                }
                else
                {
                    if (menuNumber == 0) //inventory
                    {
                        if(isSettingQuantity==false)
                            InventorySwapping();

                        PutInventory();
                    }
                    if (menuNumber == 1)//itemBox
                    {

                    }

                    if (Input.GetKeyDown(inputSetup.back))
                    {
                        if (itemBox.isItemBoxOpened == true)
                        {
                            inventoryView.SetActive(false);
                            itemBox.itemBoxView.SetActive(false);
                            itemBox.isItemBoxOpened = false;
                            isOpen = false;
                            ResetMenu();
                            GameStatus.ResumeGame();
                        }
                    }
                }
            }

        }
    }

    void GetScrollInput()
    {
        inputAxis.y = Input.GetAxisRaw("D-Pad Up");
        inputAxis.x = Input.GetAxisRaw("D-Pad Right");
    }

    void PutInventory()
    {
        if (Input.GetKeyDown(inputSetup.putInventory) && isSettingQuantity==false)
        {
            isSettingQuantity = true;
            SetQuantity();
        }
        if (Input.GetKeyDown(inputSetup.select) && isSettingQuantity == true)
        {
            itemBox.PlaceItem(temporaryItem, (int)inventoryQuantitySlider.value);
            slider.SetActive(false);
            isSettingQuantity = false;
        }
    }

    void SetQuantity()
    {
        for (int i = 0; i < player.item.Count; i++)
        {
            if (inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<InventoryItem>().itemID == player.item[i].id)
            {
                temporaryItem = player.item[i]; //jenis item
                slider.SetActive(true);
                inventoryQuantitySlider.minValue = 1;
                inventoryQuantitySlider.maxValue = player.item[i].quantity;
                inventoryQuantitySlider.transform.Find("Text").GetComponent<Text>().text = inventoryQuantitySlider.value.ToString();
                break;
            }
        }
    }

    void SelectMenu()
    {
        if (itemBox.isItemBoxOpened == false)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                if (menuNumber < menuPointer.Length - 1)
                {
                    menuNumber++;
                }
                ResetPointer();
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (menuNumber > 0)
                {
                    menuNumber--;
                }
                ResetPointer();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                if (menuNumber < itemBoxPointer.Length - 1)
                {
                    menuNumber++;
                }
                ResetPointer();
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (menuNumber > 0)
                {
                    menuNumber--;
                }
                ResetPointer();
            }
        }
    }

    public void ResetPointer()
    {
        if (itemBox.isItemBoxOpened == false)
        {
            for (int i = 0; i < menuPointer.Length; i++)
            {
                menuPointer[i].SetActive(false);
            }
            menuPointer[menuNumber].SetActive(true);
        }
        else
        {
            for (int i = 0; i < itemBoxPointer.Length; i++)
            {
                itemBoxPointer[i].SetActive(false);
            }
            itemBoxPointer[menuNumber].SetActive(true);
        }
    }

    IEnumerator PointerInputHold()
    {
        pointerInputHold = true;
        yield return new WaitForSeconds(0.15f);
        pointerInputHold = false;
    }

    IEnumerator ButtonInputHold()
    {
        buttonInputHold = true;
        yield return new WaitForSeconds(0.15f);
        buttonInputHold = false;
    }

    void ApplyInput()
    {
        if (itemBox.isItemBoxOpened == false)
        {
            if (menuNumber == 0) //inven
            {
                InventorySelection();
            }
            else if (menuNumber == 1) //quest
            {
                QuestSelection();
            }
        }
        else
        {
            if (menuNumber == 0) //inven
            {
                InventorySelection();
            }
            else if (menuNumber == 1) //itembox
            {
                //itembox selection
                itemBox.ItemBoxSelection();
            }
        }
    }

    void InventorySelection()
    {
        if (inputAxis.y == -1) // kebawah
        {
            if (inventoryRowIndex < inventoryRow - 1)
            {
                inventoryRowIndex++;
            }
            else
            {
                inventoryRowIndex = 0;
            }
        }
        else if (inputAxis.y == 1) // keatas
        {
            if (inventoryRowIndex > 0)
            {
                inventoryRowIndex--;
            }
            else
            {
                inventoryRowIndex = inventoryRow - 1;
            }
        }

        if (inputAxis.x == -1) // kekiri
        {
            if (inventoryColumnIndex > 0)
            {
                inventoryColumnIndex--;
            }
            else
            {
                inventoryColumnIndex = inventoryColumn - 1;
            }
        }
        else if (inputAxis.x == 1) //kekanan
        {
            if (inventoryColumnIndex < inventoryColumn - 1)
            {
                inventoryColumnIndex++;
            }
            else
            {
                inventoryColumnIndex = 0;
            }
        }
        MarkInventory();
    }

    void InventorySwapping()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && isSwapping == false)
        {
            invenSwap1 = inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<InventoryItem>();
            invenSwap1.gameObject.GetComponent<Image>().color = selectedColor;
            invenSwap1.isSelected = true;
            isSwapping = true;
            StartCoroutine(ButtonInputHold());
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button0) && isSwapping == true)
        {
            invenSwap2 = inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<InventoryItem>();
            int id1 = invenSwap1.itemID;
            int id2 = invenSwap2.itemID;
            invenSwap1.itemID = id2;
            invenSwap2.itemID = id1;
            player.RefreshItem();
            invenSwap1.isSelected = false;
            invenSwap1.gameObject.GetComponent<Image>().color = normalColor;
            isSwapping = true;
            isSwapping = false;
            StartCoroutine(ButtonInputHold());
        }
    }

    public void MarkInventory()
    {
        try
        {
            for (int i = 0; i < inventoryRow; i++)
            {
                for (int j = 0; j < inventoryColumn; j++)
                {
                    if (inventoryPos[i, j].GetComponent<InventoryItem>().isSelected == false)
                    {
                        inventoryPos[i, j].GetComponent<Image>().color = normalColor;
                    }
                }
            }
            if (inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<InventoryItem>().isSelected == false)
            {
                inventoryPos[inventoryRowIndex, inventoryColumnIndex].GetComponent<Image>().color = markColor;
            }
        }
        catch { }
    }

    void QuestSelection()
    {
        if (inputAxis.y == -1)
        {
            if (questIndex < questMaxIndex)
            {
                questIndex += 1;
            }
            else
            {
                questIndex = 0;
            }
        }
        else if (inputAxis.y == 1)
        {
            if (questIndex > 0)
            {
                questIndex -= 1;
            }
            else
            {
                questIndex = questMaxIndex;
            }
        }
        ScrollQuest();
        MarkQuest();
        RefreshQuest();
    }

    public void ScrollQuest()
    {
        if (questIndex == questMaxIndex)
        {
            questViewScrollbar.value = 0;
        }
        else
        {
            float a = (float)questMaxIndex;
            float b = (float)questIndex;
            float c = a - b;
            float d = c / a;
            questViewScrollbar.value = c / a;
        }
    }

    public void MarkQuest()
    {
        try
        {
            for (int i = 0; i <= questMaxIndex; i++)
            {
                player.questContent.transform.GetChild(i).GetComponent<Image>().color = normalColor;
            }
            player.questContent.transform.GetChild(questIndex).GetComponent<Image>().color = selectedColor;
        }
        catch
        {
        }
    }

    public void RefreshQuest()
    {
        try
        {
            for (int i = 0; i < player.collectionQuest.Count; i++)
            {
                if (player.collectionQuest[i].id == player.questContent.transform.GetChild(questIndex).GetComponent<QuestListUI>().questID)
                {
                    CollectionQuest newCol = player.collectionQuest[i];
                    questDescription.text = newCol.ToString(); //newCol.description + newCol.ToString();
                    break;
                }
                else
                {
                    questDescription.text = "";
                }
            }
        }
        catch
        {
        }
    }

    public void ResetMenu()
    {
        menuNumber = 0;
        ResetPointer();

        //reset inventory pointer
        inventoryColumnIndex = 0;
        inventoryRowIndex = 0;
        MarkInventory();

        //reset quest pointer
        questIndex = 0;
        ScrollQuest();
        MarkQuest();
        RefreshQuest();
    }
}
