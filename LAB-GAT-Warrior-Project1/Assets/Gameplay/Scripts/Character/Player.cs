using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public CameraMovement CameraMovement;
    public int[] characterAppearance; //Gender,Skincolor,hair,haircolor
    public GameObject Character;
    public GameObject[] Body;// 0 Gender, 1 Hair
    public GameDataBase GDB;
    public SaveSlot SaveSlot;

	// Use this for initialization
	void Start () {
        GDB = GameObject.FindGameObjectWithTag("GDB").GetComponent<GameDataBase>();
        SaveSlot = GameObject.Find("SaveSlot").GetComponent<SaveSlot>();
        LoadPlayer();
	}

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this, SaveSlot.saveSlot.ToString());
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer(SaveSlot.saveSlot.ToString());

        characterAppearance = new int[data.characterAppearance.Length];

        for (int i = 0; i < characterAppearance.Length; i++)
        {
            characterAppearance[i] = data.characterAppearance[i];
        }

        try
        {
            Body[0] = GameObject.Instantiate(GDB.genderType[characterAppearance[0]], GameObject.Find("SpawnLocation").transform.position, Quaternion.identity, null);
            Body[0].GetComponent<Renderer>().material.color = GDB.skinColor[characterAppearance[1]];
            if (characterAppearance[0] == 0)
            {
                Body[1] = GameObject.Instantiate(GDB.maleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
            }
            else
            {
                Body[1] = GameObject.Instantiate(GDB.femaleHairType[characterAppearance[2]], GameObject.FindGameObjectWithTag("PlayerHead").transform);
            }
            Body[1].GetComponent<Renderer>().material.color = GDB.hairColor[characterAppearance[3]];
        }
        catch {
        }

        try
        {
            CameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
            Character = GameObject.FindGameObjectWithTag("Player");
            CameraMovement.lookAt = Character.transform;
            Character.GetComponent<Rigidbody>().isKinematic = false;
        }
        catch { }
    }
}
