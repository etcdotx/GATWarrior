using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NewMonsterMovement : MonoBehaviour
{

    public NavMeshAgent agent;
    public Animator anim;
    public GameObject position;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //anim.SetBool("isMove", true);
        StartCoroutine(setDest());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator setDest()
    {
        yield return new WaitForSeconds(3f);
        agent.SetDestination(position.transform.position);
    }
}
