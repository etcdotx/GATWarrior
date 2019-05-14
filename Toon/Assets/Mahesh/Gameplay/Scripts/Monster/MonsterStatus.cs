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
    public CharacterCombat characterCombat;
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
    public float force;
    public float flyForce;

    [Header("GetMonsterScript")]
    private SphereMonsterStatus sms;

    [Header("Drop rate")]
    public int[] itemID;
    public int[] itemDropPercentage;
    public int[] dropTime;
    public int[] itemDropped;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        monsterMovement = GetComponent<MonsterMovement>();
        monsterAttack = GetComponent<MonsterAttack>();
        characterCombat = player.GetComponent<CharacterCombat>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        interactable = GetComponent<Interactable>();


        if (enemyID == 1) {
            sms = GetComponent<SphereMonsterStatus>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDamaged == true && characterCombat.attackCount==0)
        {
            isDamaged = false;
            playerAttack.Clear();
        }
        if (!monsterAttack.isAttacking && monsterMovement.playerOnSight
            && (monsterMovement.awareType || monsterMovement.inCombat))
        {
            StartCoroutine(Attacking());
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
            isDamaged = true;

            bool checkForce = CheckForce(attackNum);

            if (monsterMovement.canBeInterrupted == true && checkForce == false)
            {
                monsterMovement.StopCoroutine(monsterMovement.Interrupted());
                monsterMovement.StartCoroutine(monsterMovement.Interrupted());
            }

            if (hp <= 0)
            {
                StartCoroutine(Dying());
            }
        }
    }

    IEnumerator Attacking()
    {
        int wait = Random.Range(6, 8);
        yield return new WaitForSeconds(wait);
        Debug.Log("attack!");
        monsterAttack.isAttacking = true;
        StopAllCoroutines();
    }

    IEnumerator Dying()
    {
        gameObject.tag = "Untagged";
        monsterMovement.StopAllCoroutines();
        monsterAttack.StopAllCoroutines();
        agent.isStopped = true;
        monsterMovement.wanderingType = false;
        animator.SetBool("isAttacked", false);
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(dieTime);
        rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        interactable.isInteractable = true;
        StartCoroutine(DestroyThis());
    }

    public bool CheckForce(int attackNum) {
        if (attackNum == 3)
        {
            transform.LookAt(player.transform);
            monsterMovement.StopAllCoroutines();
            monsterMovement.StartCoroutine(monsterMovement.Falling());
            return true;
        }
        return false;
    }

    public IEnumerator DestroyThis() {
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }
}
