using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    //untuk nama spawn locationnya ketika player di spawn
    string selectSpawnLocationName;

    /// <summary>
    /// nentuin spawn position ketika pindah scene
    /// dengan cara mengetahui sumber scene sebelumnya dan scene selanjutnya
    /// 
    /// harusnya bisa diganti dengan method loadscene dengan membawa position (not implemented)
    /// </summary>
    static string curScene;
    static string newScene;

    /// <summary>
    /// set nama-nama string sesuai dengan nama scene
    /// </summary>
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

        //DEVELOPERMODE
        if (GameDataBase.instance.DEBUGMODE == true)
        {
            GameDataBase.instance.saveSlot = 0;
        }

        if(!GameDataBase.instance.newGame)
            PlayerData.instance.LoadPlayer(selectSpawnLocationName);
    }

    /// <summary>
    /// function untuk check kalau pindah scene dan nentuin spawn posnya
    /// </summary>
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
