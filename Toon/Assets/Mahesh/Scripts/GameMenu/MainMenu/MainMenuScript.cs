using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuScript : MonoBehaviour {

    public static MainMenuScript instance;

    [Header("Save System")]
    //text yang nanti akan tampil ketika sedang select save data
    public TextMeshProUGUI[] saveSlotText;

    [Header("Scene Name")]
    //scene selanjutnya setelah loadgame
    public string gameScene;

    //public karena drag n drop
    public EventSystem eventSystem;
    public GameObject firstSelectedLoadGame;

    [Header("Background optiong")]
    //untuk digerakin backgroundnya
    public RectTransform background;
    //speed untuk lerping
    public float lerpingSpeed;
    Vector3 targetVector;
    bool backGroundLerping;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    
    void Start()
    {
        backGroundLerping = false;
    }

    private void Update()
    {
        //perpindahan background dengan lerping
        if (backGroundLerping)
        {
            background.anchoredPosition = Vector3.Lerp(background.anchoredPosition, targetVector, Time.deltaTime * lerpingSpeed);
            if (Vector3.Distance(background.anchoredPosition, targetVector) <= 1)
            {
                background.anchoredPosition = targetVector;
                backGroundLerping = false;
            }
        }
    }

    /// <summary>
    /// pilih saveslotnya
    /// </summary>
    /// <param name="x"></param>
    public void LoadSlot(int x)
    {
        GameDataBase.instance.saveSlot = x;
        //Instantiate(ddolGamePrefab);

        if (GameDataBase.instance.saveSlotExist[x] == true)
        {
            //
        }
        else
        {
            GameDataBase.instance.newGame = true;
        }
        StartCoroutine(LoadScene(gameScene));
    }

    /// <summary>
    /// function untuk delete save
    /// </summary>
    /// <param name="x">nomor slot savenya</param>
    public void DeleteSlot(int x)
    {
        SaveSystem.DeletePlayer(x.ToString());
        saveSlotText[x].text = "Empty Game";
        GameDataBase.instance.saveSlotExist[x] = false;
        Debug.Log("Delete successful");
    }

    /// <summary>
    /// transisi di mainmenu
    /// </summary>
    /// <param name="isUp">isup = true masuk ke menu save data</param>
    public void MoveBackground(bool isUp) {
        if (isUp)
        {
            targetVector = new Vector3(background.anchoredPosition.x, -540, 0);
            eventSystem.SetSelectedGameObject(firstSelectedLoadGame);
        }
        else
        {
            targetVector = new Vector3(background.anchoredPosition.x, 540, 0);
        }

        backGroundLerping = true;
    }

    /// <summary>
    /// loading screen
    /// </summary>
    /// <param name="sceneName">scene yang di load</param>
    /// <returns></returns>
    public IEnumerator LoadScene(string sceneName)
    {
        LoadingScreen.instance.loadingImage.gameObject.SetActive(true);
        LoadingScreen.instance.StartLoading();
        LoadingScreen.instance.progressBar.fillAmount = 0;
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            LoadingScreen.instance.progressBar.fillAmount = async.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
