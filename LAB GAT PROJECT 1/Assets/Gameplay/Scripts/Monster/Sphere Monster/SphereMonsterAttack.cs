using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereMonsterAttack : MonoBehaviour
{
    public MonsterAttack monsterAttack;
    public NavMeshAgent agent;
    public int maxAttackNum = 1;
    public float damage;

    public Rigidbody rigid;
    public float speed;
    public float flyForce;

    private void Start()
    {
        monsterAttack = GetComponent<MonsterAttack>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Attack(int num) {
        if (num == 1) {
            monsterAttack.StartCoroutine(Attacking());
            Vector3 playerForce = new Vector3(0, flyForce, 0) + transform.forward * speed;
            rigid.AddForce(playerForce, ForceMode.Acceleration);
            Debug.Log("in");
        }
    }

    public IEnumerator Attacking()
    {
        monsterAttack.attacking = true;
        agent.isStopped = true;
        agent.enabled = false;
        yield return new WaitForSeconds(1);
        monsterAttack.attacking = false;
        agent.enabled = true;
        agent.isStopped = false;
        agent.ResetPath();
        rigid.velocity = new Vector3(0, 0, 0);
    }
}
