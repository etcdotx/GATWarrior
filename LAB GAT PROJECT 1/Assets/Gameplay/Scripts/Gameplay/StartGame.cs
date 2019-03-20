using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public PlayerData playerData;
    public GameMenuManager gameMenuManager;
    public GameObject character;
    public GameDataBase gameDataBase;
    public string selectSpawnLocationName;
    public Vector3 characterScale;
    public static string curScene;
    public static string newScene;

    [Header("Scene Name")]
    public string Rumah;
    public string HutanAman;
    public string Kota;
    public string Hutan1_1;
    public static string Hutan1_2;
    public string Hutan1_3;
    public string Hutan1_4;

    private void Start()
    {
        CheckScene();
        GameStatus.isTalking = false;
        GameStatus.ResumeGame();
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        gameMenuManager = GameObject.FindGameObjectWithTag("GameMenuManager").GetComponent<GameMenuManager>();
        gameDataBase = GameObject.FindGameObjectWithTag("GameDataBase").GetComponent<GameDataBase>();
        gameMenuManager.cantOpenMenu = false;
        //DEVELOPERMODE
        if (playerData.DEVELOPERMODE == true)
        {
            gameDataBase.saveSlot = 0;
        }
        playerData.LoadPlayer(selectSpawnLocationName);
    }

    void CheckScene()
    {
        if (curScene == null)
        {
            curScene = Rumah;
        }

        newScene = SceneManager.GetActiveScene().name;
        if (curScene == Rumah)
        {
            if (newScene == Rumah)
            {
                selectSpawnLocationName = HutanAman + "-" + Rumah;
                curScene = newScene;
                Debug.Log(selectSpawnLocationName);
                return;
            }
            if (newScene == HutanAman)
            {
                selectSpawnLocationName = Rumah + "-" + HutanAman;
                curScene = newScene;
                Debug.Log(selectSpawnLocationName);
                return;
            }
        }
        else if (curScene == HutanAman)
        {
            if (newScene == Rumah)
            {
                selectSpawnLocationName = HutanAman + "-" + Rumah;
                curScene = newScene;
                Debug.Log(selectSpawnLocationName);
                return;
            }
            if (newScene == Kota)
            {
                selectSpawnLocationName = HutanAman + "-" + Kota;
                curScene = newScene;
                Debug.Log(selectSpawnLocationName);
                return;
            }
        }
        else if (curScene == Kota)
        {
            if (newScene == HutanAman)
            {
                selectSpawnLocationName = Kota + "-" + HutanAman;
                curScene = newScene;
                Debug.Log(selectSpawnLocationName);
                return;
            }
        }
    }
}
