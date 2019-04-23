using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    public GameObject player;
    public WeaponDetail weaponDetail;
    public CharacterAttack characterAttack;
    public BoxCollider boxCol;
    public float[] attackDamage;
    public int attackCount; //darianimasi

    public GameObject hitIndicatorPrefab;
    public GameObject hitIndicatorLocation;

    [Header("Force setiap count attack")]
    public float[] force;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterAttack = player.GetComponent<CharacterAttack>();

        boxCol = GetComponent<BoxCollider>();

        RefreshWeapon();
    }

    void RefreshWeapon()
    {
        weaponDetail = transform.GetChild(1).GetComponent<WeaponDetail>();
        attackDamage = new float[weaponDetail.damage.Length];
        attackDamage = weaponDetail.damage;
    }

    private void Update()
    {
        if (characterAttack.isAttacking == true)
        {
            AttackBehaviour();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            MonsterStatus monsterStatus = other.gameObject.GetComponent<MonsterStatus>();
            monsterStatus.ReceiveDamageInfo(attackCount, attackDamage[attackCount], this);
            Debug.Log("in cold");
        }
    }

    public void HitSuccessful() {
        Instantiate(hitIndicatorPrefab, hitIndicatorLocation.transform.position, Quaternion.identity, null);
    }

    public void AttackBehaviour()
    {
        //if (boxCol.enabled == true)
        //{
        //    Collider[] cols = Physics.OverlapBox(boxCol.bounds.center, boxCol.bounds.extents, boxCol.transform.rotation, LayerMask.GetMask("Monster"));

        //    foreach (Collider c in cols)
        //    {
        //        boxCol.enabled = false;
        //        MonsterStatus monsterStatus = c.gameObject.GetComponent<MonsterStatus>();
        //        monsterStatus.ReceiveDamageInfo(attackCount, attackDamage[attackCount], this);
        //        Debug.Log("in cols");
        //    }
        //}

        if (attackCount == 1 || attackCount == 2 || attackCount == 3)
        {
            player.transform.position += player.transform.forward * force[attackCount] * Time.deltaTime;
        }
    }
}
