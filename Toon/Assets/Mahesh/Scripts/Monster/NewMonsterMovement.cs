using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NewMonsterMovement : MonoBehaviour
{

    NavMeshAgent agent;
    Animator animator;
    MonsterAttack monsterAttack;

    [Header("Movement Settings")]
    public bool canMove; //jika dia bisa berpindah posisi
    public bool canBeInterrupted;

    [Header("Wandering")]
    public bool wanderingType; // on jika dia adalah tipe yang bisa wandering / bergerak2 tidak ditempat
    public float minWanderTime; // minimal waktu dia wandering
    public float maxWanderTime; // max waktu dia wandering

    [SerializeField]
    bool isWandering; // mengecek jika dia lagi wandering atau tidak
    public float curWanderingTime; // sudah berapa lama dia wandering

    [Header("Wander Radius")]
    public float minWanderRadius; //min wander radius
    public float maxWanderRadius; //max wander radius

    [SerializeField]
    float wanderingRadius; //besarnya radius range untuk menentukan target position wandering

    [Header("Waiting")]
    public bool waitingType; //on jika dia ada delay setiap akan menentukan target position
    public float minWaitTime; //min waktu diam
    public float maxWaitTime; //max waktu diam

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        monsterAttack = GetComponent<MonsterAttack>();
    }

    private void Start()
    {
        StartCoroutine(SetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (isWandering && !monsterAttack.isAttacking)
            {
                if (curWanderingTime > 0)
                    curWanderingTime -= Time.deltaTime;
                else
                {
                    isWandering = false;
                    animator.SetBool("isMove", false);
                    StartCoroutine(SetDestination());
                }
            }
        }
    }

    /// <summary>
    /// Reset destination untuk wandering
    /// </summary>
    /// <returns></returns>
    IEnumerator SetDestination()
    {
        float randWaitTime = 0;

        //check apakah dia tipe yang diam setelah wandering
        if (waitingType)
        {
            agent.isStopped = true;
            randWaitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(randWaitTime);
            agent.isStopped = false;
        }

        //menghitung radius wandering
        wanderingRadius = Random.Range(minWanderRadius, maxWanderRadius);
        Vector3 destination = RandomNavmeshLocation(wanderingRadius, transform);
        if (monsterAttack.onCombat)
        {
            destination = RandomNavmeshLocation(wanderingRadius, monsterAttack.target);
        }

        //set wandering time baru
        curWanderingTime = Random.Range(minWanderTime, maxWanderTime);        
        agent.SetDestination(destination);

        animator.SetBool("isMove", true);
        isWandering = true;
    }

    /// <summary>
    /// Pilih random point secara sphere dari titik gameobject
    /// </summary>
    /// <param name="radius"></param>
    /// <returns></returns>
    public Vector3 RandomNavmeshLocation(float radius, Transform center)
    {
        Vector3 randomDirection = center.position + (Random.insideUnitSphere * radius);
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
