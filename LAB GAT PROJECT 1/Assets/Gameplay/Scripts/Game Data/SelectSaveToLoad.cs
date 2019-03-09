using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSaveToLoad : MonoBehaviour
{
    public MainMenuScript mms;
    public int x;

    private void Start()
    {
        mms = GameObject.Find("MainMenuScript").GetComponent<MainMenuScript>();
    }

    public void loadGame() {
        mms.saveSlot(x);
    }
}
