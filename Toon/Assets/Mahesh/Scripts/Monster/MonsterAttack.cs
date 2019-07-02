using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAttack : MonoBehaviour
{
    public MonsterStatus monsterStatus;
    public NewMonsterMovement monsterMovement;
    public NavMeshAgent agent;

    public int maxAttackNum;
    public int attackNum;
    public float damage;
    public bool isAttacking = false;

    public Rigidbody rigid;
    public float speed;

    public List<int> attackSuccess = new List<int>();
    private void Awake()
    {
        monsterStatus = GetComponent<MonsterStatus>();
        monsterMovement = GetComponent<NewMonsterMovement>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if (attackSuccess.Count != 0 && attackNum==0)
            attackSuccess.Clear();
    }
}
