using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    public MonsterStatus monsterStatus;
    public MonsterAttack monsterAttack;
    public Animator anim;
    private Transform target;
    public Rigidbody rigid;
    private NavMeshAgent agent;

    public GameObject player;
    public bool playerOnSight = false;

    [Header("Movement (FILL)")]
    public float moveSpeed;
    public float positionGap;
    public float breakGap;
    public float distance;

    [Header("View (FILL)")]
    public float fieldOfViewAngle;
    public float rotateSpeed;

    [Header("Wandering")]
    public float wanderRadius;
    public float wanderTimer;
    public float wanderTime;

    [Header("Normal Wandering (FILL)")]
    public float minWanderRadius;
    public float maxWanderRadius;
    public float minWanderTime;
    public float maxWanderTime;

    [Header("Attack Wandering (FILL)")]
    public float minAttackWanderRadius;
    public float maxAttackWanderRadius;
    public float minAttackWanderTime;
    public float maxAttackWanderTime;

    [Header("Condition")]
    public bool isInterrupted;
    public bool isFalling;
    public bool inCombat;
    public float interruptedRecoveryTime;
    public float fallingRecoveryTime;
    public bool canBeInterrupted;
    public bool canBeFall;

    [Header("Set Condition")]
    public bool wanderingType;
    public bool awareType;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        monsterStatus = GetComponent<MonsterStatus>();
        monsterAttack = GetComponent<MonsterAttack>();
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        //wander
        float wanderTimer = Random.Range(2, 5);
        wanderTime = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled==true)
        {
            if (monsterStatus.isAttacking == false)
            {
                if (wanderingType == true)
                {
                    if (playerOnSight == true && (awareType == true || inCombat == true))
                    {
                        LookAtPlayer();
                        if (distance > positionGap)
                        {
                            agent.ResetPath();
                            transform.position += transform.forward * moveSpeed * Time.deltaTime;
                        }
                        else
                        {
                            Wandering();
                        }
                    }
                    else
                    {
                        Wandering();
                    }

                    if (distance >= breakGap)
                    {
                        StopCombat();
                    }
                }
            }
        }
    }

    void LookAtPlayer() {
        distance = Vector3.Distance(transform.position, player.transform.position);

        Vector3 target = player.transform.position - transform.position;
        target.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), Time.time * rotateSpeed);
    }

    void Wandering()
    {
        isInterrupted = false;
        wanderTime += Time.deltaTime;

        if (wanderTime >= wanderTimer)
        {
            if (playerOnSight == true)
            {
                //attack
                wanderRadius = Random.Range(minAttackWanderRadius, maxAttackWanderRadius);
                wanderTimer = Random.Range(minAttackWanderTime, maxAttackWanderTime);
            }
            else {
                wanderRadius = Random.Range(minWanderRadius, maxWanderRadius);
                wanderTimer = Random.Range(minWanderTime, maxWanderTime);
            }
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            wanderTime = 0;
        }
    }

    void StopCombat() {
        playerOnSight = false;
        distance = 0;
        inCombat = false;
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist + origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        Debug.DrawRay(randDirection, Vector3.up, Color.blue, 1.0f);
        Debug.DrawRay(navHit.position, Vector3.up, Color.red, 1.0f);

        return navHit.position;
    }

    public IEnumerator Interrupted()
    {
        playerOnSight = true;
        isInterrupted = true;
        agent.isStopped = true;
        anim.SetBool("isAttacked", true);
        yield return new WaitForSeconds(interruptedRecoveryTime);
        isInterrupted = false;
        agent.isStopped = false;
        agent.ResetPath();
        anim.SetBool("isAttacked", false);
    }

    public IEnumerator Falling()
    {
        playerOnSight = true;
        agent.isStopped = true;
        agent.enabled = false;
        anim.SetBool("isAttacked", true);
        isFalling = true;
        rigid.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(fallingRecoveryTime);
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        isFalling = false;
        anim.SetBool("isAttacked", false);
        agent.enabled = true;
        agent.isStopped = false;
        agent.ResetPath();
        rigid.velocity = new Vector3(0, 0, 0);
    }
}
