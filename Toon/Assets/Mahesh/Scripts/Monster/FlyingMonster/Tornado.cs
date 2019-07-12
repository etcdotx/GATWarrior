using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public float speed;
    public float dps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStatus.instance.curHealth -= dps;
            PlayerStatus.instance.RefreshHp();
            Debug.Log("damaged");
        }
    }
}
