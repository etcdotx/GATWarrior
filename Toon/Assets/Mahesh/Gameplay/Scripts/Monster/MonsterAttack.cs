using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAttack : MonoBehaviour
{
    public MonsterStatus monsterStatus;
    public MonsterMovement monsterMovement;
    public NavMeshAgent agent;

    public int maxAttackNum = 1;
    public float damage;
    public bool isAttacking = false;
    public int attackNum;

    public Rigidbody rigid;
    public float speed;

    private void Start()
    {
        monsterStatus = GetComponent<MonsterStatus>();
        monsterMovement = GetComponent<MonsterMovement>();
        agent = GetComponent<NavMeshAgent>();
    }
}
