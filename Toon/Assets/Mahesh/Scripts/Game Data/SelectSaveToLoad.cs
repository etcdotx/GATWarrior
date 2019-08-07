using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSaveToLoad : MonoBehaviour
{
    MainMenuScript mms;

    [Header("Nomor untuk save yang ingin di load")]
    public int x;

    private void Start()
    {
        mms = GameObject.Find("MainMenuScript").GetComponent<MainMenuScript>();
    }

    /// <summary>
    /// dipakai dibutton untuk select file yang mau di load
    /// </summary>
    public void LoadGame() {
        mms.LoadSlot(x);
    }
}
