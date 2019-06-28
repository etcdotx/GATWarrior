using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
    public Transform ddolGamePrefab;

    public int menuNumber;

    public GameObject menu0;
    public GameObject menu1;
    public GameObject[] menu0Nav;
    public GameObject[] menu1Nav;

    [Header("Save System")]
    public Text[] saveSlotText;

    [Header("Scene Name")]
    public string[] sceneNameMenu1;//0.gameplay,1.charactereditor

    [Header("Navigator")]
    public int navigatorNum;
    public bool canNav;

    // Use this for initialization
    void Start() {
        ResetMenu();
        menuNumber = 0;
        LoadMenu();
        canNav = true;

        //

        string path = Application.persistentDataPath + "/player" + ".savegame";
        Debug.Log(path);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(InputSetup.instance.back))
        {
            if (menuNumber > 0)
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                menuNumber--;
                LoadMenu();
            }            
        }

        if (Input.GetKeyDown(InputSetup.instance.select))
        {
            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
            DoSelect(menuNumber, navigatorNum);
        }

        if (menuNumber == 1)
        {
            if (Input.GetKeyDown(InputSetup.instance.deleteSave))
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                SeleteSlot(navigatorNum);
            }
        }

        if (canNav == true)
        {
            DoNavigate();
        }
    }

    void DoSelect(int menuNum, int navNum) {
        if (menuNum == 0)
        {
            if (Input.GetKeyDown(InputSetup.instance.select) && navigatorNum == 0)
            {
                SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
                menuNumber = 1;
                LoadMenu();
            }
        }
        if (menuNum == 1)
        {
            SaveSlot(navNum);
        }
    }
    
    void DoNavigate() {
        if (Input.GetAxis("LeftJoystickVertical") == -1 || Input.GetAxis("D-Pad Down") == 1 || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
            if (menuNumber == 0)
            {
                StartNavMenu(menu0Nav, true);
            }
            if (menuNumber == 1)
            {
                StartNavMenu(menu1Nav, true);
            }
        }
        if (Input.GetAxis("LeftJoystickVertical") == 1 || Input.GetAxis("D-Pad Up") == 1 || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
            if (menuNumber == 0)
            {
                StartNavMenu(menu0Nav, false);
            }
            if (menuNumber == 1)
            {
                StartNavMenu(menu1Nav, false);
            }
        }
    }

    void StartNavMenu(GameObject[] Menu, bool isDown) {
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
            StartCoroutine(InputHold(Menu));
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
            StartCoroutine(InputHold(Menu));
        }
    }

    private IEnumerator InputHold(GameObject[] Menu)
    {
        canNav = false;
        ApplyNav(Menu);
        yield return new WaitForSeconds(0.2f);
        canNav = true;
    }

    void ApplyNav(GameObject[] Menu)
    {
        for (int i = 0; i < Menu.Length; i++)
        {
            Menu[i].SetActive(false);
        }
        Menu[navigatorNum].SetActive(true);
    }

    void ResetMenu() {
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

    void LoadMenu()
    {
        ResetMenu();

        if (menuNumber == 0)
        {
            menu0.SetActive(true);
            navigatorNum = 0;
            ApplyNav(menu0Nav);
        }

        if (menuNumber == 1)
        {
            menu1.SetActive(true);
            navigatorNum = 0;
            ApplyNav(menu1Nav);
        }
    }

    public void OpenStartGame() {
        menuNumber = 1;
        LoadMenu();
    }
    
    public void SaveSlot(int x)
    {
        GameDataBase.instance.saveSlot = x;
        Instantiate(ddolGamePrefab);

        if (GameDataBase.instance.saveSlotExist[x] == true)
        {
            SceneManager.LoadScene(sceneNameMenu1[0]);
        }
        else
        {
            SceneManager.LoadScene(sceneNameMenu1[1]);
        }
    }
    
    public void SeleteSlot(int x)
    {
        SaveSystem.DeletePlayer(x.ToString());
        saveSlotText[x].text = "Empty Game";
        GameDataBase.instance.saveSlotExist[x] = false;
        Debug.Log("Delete successful");
    }
}
