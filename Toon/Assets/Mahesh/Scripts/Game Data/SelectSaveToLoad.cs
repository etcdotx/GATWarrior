using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSaveToLoad : MonoBehaviour
{
    MainMenuScript mms;

    /// <summary>
    /// dipakai dibutton untuk select file yang mau di load
    /// </summary>
    public int x;

    private void Start()
    {
        mms = GameObject.Find("MainMenuScript").GetComponent<MainMenuScript>();
    }

    public void LoadGame() {
        mms.SaveSlot(x);
    }
}
