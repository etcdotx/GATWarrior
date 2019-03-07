using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour {

    public int menuNumber;
    public GameObject[] Menu1;
    public GameObject[] Menu2;
    public Text[] saveSlotText;
    public SaveSlot SS;

    [Header("Scene Name")]
    public string CharacterCreation;
    public string Gameplay;

    // Use this for initialization
    void Start () {
        resetMenu();
        menuNumber = 1;
        loadMenu();
        SS = GameObject.Find("SaveSlot").GetComponent<SaveSlot>();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuNumber = 1;
            loadMenu();
        }
    }

    void resetMenu() {
        for (int i = 0; i < Menu1.Length; i++)
        {
            Menu1[i].SetActive(false);
        }
        for (int i = 0; i < Menu2.Length; i++)
        {
            Menu2[i].SetActive(false);
        }
    }

    void loadMenu()
    {
        resetMenu();

        if (menuNumber == 1)
        {
            for (int i = 0; i < Menu1.Length; i++)
            {
                Menu1[i].SetActive(true);
            }
            Debug.Log("Menu1");
        }

        if (menuNumber == 2)
        {
            for (int i = 0; i < Menu2.Length; i++)
            {
                Menu2[i].SetActive(true);
            }
            Debug.Log("Menu2");
        }
    }

    public void openStartGame() {
        menuNumber = 2;
        loadMenu();
    }

    public void SaveSlot0()
    {
        SS.saveSlot = 0;
        if (SS.saveSlotExist[0] == true)
        {
            SceneManager.LoadScene(Gameplay);
        }
        else
        {
            SceneManager.LoadScene(CharacterCreation);
        }
    }

    public void SaveSlot1()
    {
        SS.saveSlot = 1;
        if (SS.saveSlotExist[1] == true)
        {
            SceneManager.LoadScene(Gameplay);
        }
        else
        {
            SceneManager.LoadScene(CharacterCreation);
        }

    }

    public void SaveSlot2()
    {
        SS.saveSlot = 2;
        if (SS.saveSlotExist[2] == true)
        {
            SceneManager.LoadScene(Gameplay);
        }
        else
        {
            SceneManager.LoadScene(CharacterCreation);
        }

    }
}
