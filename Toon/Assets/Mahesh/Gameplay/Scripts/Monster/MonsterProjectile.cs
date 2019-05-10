using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    public float damage;
    public float destroyTime;
    public float speed;
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterStatus cs = other.transform.GetComponent<CharacterStatus>();
            cs.Damaged(damage);
            Destroy(gameObject);
            Debug.Log("Projectile hit player");
        } else if (other.CompareTag("Shield")){
            Destroy(gameObject);
            Debug.Log("Projectile blocked");
        }
    }
}
