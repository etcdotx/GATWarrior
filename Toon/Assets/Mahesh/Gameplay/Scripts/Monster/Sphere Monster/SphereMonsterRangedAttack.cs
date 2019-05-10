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
    private void Start()
    {
        monsterStatus = GetComponent<MonsterStatus>();
        monsterMovement = GetComponent<MonsterMovement>();
        monsterAttack = GetComponent<MonsterAttack>();


        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (monsterStatus.hp > 0)
            if (monsterAttack.isAttacking == true && monsterMovement.isInterrupted == false)
                StartCoroutine(Attacking(1));
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
        agent.isStopped = true;
        //agent.enabled = false;
        monsterMovement.StopAllCoroutines();
        Attack(num);
        yield return new WaitForSeconds(1f);
        //agent.enabled = true;
        agent.isStopped = false;
        agent.ResetPath();
        rigid.velocity = new Vector3(0, 0, 0);
        StopAllCoroutines();
    }
}
