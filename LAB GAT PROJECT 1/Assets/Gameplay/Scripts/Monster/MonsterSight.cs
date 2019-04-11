using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSight : MonoBehaviour
{
    public MonsterMovement monsterMovement;
    private void Start()
    {
        monsterMovement = GetComponentInParent<MonsterMovement>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < monsterMovement.fieldOfViewAngle * 0.5)
            {
                monsterMovement.playerOnSight = true;
            }
        }
    }
}
