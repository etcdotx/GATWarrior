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

    [Header("Movement with distance gap (FILL)")]
    public float positionGap;
    float distance;

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
        if (monsterStatus.hp > 0)
        {
            if (!monsterAttack.isAttacking)
            {
                if (wanderingType)
                {
                    if (playerOnSight && (awareType || inCombat))
                    {
                        LookAtPlayer();
                        if (distance > positionGap)
                        {
                            agent.SetDestination(player.transform.position);
                            Debug.Log("in");
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
                }
                else {
                    if (playerOnSight && (awareType || inCombat))
                    {
                        LookAtPlayer();
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

    public void StopCombat() {
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
        inCombat = true;
        playerOnSight = true;
        isInterrupted = true;
        agent.isStopped = true;
        anim.SetTrigger("attacked");
        yield return new WaitForSeconds(interruptedRecoveryTime);
        isInterrupted = false;
        agent.isStopped = false;
        agent.ResetPath();
    }

    public IEnumerator Falling()
    {
        inCombat = true;
        playerOnSight = true;
        agent.isStopped = true;
        anim.SetBool("fall", true);
        isInterrupted = false;
        isFalling = true;
        yield return new WaitForSeconds(fallingRecoveryTime);
        isFalling = false;
        anim.SetBool("fall", false);
        agent.isStopped = false;
        agent.ResetPath();
    }
}
