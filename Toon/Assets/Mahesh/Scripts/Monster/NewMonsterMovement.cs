using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NewMonsterMovement : MonoBehaviour
{

    public NavMeshAgent agent;
    public Animator anim;

    [Header("Wandering")]
    public bool wanderingType; // on jika dia adalah tipe yang bisa wandering / bergerak2 tidak ditempat
    public float minWanderTime; // minimal waktu dia wandering
    public float maxWanderTime; // max waktu dia wandering

    [SerializeField]
    bool isWandering; // mengecek jika dia lagi wandering atau tidak
    [SerializeField]
    float curWanderingTime; // sudah berapa lama dia wandering

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
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(SetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        if (isWandering)
        {
            if (curWanderingTime > 0)
                curWanderingTime -= Time.deltaTime;
            else
            {
                isWandering = false;
                anim.SetBool("isMove", false);
                StartCoroutine(SetDestination());
            }
        }
    }

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
        Vector3 destination = RandomNavmeshLocation(wanderingRadius);

        //set wandering time baru
        curWanderingTime = Random.Range(minWanderTime, maxWanderTime);        
        agent.SetDestination(destination);

        anim.SetBool("isMove", true);
        isWandering = true;
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = transform.position + (Random.insideUnitSphere * radius);
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
