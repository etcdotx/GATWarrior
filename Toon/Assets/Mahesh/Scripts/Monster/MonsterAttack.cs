using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAttack : MonoBehaviour
{
    MonsterStatus monsterStatus;
    NewMonsterMovement monsterMovement;
    NavMeshAgent agent;
    Animator animator;
    BoxCollider hitbox;

    public Transform target;
    public bool isAttacking;
    [Header("Normal Attack Settings")]
    public int attackNum;
    public float[] damage; //damage for attacknum
    int bonusAttack; //for chain attack

    [Header("Special Attack Settings")]
    public bool haveSpecialAttack;
    public int specialAttackChance; //chance to special attack
    public int specialAttackNum;
    public float[] specialAttackDamage;

    [Header("Attack Aggro (Choose 1)")]
    public bool passive; //gak nyerang musuh
    public bool aggressive; //liat musuh lgsg nyerang

    [Header("Sight")]
    public bool playerOnSight;
    public bool onCombat;

    [Header("Attack Interval")]
    public float minAttackInterval;
    public float maxAttackInterval;

    public List<int> attackSuccess = new List<int>();
    public bool startAttack;
    public int attackCount;

    private void Awake()
    {
        monsterStatus = GetComponent<MonsterStatus>();
        monsterMovement = GetComponent<NewMonsterMovement>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        hitbox = transform.Find("Hitbox").GetComponent<BoxCollider>();
    }

    private void Start()
    {
        isAttacking = false;
        hitbox.enabled = false;
    }

    public void Update()
    {
        if (monsterStatus.hp > 0)
        {
            if (playerOnSight)
            {
                if (aggressive)
                    onCombat = true;
            }
            else {
                onCombat = false;
            }

            if (onCombat && !startAttack)
            {
                StartCoroutine(Combat());
                startAttack = true;
            }

            if (attackSuccess.Count != 0 && !isAttacking)
                attackSuccess.Clear();
            
            if (isAttacking)
            {
                CheckAttackCollider();
            }
        }
    }

    public IEnumerator Combat() {
        float attackInterval = Random.Range(minAttackInterval, maxAttackInterval);
        yield return new WaitForSeconds(attackInterval);
        if (haveSpecialAttack)
        {
            int getSpecialChance = Random.Range(1, 100);
            Debug.Log(getSpecialChance);
            if (getSpecialChance <= specialAttackChance)
            {
                specialAttackNum = Random.Range(0, specialAttackDamage.Length - 1);
                SpecialAttack(attackNum);
            }
            else
            {
                attackNum = Random.Range(0, damage.Length - 1);
                Attack(attackNum);
            }
        }
        else
        {
            attackNum = Random.Range(0, damage.Length - 1);
            Attack(attackNum);
        }
        isAttacking = true;
    }

    public void Attack(int num)
    {
        if (num == 0)
        {
            bonusAttack = Random.Range(0, 2);
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
            animator.SetTrigger("attack");
            attackCount++;
        }
        else if (num == 1)
        {
            bonusAttack = Random.Range(0, 2);
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
            animator.SetTrigger("attack2");
            attackCount++;
        }
        else if (num == 1)
        {
            bonusAttack = Random.Range(0, 2);
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
            animator.SetTrigger("attack3");
            attackCount++;
        }
    }

    public void SpecialAttack(int num)
    {
        if (num == 0)
        {
            bonusAttack = Random.Range(0, 2);
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
            animator.SetTrigger("specialAttack");
            attackCount++;
            Debug.Log(gameObject.name + " use special attack!");
        }
    }

    public void CheckAttackCollider()
    {
        if (hitbox.enabled)
        {
            //blocked
            Collider[] col = Physics.OverlapBox(hitbox.bounds.center, hitbox.bounds.extents, hitbox.transform.rotation, LayerMask.GetMask("Shield"));
            foreach (Collider c in col)
            {
                for (int i = 0; i < attackSuccess.Count; i++)
                {
                    if (attackSuccess[i] == attackCount)
                    {
                        return;
                    }
                }
                attackSuccess.Add(attackCount);
                CharacterStatus characterStatus = c.GetComponent<Shield>().characterStatus;
                characterStatus.Blocked(damage[attackNum]);
                Debug.Log(c.transform.name + " blocked");
            }

            //damaging
            Collider[] cols = Physics.OverlapBox(hitbox.bounds.center, hitbox.bounds.extents, hitbox.transform.rotation, LayerMask.GetMask("Player"));
            foreach (Collider c in cols)
            {
                for (int i = 0; i < attackSuccess.Count; i++)
                {
                    if (attackSuccess[i] == attackCount)
                    {
                        return;
                    }
                }
                attackSuccess.Add(attackCount);
                CharacterStatus characterStatus = c.GetComponent<CharacterStatus>();
                characterStatus.Damaged(damage[attackNum], transform.position);
                Debug.Log(c.transform.name + " Damaged by " + damage);
            }
        }
    }

    /// <summary>
    /// wajib ada ini atau reset attack di animasi
    /// </summary>
    public void CheckMultipleAttack()
    {
        if (bonusAttack == 0)
        {
            attackCount = 0;
            isAttacking = false;
            startAttack = false;
            monsterMovement.curWanderingTime = 0;
        }
        else if (bonusAttack > 0)
        {
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
            animator.SetTrigger("attack");
            attackCount++;
            bonusAttack--;
        }
    }

    /// <summary>
    /// wajib ada ini atau checkmultiple attack di animasi
    /// </summary>
    public void ResetAttack()
    {
        attackCount = 0;
        isAttacking = false;
        startAttack = false;
        monsterMovement.curWanderingTime = 0;
    }
}
