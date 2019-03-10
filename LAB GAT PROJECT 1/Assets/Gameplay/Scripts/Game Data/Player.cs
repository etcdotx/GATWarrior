using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public CameraMovement cm;
    public int[] characterAppearance; //Gender,Skincolor,hair,haircolor
    public GameObject character;
    public GameObject[] body;// 0 Gender, 1 Hair
    public GameDataBase gdb;
    public StartGame sg;
    public SaveSlot ss;

    public List<CollectionQuest> collectionQuest = new List<CollectionQuest>();
    public List<int> finishedCollectionQuestID = new List<int>();

    public Image[] inventoryIndicator;
    public int[] inventory;
    public List<Item> item = new List<Item>();

    public GameObject[] needToBeLoad;
    // Use this for initialization
    void Start () {
        gdb = GameObject.FindGameObjectWithTag("GDB").GetComponent<GameDataBase>();
        ss = GameObject.Find("SaveSlot").GetComponent<SaveSlot>();
        RefreshItem();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this, ss.saveSlot.ToString());
    }

    public void LoadPlayer()
    {
        //start new game/scene
        for (int i = 0; i < needToBeLoad.Length; i++)
        {
            needToBeLoad[i].SetActive(true);
        }

        PlayerData data = SaveSystem.LoadPlayer(ss.saveSlot.ToString());

        characterAppearance = new int[data.characterAppearance.Length];

        for (int i = 0; i < characterAppearance.Length; i++)
        {
            characterAppearance[i] = data.characterAppearance[i];
        }

        try
        {
            GameObject spawnLocation = GameObject.Find("SpawnLocation");
            body[0] = GameObject.Instantiate(gdb.genderType[characterAppearance[0]], spawnLocation.transform.position, spawnLocation.transform.rotation, null);
            body[0].GetComponent<Renderer>().material.color = gdb.skinColor[characterAppearance[1]];
            if (characterAppearance[0] == 0)
            {
                body[1] = GameObject.Instantiate(gdb.maleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
            }
            else
            {
                body[1] = GameObject.Instantiate(gdb.femaleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
            }
            body[1].GetComponent<Renderer>().material.color = gdb.hairColor[characterAppearance[3]];
        }
        catch {
        }

        do {
            try
            {
                cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
                character = GameObject.FindGameObjectWithTag("Player");
                cm.lookAt = character.transform;
                character.GetComponent<Rigidbody>().isKinematic = false;
                sg = GameObject.Find("StartGame").GetComponent<StartGame>();
                character.transform.localScale = sg.characterScale;
            }
            catch { }
        } while (cm == null);        
    }

    public void AddQuest(CollectionQuest newQuest)
    {
        collectionQuest.Add(newQuest);
    }

    public void AddItem(Item newItem)
    {
        bool itemExist=false;

        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].id == newItem.id)
            {
                item[i].quantity++;
                itemExist = true;
                RefreshItem();
                break;
            }
        }

        if (itemExist == false)
        {
            item.Add(newItem);
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == 0)
                {
                    inventory[i] = newItem.id;
                    RefreshItem();
                    break;
                }
            }
        }
    }

    public void RefreshItem()
    {
        for (int i = 0; i < inventoryIndicator.Length; i++)
        {
            try
            {
                inventoryIndicator[i].transform.GetChild(0).gameObject.SetActive(false);
                if (item[i].quantity != 0)
                {
                    inventoryIndicator[i].sprite = item[i].itemImage;
                    inventoryIndicator[i].transform.GetChild(0).GetComponent<Text>().text = item[i].quantity.ToString();
                    inventoryIndicator[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            } catch { }
        }
    }
}
