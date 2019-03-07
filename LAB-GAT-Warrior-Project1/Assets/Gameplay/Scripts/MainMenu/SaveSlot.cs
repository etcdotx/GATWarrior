using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSlot : MonoBehaviour {

    public MainMenuScript MMS;
    public int saveSlot;
    public bool[] saveSlotExist;

	// Use this for initialization
	void Start () {
        try
        {
            MMS = GameObject.Find("ScriptHandler").GetComponent<MainMenuScript>();
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
                    MMS.saveSlotText[i].text = "Exist";
                } catch { }
            }
        }        
	}
}
