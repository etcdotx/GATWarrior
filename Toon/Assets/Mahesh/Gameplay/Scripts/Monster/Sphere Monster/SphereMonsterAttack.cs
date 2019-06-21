using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereMonsterAttack : MonoBehaviour
{
    public Animator anim;
    public MonsterStatus monsterStatus;
    public MonsterMovement monsterMovement;
    public MonsterAttack monsterAttack;
    public NavMeshAgent agent;
    
    public int bonusAttack;
    public float damage;
    public BoxCollider hitBox;

    public bool canAttack;
    private void Start()
    {
        anim = GetComponent<Animator>();
        monsterStatus = GetComponent<MonsterStatus>();
        monsterMovement = GetComponent<MonsterMovement>();
        monsterAttack = GetComponent<MonsterAttack>();


        if (monsterMovement.moveWithAgent)
            agent = GetComponent<NavMeshAgent>();
        hitBox.enabled = false;
        canAttack = true;
    }

    private void Update()
    {
        if (monsterStatus.hp > 0)
            if (monsterAttack.isAttacking && canAttack && !monsterMovement.isInterrupted && !monsterMovement.isFalling)
            {
                monsterAttack.attackNum = Random.Range(1, monsterAttack.maxAttackNum);
                Attack(monsterAttack.attackNum);
                canAttack = false;
            }

        AttackBehaviour();
    }

    public void Attack(int num) {
        if (num == 1)
        {
            if(monsterMovement.moveWithAgent)
                agent.isStopped = true;
            bonusAttack = Random.Range(0, 2);
            anim.SetTrigger("attack");
        }
    }

    public void AttackBehaviour()
    {
        if (hitBox.enabled)
        {
            //blocked
            Collider[] col = Physics.OverlapBox(hitBox.bounds.center, hitBox.bounds.extents, hitBox.transform.rotation, LayerMask.GetMask("Shield"));
            foreach (Collider c in col)
            {
                for (int i = 0; i < monsterAttack.attackSuccess.Count; i++)
                {
                    if (monsterAttack.attackSuccess[i] == monsterAttack.attackNum)
                    {
                        return;
                    }
                }
                monsterAttack.attackSuccess.Add(monsterAttack.attackNum);
                CharacterStatus characterStatus = c.GetComponent<Shield>().characterStatus;
                characterStatus.Blocked(damage);
            }

            //damaging
            Collider[] cols = Physics.OverlapBox(hitBox.bounds.center, hitBox.bounds.extents, hitBox.transform.rotation, LayerMask.GetMask("Player"));
            foreach (Collider c in cols)
            {
                for (int i = 0; i < monsterAttack.attackSuccess.Count; i++)
                {
                    if (monsterAttack.attackSuccess[i] == monsterAttack.attackNum)
                    {
                        return;
                    }
                }
                monsterAttack.attackSuccess.Add(monsterAttack.attackNum);
                CharacterStatus characterStatus = c.GetComponent<CharacterStatus>();
                characterStatus.Damaged(damage);
                Debug.Log(c.transform.name + " Damaged");
            }
        }
    }

    public void CheckAttack()
    {
        if (bonusAttack == 0)
        {
            monsterAttack.attackNum = 0;
            monsterAttack.isAttacking = false;
            monsterStatus.canAttack = true;
            if (monsterMovement.moveWithAgent)
                agent.isStopped = false;
            canAttack = true;
            if (monsterMovement.moveWithAgent)
                agent.ResetPath();
        }
        else if (bonusAttack > 0)
        {
            anim.SetTrigger("attack");
            monsterAttack.attackNum++;
            bonusAttack--;
        }
    }

}
