using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    public GameObject player;
    public WeaponDetail weaponDetail;
    public BoxCollider boxCol;
    public float[] attackDamage;
    public int attackCount; //darianimasi

    public GameObject hitIndicatorPrefab;
    public GameObject hitIndicatorLocation;
    public Vector3 hitPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boxCol = GetComponent<BoxCollider>();

        RefreshWeapon();
    }

    void RefreshWeapon()
    {
        weaponDetail = transform.GetChild(1).GetComponent<WeaponDetail>();
        attackDamage = new float[weaponDetail.damage.Length];
        attackDamage = weaponDetail.damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            //Debug.Log(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
            hitPosition = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            MonsterStatus monsterStatus = other.gameObject.GetComponent<MonsterStatus>();
            monsterStatus.ReceiveDamageInfo(attackCount, attackDamage[attackCount], this);
            Debug.Log(other.name + " damaged");
        }
    }

    public void HitSuccessful() {
        //Debug.Log(hitPosition);
        Instantiate(hitIndicatorPrefab, hitPosition, Quaternion.identity, null);
    }
}
