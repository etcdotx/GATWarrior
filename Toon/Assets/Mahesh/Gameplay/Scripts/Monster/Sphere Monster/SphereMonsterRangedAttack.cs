using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereMonsterRangedAttack : MonoBehaviour
{
    public MonsterStatus monsterStatus;
    public MonsterMovement monsterMovement;
    public MonsterAttack monsterAttack;
    public NavMeshAgent agent;

    public int maxAttackNum = 1;
    public float damage;
    public int attackNum;

    public Rigidbody rigid;
    public float speed;
    public float flyForce;

    public GameObject sphereBall;
    public Transform spawnPos;

    public bool attacking;

    private void Start()
    {
        monsterStatus = GetComponent<MonsterStatus>();
        monsterMovement = GetComponent<MonsterMovement>();
        monsterAttack = GetComponent<MonsterAttack>();


        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        attacking = false;
    }

    private void Update()
    {
        if (monsterStatus.hp > 0)
            if (monsterAttack.isAttacking && !monsterMovement.isInterrupted 
                && !monsterMovement.isFalling && !attacking)
                StartCoroutine(Attacking(1));

        if (monsterMovement.isInterrupted || monsterMovement.isFalling)
        {
            StopAllCoroutines();
        }
    }

    public void Attack(int num) {
        if (num == 1)
        {
            monsterAttack.isAttacking = false;
            Instantiate(sphereBall, spawnPos.position, transform.rotation, null);
        }
    }

    public IEnumerator Attacking(int num)
    {
        attacking = true;
        agent.isStopped = true;
        Attack(num);
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
        agent.ResetPath();
        rigid.velocity = new Vector3(0, 0, 0);
        attacking = false;
    }
}
