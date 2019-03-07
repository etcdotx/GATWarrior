using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int[] characterAppearance; //gender,skincolor,hairtype,haircolor

    public PlayerData(Player player)
    {
        characterAppearance = new int[player.characterAppearance.Length];

        for (int i = 0; i < characterAppearance.Length; i++)
        {
            characterAppearance[i] = player.characterAppearance[i];
        }
    }
}
