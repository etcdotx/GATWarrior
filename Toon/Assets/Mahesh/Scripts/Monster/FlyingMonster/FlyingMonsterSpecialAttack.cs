using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonsterSpecialAttack : MonoBehaviour
{
    public GameObject tornadoPrefab;

    public void Tornado() {
        GameObject tornado = Instantiate(tornadoPrefab, transform.position, Quaternion.identity, null);
        tornado.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
    }
}
