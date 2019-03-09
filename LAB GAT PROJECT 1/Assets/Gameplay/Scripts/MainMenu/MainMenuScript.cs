using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour {

    public InputSetup inputSetup;

    public int menuNumber;

    public GameObject menu0;
    public GameObject menu1;
    public GameObject[] menu0Nav;
    public GameObject[] menu1Nav;

    [Header("Save System")]
    public SaveSlot ss;
    public Text[] saveSlotText;

    [Header("Scene Name")]
    public string[] sceneNameMenu1;//0.gameplay,1.charactereditor

    [Header("Navigator")]
    public int navigatorNum;
    public float navigatorDelay;
    public bool canNav;

    // Use this for initialization
    void Start() {
        resetMenu();
        menuNumber = 0;
        loadMenu();
        ss = GameObject.Find("SaveSlot").GetComponent<SaveSlot>();
        canNav = true;
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(inputSetup.back))
        {
            if (menuNumber > 0)
            {
                menuNumber--;
                loadMenu();
            }            
        }

        if (Input.GetKeyDown(inputSetup.select))
        {
            doSelect(menuNumber, navigatorNum);
        }

        if (menuNumber == 1)
        {
            if (Input.GetKeyDown(inputSetup.deleteSave))
            {
                deleteSlot(navigatorNum);
            }
        }

        if (canNav == true)
        {
            if (Input.GetAxis("LeftJoystickVertical") != 0 || Input.GetAxis("D-Pad Up") != 0 || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                doNavigate();
            }
        }
    }

    void doSelect(int menuNum, int navNum) {
        if (menuNum == 0)
        {
            if (Input.GetKeyDown(inputSetup.select) && navigatorNum == 0)
            {
                menuNumber = 1;
                loadMenu();
            }
        }
        if (menuNum == 1)
        {
            saveSlot(navNum);
        }
    }

    #region navigate
    void doNavigate() {
        if (Input.GetAxis("LeftJoystickVertical") == -1 || Input.GetAxis("D-Pad Down") == 1 || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (menuNumber == 0)
            {
                startNavMenu(menu0Nav, true);
            }
            if (menuNumber == 1)
            {
                startNavMenu(menu1Nav, true);
            }
        }
        if (Input.GetAxis("LeftJoystickVertical") == 1 || Input.GetAxis("D-Pad Up") == 1 || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (menuNumber == 0)
            {
                startNavMenu(menu0Nav, false);
            }
            if (menuNumber == 1)
            {
                startNavMenu(menu1Nav, false);
            }
        }
    }

    #endregion

    void startNavMenu(GameObject[] Menu, bool isDown) {
        if (isDown == true)
        {
            if (navigatorNum == Menu.Length - 1)
            {
                navigatorNum = 0;
            }
            else
            {
                navigatorNum++;
            }
            StartCoroutine(navMenuDelay(Menu));
        }
        if (isDown == false)
        {
            if (navigatorNum == 0)
            {
                navigatorNum = Menu.Length-1;
            }
            else
            {
                navigatorNum--;
            }
            StartCoroutine(navMenuDelay(Menu));
        }
    }

    private IEnumerator navMenuDelay(GameObject[] Menu)
    {
        canNav = false;
        applyNav(Menu);
        yield return new WaitForSeconds(navigatorDelay);
        canNav = true;
    }

    void applyNav(GameObject[] Menu)
    {
        for (int i = 0; i < Menu.Length; i++)
        {
            Menu[i].SetActive(false);
        }
        Menu[navigatorNum].SetActive(true);
    }

    void resetMenu() {
        menu0.SetActive(false);
        menu1.SetActive(false);
        for (int i = 0; i < menu0Nav.Length; i++)
        {
            
            menu0Nav[i].SetActive(false);
        }
        for (int i = 0; i < menu1Nav.Length; i++)
        {
            menu1Nav[i].SetActive(false);
        }
    }

    void loadMenu()
    {
        resetMenu();

        if (menuNumber == 0)
        {
            menu0.SetActive(true);
            navigatorNum = 0;
            applyNav(menu0Nav);
        }

        if (menuNumber == 1)
        {
            menu1.SetActive(true);
            navigatorNum = 0;
            applyNav(menu1Nav);
        }
    }

    public void openStartGame() {
        menuNumber = 1;
        loadMenu();
    }

    #region SAVESLOT
    public void saveSlot(int x)
    {
        ss.saveSlot = x;
        if (ss.saveSlotExist[x] == true)
        {
            SceneManager.LoadScene(sceneNameMenu1[0]);
        }
        else
        {
            SceneManager.LoadScene(sceneNameMenu1[1]);
        }
    }
    #endregion

    #region DELETESLOT
    public void deleteSlot(int x)
    {
        SaveSystem.deletePlayer(x.ToString());
        saveSlotText[x].text = "Empty Game";
        ss.saveSlotExist[x] = false;
        Debug.Log("Delete successful");
    }
    #endregion
}
