using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    public PlayerData playerData;
    public CharacterMovement characterMovement;

    [Header("Player Status")]
    public string playerName;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        characterMovement = GetComponent<CharacterMovement>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damaged(float dmg)
    {
        anim.SetTrigger("attacked");
        characterMovement.currentSpeed = 0;
        playerData.curHealth -= dmg;
        playerData.RefreshHp();
    }
}
