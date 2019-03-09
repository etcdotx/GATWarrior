using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public CameraMovement cm;
    public int[] characterAppearance; //Gender,Skincolor,hair,haircolor
    public GameObject character;
    public GameObject[] body;// 0 Gender, 1 Hair
    public GameDataBase gdb;
    public SaveSlot ss;

	// Use this for initialization
	void Start () {
        gdb = GameObject.FindGameObjectWithTag("GDB").GetComponent<GameDataBase>();
        ss = GameObject.Find("SaveSlot").GetComponent<SaveSlot>();
	}

    public void savePlayer()
    {
        SaveSystem.savePlayer(this, ss.saveSlot.ToString());
    }

    public void loadPlayer(int spawnPosition)
    {
        PlayerData data = SaveSystem.loadPlayer(ss.saveSlot.ToString());

        characterAppearance = new int[data.characterAppearance.Length];

        for (int i = 0; i < characterAppearance.Length; i++)
        {
            characterAppearance[i] = data.characterAppearance[i];
        }

        try
        {
            body[0] = GameObject.Instantiate(gdb.genderType[characterAppearance[0]], GameObject.Find("SpawnLocation").transform.position, Quaternion.identity, null);
            body[0].GetComponent<Renderer>().material.color = gdb.skinColor[characterAppearance[1]];
            if (characterAppearance[0] == 0)
            {
                body[1] = GameObject.Instantiate(gdb.maleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
            }
            else
            {
                body[1] = GameObject.Instantiate(gdb.femaleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
            }
            body[1].GetComponent<Renderer>().material.color = gdb.hairColor[characterAppearance[3]];
        }
        catch {
        }

        do {
            try
            {
                cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
                character = GameObject.FindGameObjectWithTag("Player");
                cm.lookAt = character.transform;
                character.GetComponent<Rigidbody>().isKinematic = false;
            }
            catch { }
        } while (cm == null);        
    }
}
