using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int[] characterAppearance; //gender,skincolor,hairtype,haircolor

    public PlayerSaveData()
    {
        characterAppearance = new int[PlayerData.instance.characterAppearance.Length];

        for (int i = 0; i < characterAppearance.Length; i++)
        {
            characterAppearance[i] = PlayerData.instance.characterAppearance[i];
        }
    }
}
