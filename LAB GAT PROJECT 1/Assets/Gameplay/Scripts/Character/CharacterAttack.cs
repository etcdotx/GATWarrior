using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public InputSetup inputSetup;
    public UsableItem usableItem;
    public Animator charAnim;
    public Rigidbody rigid;
    public WeaponStatus weaponStatus;

    [Header("Attack Status")]
    public bool isAttacking;
    public bool attackInputHold;

    [Header("Attack (FILL)")]
    public int attackCountMax;
    public int attackCount;

    // Start is called before the first frame update
    void Start()
    {
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        usableItem = GameObject.FindGameObjectWithTag("UsableItem").GetComponent<UsableItem>();
        charAnim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        weaponStatus = GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<WeaponStatus>();

    }

    // Update is called once per frame
    void Update()
    {
        if (usableItem.isSelectingItem == false)
        {
            if (Input.GetKeyDown(inputSetup.attack) && attackCount<attackCountMax)
            {
                StopAllCoroutines();
                Attacking();

                if (attackCount == attackCountMax)
                {
                    StartCoroutine(AttackInputHold());
                }
            }
            if (isAttacking == true)
            {
                weaponStatus.AttackBehaviour();
            }
        }
    }

    void Attacking()
    {
        charAnim.SetBool("isAttacking",true);
        charAnim.SetTrigger("attack");
        isAttacking = true;
        StartCoroutine(StopAttack());
    }

    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.3f);
        attackCount = 0;
        isAttacking = false;
        charAnim.SetBool("isAttacking", false);
    }

    public IEnumerator AttackInputHold()
    {
        charAnim.SetBool("isAttacking", false);
        attackInputHold = true;
        yield return new WaitForSeconds(0.3f);
        attackInputHold = false;
    }

}
