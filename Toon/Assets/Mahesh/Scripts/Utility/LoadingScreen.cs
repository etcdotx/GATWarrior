using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    public Image loadingImage;
    public Sprite[] loadingImageList;

    public Image progressBar;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        loadingImage.gameObject.SetActive(false);
        progressBar.fillAmount = 0;
    }

    public void StartLoading() {
        int number = Random.Range(0, loadingImageList.Length-1);
        loadingImage.overrideSprite = loadingImageList[number];
    }
}
