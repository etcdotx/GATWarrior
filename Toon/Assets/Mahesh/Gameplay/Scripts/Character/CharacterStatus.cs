using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public CharacterMovement characterMovement;

    [Header("Player Status")]
    public string playerName;
    public Animator anim;

    public List<MonsterAttack> monsterAttack = new List<MonsterAttack>();
    public bool isDamaged;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("PlayerStatus").GetComponent<PlayerStatus>();
        characterMovement = GetComponent<CharacterMovement>();
        anim = GetComponent<Animator>();
    }

    public void Damaged(float dmg)
    {
        anim.SetTrigger("attacked");
        characterMovement.currentSpeed = 0;
        playerStatus.curHealth -= dmg;
        playerStatus.RefreshHp();
    }

    public void Blocked(float dmg)
    {
        anim.SetTrigger("blocking");
    }
}
