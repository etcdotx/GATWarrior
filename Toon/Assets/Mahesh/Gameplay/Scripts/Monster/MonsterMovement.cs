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

    [Header("Wander Holder")]
    public bool resetHold;
    public float wanderHoldTime;
    public float minWanderHoldTime;
    public float maxWanderHoldTime;

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
    public Coroutine recover;

    [Header("Change direction without agent")]
    public bool isRotating;
    public Vector3 newTargetPos;

    [Header("Set Condition")]
    public bool moveWithAgent;
    public bool wanderingType;
    public bool awareType;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        monsterStatus = GetComponent<MonsterStatus>();
        monsterAttack = GetComponent<MonsterAttack>();
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        //wander
        float wanderTimer = Random.Range(2, 5);
        wanderTime = wanderTimer;

        if(wanderingType)
            StartCoroutine(WanderHolder());
    }

    // Update is called once per frame
    void Update()
    {
        if (monsterStatus.hp > 0 && !isInterrupted && !isFalling)
        {
            if (moveWithAgent)
            {
                MoveWithAgent();
            }
            else
            {
                MoveWithAnimation();
            }
        } 
    }

    void MoveWithAgent() {
        if (wanderingType)
        {
            if (playerOnSight && (awareType || inCombat))
            {
                LookAtPlayer();
                if (distance > positionGap)
                {
                    agent.SetDestination(player.transform.position);
                }
                else
                {
                    if (wanderTime < wanderTimer)
                        Wandering();
                }
            }
            else
            {
                if (wanderTime < wanderTimer)
                    Wandering();
            }
        }
        else
        {
            if (playerOnSight && (awareType || inCombat))
            {
                LookAtPlayer();
            }
        }
    }

    void MoveWithAnimation() {
        if (isRotating)
        {
            ChangeDirection();
        }
        else {
            if (wanderingType)
            {
                if (playerOnSight && (awareType || inCombat))
                {
                    LookAtPlayer();
                    if (distance > positionGap)
                    {
                        agent.SetDestination(player.transform.position);
                        //Debug.Log(distance + " > " + positionGap);
                    }
                    else
                    {
                        if (wanderTime < wanderTimer)
                            Wandering();
                    }
                }
                else
                {
                    if (wanderTime < wanderTimer)
                        Wandering();
                }
            }
            else
            {
                if (playerOnSight && (awareType || inCombat))
                {
                    LookAtPlayer();
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

    void ChangeDirection()
    {
        Vector3 target = newTargetPos - transform.position;
        target.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), Time.time * rotateSpeed);
    }

    void Wandering()
    {
        isInterrupted = false;
        wanderTime += Time.deltaTime;

        if (wanderTime >= wanderTimer)
        {
            if (resetHold)
            {
                StartCoroutine(WanderHolder());
            }
        }
    }

    IEnumerator WanderHolder()
    {
        isRotating = true;
        resetHold = false;
        agent.isStopped = true;
        wanderHoldTime = Random.Range(minWanderHoldTime, maxWanderHoldTime);
        anim.SetBool("isMove", true);
        yield return new WaitForSeconds(wanderHoldTime);
        isRotating = false;
        wanderTime = 0;
        agent.isStopped = false;
        resetHold = true;

        if (playerOnSight == true)
        {
            wanderRadius = Random.Range(minAttackWanderRadius, maxAttackWanderRadius);
            wanderTimer = Random.Range(minAttackWanderTime, maxAttackWanderTime);
        }
        else
        {
            wanderRadius = Random.Range(minWanderRadius, maxWanderRadius);
            wanderTimer = Random.Range(minWanderTime, maxWanderTime);
        }

        anim.SetBool("isMove", false);
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        if (moveWithAgent)
            agent.SetDestination(newPos);
        else
        {
            if ((!playerOnSight || !awareType) && !inCombat)
            {
                newPos.y = 0;
                newTargetPos = newPos;
            }
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

    public void Interrupted()
    {
        if(recover!=null)
            StopCoroutine(recover);

        inCombat = true;
        playerOnSight = true;
        isInterrupted = true;
        agent.isStopped = true;
        anim.SetTrigger("attacked");
        recover = StartCoroutine(Recover(interruptedRecoveryTime));
    }

    public void Falling()
    {
        if (recover != null)
            StopCoroutine(recover);

        inCombat = true;
        playerOnSight = true;
        agent.isStopped = true;
        anim.SetBool("fall", true);
        isFalling = true;
        recover = StartCoroutine(Recover(fallingRecoveryTime));
    }

    public IEnumerator Recover(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("recovering " + time);
        anim.SetBool("fall", false);
        isInterrupted = false;
        isFalling = false;
        agent.isStopped = false;
        agent.ResetPath();

    }
}
