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

    [Header("Condition")]
    public bool isAttacking=false;

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
        if (isAttacking == false && monsterAttack.isAttacking==false && monsterMovement.playerOnSight == true && (monsterMovement.awareType == true || monsterMovement.inCombat == true))
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
        int ran = Random.Range(6, 8);
        yield return new WaitForSeconds(ran);
        isAttacking = true;
        monsterAttack.isAttacking = true;
        isAttacking = false;
        StopAllCoroutines();
    }

    IEnumerator Dying()
    {
        gameObject.tag = "Untagged";
        monsterMovement.StopAllCoroutines();
        monsterAttack.StopAllCoroutines();
        //agent.enabled = true;
        agent.isStopped = true;
        monsterMovement.wanderingType = false;
        //col.isTrigger = true;
        animator.SetBool("isAttacked", false);
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(dieTime);
        rigid.velocity = new Vector3(0, 0, 0);
        rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        //col.isTrigger = false;
        interactable.isInteractable = true;
        StartCoroutine(DestroyThis());
    }

    public bool CheckForce(int attackNum) {
        if (attackNum == 3)
        {
            //monsterMovement.StopAllCoroutines();
            //monsterMovement.StartCoroutine(monsterMovement.Falling());
            Vector3 playerForce = new Vector3(0, flyForce, 0) + player.transform.forward*force;
            rigid.AddForce(playerForce, ForceMode.Acceleration);
            Debug.Log("hard hit");
            return true;
        }
        return false;
    }

    public IEnumerator DestroyThis() {
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }
}
