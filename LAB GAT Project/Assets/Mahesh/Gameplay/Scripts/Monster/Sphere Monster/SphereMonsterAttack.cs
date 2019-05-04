using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereMonsterAttack : MonoBehaviour
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

    public BoxCollider hitBox;
    private void Start()
    {
        monsterStatus = GetComponent<MonsterStatus>();
        monsterMovement = GetComponent<MonsterMovement>();
        monsterAttack = GetComponent<MonsterAttack>();


        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        hitBox.enabled = false;
    }

    private void Update()
    {
        if (monsterStatus.hp > 0)
            if (monsterAttack.isAttacking == true && monsterMovement.isInterrupted == false)
                StartCoroutine(Attacking(1));

        AttackBehaviour();
    }

    public void Attack(int num) {
        if (num == 1)
        {
            monsterAttack.isAttacking = false;
            Vector3 playerForce = new Vector3(0, flyForce, 0) + transform.forward * speed;
            rigid.AddForce(playerForce, ForceMode.Acceleration);
        }
    }

    public IEnumerator Attacking(int num)
    {
        hitBox.enabled = true;
        agent.isStopped = true;
        agent.enabled = false;
        monsterMovement.StopAllCoroutines();
        Attack(num);
        yield return new WaitForSeconds(1f);
        agent.enabled = true;
        agent.isStopped = false;
        agent.ResetPath();
        rigid.velocity = new Vector3(0, 0, 0);
        StopAllCoroutines();
        hitBox.enabled = false;
    }

    public void AttackBehaviour()
    {
        bool blocked = false;
        if (hitBox.enabled == true)
        {
            Debug.Log("attack!");

            Collider[] col = Physics.OverlapBox(hitBox.bounds.center, hitBox.bounds.extents, hitBox.transform.rotation, LayerMask.GetMask("Shield"));

            foreach (Collider c in col)
            {
                Debug.Log(c.transform.name);
                hitBox.enabled = false;
                blocked = true;
                Debug.Log("blocked");
            }

            if (blocked == false)
            {
                Debug.Log("attack not blocked");
                Collider[] cols = Physics.OverlapBox(hitBox.bounds.center, hitBox.bounds.extents, hitBox.transform.rotation, LayerMask.GetMask("Player"));

                foreach (Collider c in cols)
                {
                    hitBox.enabled = false;
                    CharacterStatus characterStatus = c.GetComponent<CharacterStatus>();
                    characterStatus.Damaged(damage);
                    //Debug.Log("damaged ! hp = " + characterStatus.health);
                    Debug.Log("Damaged");
                }
            }
        }
    }

}
