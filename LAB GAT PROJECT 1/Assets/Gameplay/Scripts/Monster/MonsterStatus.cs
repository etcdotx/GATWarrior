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
    public CharacterAttack characterAttack;
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

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        monsterMovement = GetComponent<MonsterMovement>();
        monsterAttack = GetComponent<MonsterAttack>();
        characterAttack = player.GetComponent<CharacterAttack>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();

        if (enemyID == 1) {
            sms = GetComponent<SphereMonsterStatus>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDamaged == true && characterAttack.attackCount==0)
        {
            isDamaged = false;
            playerAttack.Clear();
        }
    }

    public void ReceiveDamageInfo(int attackNum, int damage, WeaponStatus weapon) { 
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

        if (monsterMovement.canBeInterrupted == true)
        {
            monsterMovement.StopCoroutine(monsterMovement.Interrupted());
            monsterMovement.StartCoroutine(monsterMovement.Interrupted());
        }

        CheckForce(attackNum);

        if (hp <= 0)
        {
            StartCoroutine(Dying());
        }
    }

    IEnumerator Dying()
    {
        monsterMovement.StopAllCoroutines();
        monsterAttack.StopAllCoroutines();
        agent.enabled = true;
        agent.isStopped = true;
        monsterMovement.wanderingType = false;
        col.enabled = false;
        animator.SetBool("isAttacked", false);
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(dieTime);
        rigid.velocity = new Vector3(0, 0, 0);
        StartCoroutine(DestroyThis());
    }

    public void CheckForce(int attackNum) {
        if (attackNum == 3)
        {
            monsterMovement.StopAllCoroutines();
            monsterMovement.StartCoroutine(monsterMovement.Falling());
            Vector3 playerForce = new Vector3(0, flyForce, 0) + player.transform.forward*force;
            rigid.AddForce(playerForce, ForceMode.Acceleration);
        }
    }

    public IEnumerator DestroyThis() {
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }
}
