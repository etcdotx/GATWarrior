using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MonsterStatus : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rigid;
    public NewMonsterMovement monsterMovement;
    public MonsterAttack monsterAttack;
    public Interactable interactable;
    public NavMeshAgent agent;

    [Header("Setup")]
    public int enemyID;
    public float maxHp;
    public float dieTime;

    [Header("Status")]
    public float hp;

    [Header("Damaged")]
    public List<int> playerAttack = new List<int>();
    public bool isDamaged = false;
    public bool resetDamage;
    public bool isDead;

    [Header("Drop rate")]
    public int[] itemID;
    public int[] itemDropPercentage;
    public int[] dropTime;
    public int[] itemDropped;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        monsterMovement = GetComponent<NewMonsterMovement>();
        monsterAttack = GetComponent<MonsterAttack>();
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        interactable = GetComponent<Interactable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (resetDamage)
        {
            resetDamage = false;
            playerAttack.Clear();
        }
        if (isDead)
        {
            StartCoroutine(Dying());
        }
    }

    public void ReceiveDamageInfo(int attackNum, float damage, WeaponStatus weapon) {
        if (hp > 0)
        {
            if (!monsterAttack.passive)
            {
                monsterAttack.target = GameObject.FindGameObjectWithTag("Player").transform;
                monsterAttack.onCombat = true;
            }

            for (int i = 0; i < playerAttack.Count; i++)
            {
                if (playerAttack[i] == attackNum)
                {
                    Debug.Log("in1");
                    return;
                }
            }

            playerAttack.Add(attackNum);
            weapon.HitSuccessful();
            hp -= damage;

            StopCoroutine(ResetCount());
            StartCoroutine(ResetCount());

            //bool checkForce = CheckForce(attackNum);

            if (monsterMovement.canBeInterrupted == true && !monsterAttack.isAttacking)
            {
                animator.SetTrigger("attacked");
            }

            if (hp <= 0)
            {
                isDead = true;
            }
        }
    }

    IEnumerator ResetCount() {
        yield return new WaitForSeconds(1f);
        resetDamage = true;
    }

    IEnumerator Dying()
    {
        isDead = false;
        gameObject.tag = "Untagged";
        monsterMovement.StopAllCoroutines();
        monsterAttack.StopAllCoroutines();
        agent.isStopped = true;
        monsterMovement.canMove = false;
        animator.SetBool("isMove", false);
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(dieTime);
        rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        interactable.isInteractable = true;
        StartCoroutine(DestroyThis());
    }

    public IEnumerator DestroyThis() {
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }
}
