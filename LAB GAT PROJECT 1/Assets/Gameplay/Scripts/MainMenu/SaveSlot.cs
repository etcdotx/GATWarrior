using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSlot : MonoBehaviour {

    public MainMenuScript mms;
    public int saveSlot;
    public bool[] saveSlotExist;

	// Use this for initialization
	void Start () {
        try
        {
            mms = GameObject.Find("MainMenuScript").GetComponent<MainMenuScript>();
        }
        catch { }
        for (int i = 0; i < saveSlotExist.Length; i++)
        {
            string txt = i.ToString();
            string path = Application.persistentDataPath + "/player" + txt + ".savegame";
            if (File.Exists(path))
            {
                saveSlotExist[i] = true;
                try
                {
                    mms.saveSlotText[i].text = "Exist";
                } catch { }
            }
        }        
	}
}
