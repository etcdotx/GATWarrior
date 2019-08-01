using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuScript : MonoBehaviour {

    public static MainMenuScript instance;
    public Transform ddolGamePrefab;

    [Header("Save System")]
    public TextMeshProUGUI[] saveSlotText;

    [Header("Scene Name")]
    public string gameScene;

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

    // Use this for initialization
    void Start()
    {
        backGroundLerping = false;
    }

    private void Update()
    {
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

    public void SaveSlot(int x)
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

    public void DeleteSlot(int x)
    {
        SaveSystem.DeletePlayer(x.ToString());
        saveSlotText[x].text = "Empty Game";
        GameDataBase.instance.saveSlotExist[x] = false;
        Debug.Log("Delete successful");
    }

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
