using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSight : MonoBehaviour
{
    public MonsterAttack monsterAttack;
    public float fieldOfViewAngle;
    private void Start()
    {
        monsterAttack = GetComponentInParent<MonsterAttack>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5)
            {
                monsterAttack.playerOnSight = true;
                monsterAttack.target = other.transform;
            }
        }
    }
}
