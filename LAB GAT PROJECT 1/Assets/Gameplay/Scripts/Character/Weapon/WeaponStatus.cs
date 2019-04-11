using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    public GameObject player;
    public CharacterAttack characterAttack;
    public BoxCollider boxCol;
    public int[] attackDamage;

    public GameObject hitIndicatorPrefab;
    public GameObject hitIndicatorLocation;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterAttack = player.GetComponent<CharacterAttack>();
        boxCol = GetComponent<BoxCollider>();
        boxCol.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("in");
            MonsterStatus monsterStatus = other.gameObject.GetComponent<MonsterStatus>();
            monsterStatus.ReceiveDamageInfo(characterAttack.attackCount, attackDamage[characterAttack.attackCount], this);
        }
    }

    public void HitSuccessful() {
        Instantiate(hitIndicatorPrefab, hitIndicatorLocation.transform.position, Quaternion.identity, null);
    }
}
