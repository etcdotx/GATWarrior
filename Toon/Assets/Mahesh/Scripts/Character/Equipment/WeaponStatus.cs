using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    GameObject player;
    //weapon detail sesuai weaponnya
    WeaponDetail weaponDetail;
    //damage reference diambil dari weapon detail
    float[] attackDamage; 

    /// <summary>
    /// di set dari animasi
    /// sebagai penanda untuk monsterstatus receive damageinfo
    /// </summary>
    public int attackCount;

    //prefab hitindicator
    public GameObject hitIndicatorPrefab;
    //posisi hit untuk spawn hit indicator
    Vector3 hitPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RefreshWeapon();
    }

    /// <summary>
    /// Untuk mengambil reference damage dari weapon
    /// </summary>
    void RefreshWeapon()
    {
        weaponDetail = transform.GetChild(1).GetComponent<WeaponDetail>();
        attackDamage = new float[weaponDetail.damage.Length];
        attackDamage = weaponDetail.damage;
    }

    /// <summary>
    /// script damage dari collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            //agar player menghadap ke monster ketika memukul kena
            player.transform.LookAt(other.transform); 

            //ambil hit position untuk di spawn
            hitPosition = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            //musuh menerima damage
            MonsterStatus monsterStatus = other.gameObject.GetComponent<MonsterStatus>();
            monsterStatus.ReceiveDamageInfo(attackCount, attackDamage[attackCount], this);
        }
    }

    /// <summary>
    /// jika hit success, maka akan spawn hit indicator
    /// </summary>
    public void HitSuccessful() {
        Instantiate(hitIndicatorPrefab, hitPosition, Quaternion.identity, null);
    }
}
