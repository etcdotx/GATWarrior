using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MonsterStatus : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rigid;
    public GameObject player;
    public MonsterMovement monsterMovement;
    public MonsterAttack monsterAttack;
    public CharacterInput characterInput;
    public Interactable interactable;
    public Collider col;
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

    [Header("Attacking")]
    public bool canAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        monsterMovement = GetComponent<MonsterMovement>();
        monsterAttack = GetComponent<MonsterAttack>();
        characterInput = player.GetComponent<CharacterInput>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        if (monsterMovement.moveWithAgent)
            agent = GetComponent<NavMeshAgent>();
        interactable = GetComponent<Interactable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (resetDamage)
        {
            resetDamage = false;
            playerAttack.Clear();
        }
        if (!monsterAttack.isAttacking && monsterMovement.playerOnSight
            && (monsterMovement.awareType || monsterMovement.inCombat) && canAttack)
        {
            StartCoroutine(Attacking());
        }
        if (isDead && !monsterMovement.isFalling)
        {
            StartCoroutine(Dying());
        }
    }

    public void ReceiveDamageInfo(int attackNum, float damage, WeaponStatus weapon) {
        if (hp > 0)
        {
            for (int i = 0; i < playerAttack.Count; i++)
            {
                if (playerAttack[i] == attackNum)
                {
                    return;
                }
            }

            playerAttack.Add(attackNum);
            weapon.HitSuccessful();
            hp -= damage;

            StopCoroutine(ResetCount());
            StartCoroutine(ResetCount());

            bool checkForce = CheckForce(attackNum);

            if (monsterMovement.canBeInterrupted == true && checkForce == false && !monsterAttack.isAttacking)
            {
                monsterMovement.Interrupted();
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


    IEnumerator Attacking()
    {
        canAttack = false;
        int wait = Random.Range(6, 8);
        yield return new WaitForSeconds(wait);
        Debug.Log("attack!");
        monsterAttack.isAttacking = true;
        StopAllCoroutines();
    }

    IEnumerator Dying()
    {
        isDead = false;
        gameObject.tag = "Untagged";
        monsterMovement.StopAllCoroutines();
        monsterAttack.StopAllCoroutines();
        if(monsterMovement.moveWithAgent)
            agent.isStopped = true;
        monsterMovement.wanderingType = false;
        animator.SetBool("isMove", false);
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(dieTime);
        rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        interactable.isInteractable = true;
        StartCoroutine(DestroyThis());
    }

    public bool CheckForce(int attackNum) {
        if (!monsterAttack.isAttacking)
        {
            if (attackNum == 3)
            {
                transform.LookAt(player.transform);
                monsterMovement.Falling();
                return true;
            }
        }
        return false;
    }

    public IEnumerator DestroyThis() {
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }
}
