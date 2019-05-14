using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    public GameObject player;
    public WeaponDetail weaponDetail;
    public CharacterCombat characterCombat;
    public BoxCollider boxCol;
    public float[] attackDamage;
    public int attackCount; //darianimasi

    public GameObject hitIndicatorPrefab;
    public GameObject hitIndicatorLocation;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterCombat = player.GetComponent<CharacterCombat>();

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
            MonsterStatus monsterStatus = other.gameObject.GetComponent<MonsterStatus>();
            monsterStatus.ReceiveDamageInfo(attackCount, attackDamage[attackCount], this);
            Debug.Log(other.name + " damaged");
        }
    }

    public void HitSuccessful() {
        Instantiate(hitIndicatorPrefab, hitIndicatorLocation.transform.position, Quaternion.identity, null);
    }
}
