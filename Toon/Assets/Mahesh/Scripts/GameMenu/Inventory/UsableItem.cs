using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public static UsableItem instance;

    Animator animator;
    GameObject player;

    [Header("Indicator")]
    //untuk di setactive true/false
    public GameObject usableItemView;

    /// <summary>
    /// viewport dan itemnya
    /// </summary>
    GameObject usableItemViewPort;
    GameObject usableItemContent;
    GameObject[] usableIndicator = new GameObject[3];
    
    //kondisi ketika dia sedang hold LB/L1(pilih item)
    public bool isSelectingItem;
    
    /// <summary>
    /// kondisi ketika dia sedang menggunakan item
    /// 1. dipakai ketika dia sedang pakai item lalu itemnya habis
    /// 2. dipakai ketika refresh usableitem namun tidak sambil menggunakan item
    /// </summary>
    public bool isUsingItem;

    List<Item> usableItemList = new List<Item>();
    public Item selectedItem; //item yang lagi di select
    bool isItemUsable;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        animator = player.GetComponent<Animator>();
        usableItemView = transform.GetChild(0).Find("UsableItemView").gameObject;
        usableItemViewPort = usableItemView.transform.Find("UsableItemViewPort").gameObject;
        usableItemContent = usableItemViewPort.transform.Find("UsableItemContent").gameObject;

        usableIndicator = new GameObject[usableItemContent.transform.childCount];
        for (int i = 0; i < usableIndicator.Length; i++)
        {
            usableIndicator[i] = usableItemContent.transform.GetChild(i).gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isUsingItem = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Joystick1Button4))
        {
            isSelectingItem = true;
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
                SlideItem(true);
                SelectItem();
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
                SlideItem(false);
                SelectItem();
            }
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button4))
        {
            isSelectingItem = false;
        }

        if (!CharacterInput.instance.combatMode)
        {
            if (Input.GetKeyDown(InputSetup.instance.useItem) && isItemUsable == true && isSelectingItem == false)
            {
                Debug.Log("useitem");
                UseItem();
            }
        }
    }

    /// <summary>
    /// function untuk gunain item
    /// </summary>
    public void UseItem()
    {
        isUsingItem = true;
        CharacterInput.instance.selectedItem = selectedItem;

        //trigger make item di animator masuk ke charinput useitem
        animator.SetTrigger("drink");
    }

    /// <summary>
    /// function untuk slide item
    /// </summary>
    /// <param name="isRight">true = geser ke kanan</param>
    public void SlideItem(bool isRight)
    {
        for (int i = 0; i < usableIndicator.Length; i++)
        {
            for (int j = 0; j < usableItemList.Count; j++)
            {
                if (usableIndicator[i].GetComponent<UsableItemIndicator>().item == usableItemList[j])
                {
                    if (isRight == true)
                    {
                        if (j == usableItemList.Count - 1)
                            usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[0];
                        else
                            usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[j + 1];

                        usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
                        break;
                    }
                    else {
                        if (j == 0)
                            usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[usableItemList.Count - 1];
                        else
                            usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[j - 1];

                        usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// setelah slide, maka item tersebut akan di select
    /// </summary>
    void SelectItem()
    {
        if (usableItemList.Count > 0)
        {
            selectedItem = usableIndicator[1].GetComponent<UsableItemIndicator>().item;
            CheckIfItemIsUsable();
        }
    }
    
    /// <summary>
    /// function ini dipanggil saat nge slide usableitem dan juga jika ada trigger
    /// misalnya saat memegang tool dekat dengan soilt
    /// </summary>
    public void CheckIfItemIsUsable()
    {
        isItemUsable = selectedItem.IsItemUsable();
    }

    /// <summary>
    /// function untuk mengambil usable item dari inventory
    /// </summary>
    public void GetUsableItem()
    {
        //is using item true kalo lagi make item, akan false kalo item yang dipake abis --> ada di inventoryindicator refreshinventory
        if (!isUsingItem)
        {
            usableItemList.Clear();
            for (int i = 0; i < Inventory.instance.inventoryIndicator.Length; i++)
            {
                if (Inventory.instance.inventoryIndicator[i].GetComponent<InventoryIndicator>().item != null)
                {
                    if (Inventory.instance.inventoryIndicator[i].GetComponent<InventoryIndicator>().item.isUsable == true)
                    {
                        usableItemList.Add(Inventory.instance.inventoryIndicator[i].GetComponent<InventoryIndicator>().item);
                    }
                }
            }
        }

        if (usableItemList.Count == 0)
        {
            for (int i = 0; i < usableIndicator.Length; i++)
            {
                usableIndicator[i].GetComponent<UsableItemIndicator>().item = null;
                usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            }
            return;
        }

        if (usableItemList.Count == 1)
        {
            for (int i = 0; i < usableIndicator.Length; i++)
            {
                usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[0];
                usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            }
        }
        else if (usableItemList.Count == 2)
        {
            usableIndicator[0].GetComponent<UsableItemIndicator>().item = usableItemList[0];
            usableIndicator[0].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            usableIndicator[1].GetComponent<UsableItemIndicator>().item = usableItemList[1];
            usableIndicator[1].GetComponent<UsableItemIndicator>().RefreshUsableItem();
            usableIndicator[2].GetComponent<UsableItemIndicator>().item = usableItemList[0];
            usableIndicator[2].GetComponent<UsableItemIndicator>().RefreshUsableItem();
        }
        else if (!isUsingItem)
        {
            for (int i = 0; i < usableItemList.Count; i++)
            {
                usableIndicator[i].GetComponent<UsableItemIndicator>().item = usableItemList[i];
                usableIndicator[i].GetComponent<UsableItemIndicator>().RefreshUsableItem();
                if (i == usableIndicator.Length - 1)
                {
                    break;
                }
            }
        }

        if (usableItemList.Count > 0)
        {
            isItemUsable = true;
        }
        else
        {
            isItemUsable = false;
        }

        if (!isUsingItem)
        {
            selectedItem = usableIndicator[1].GetComponent<UsableItemIndicator>().item;
            CheckIfItemIsUsable();
            isUsingItem = false;
        }
    }
}
