using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public CharacterMovement characterMovement;
    public InputSetup inputSetup;
    public UsableItem usableItem;
    public Animator charAnim;
    public Rigidbody rigid;

    [Header("Attack Status")]
    public bool combatMode;
    public bool isShielding;
    public bool isAttacking;
    public bool attackInputHold;

    [Header("Attack (FILL)")]
    public int attackCountMax;
    public int attackCount;

    [Header("Equipment")]
    public GameObject shield;
    public GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        usableItem = GameObject.FindGameObjectWithTag("UsableItem").GetComponent<UsableItem>();
        charAnim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        shield.SetActive(false);
        weapon.SetActive(false);

        combatMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (combatMode == true && characterMovement.isRolling == false)
        {
            if (usableItem.isSelectingItem == false)
            {
                Shielding();
                if (Input.GetKeyDown(inputSetup.attack) && attackCount < attackCountMax && attackInputHold==false)
                {
                    StopAllCoroutines();
                    Attacking();

                    if (attackCount == attackCountMax)
                    {
                        StartCoroutine(AttackInputHold());
                    }
                    else {
                        StartCoroutine(StopAttack());
                    }
                }
                if (Input.GetKeyDown(inputSetup.useItem)) {
                    SheatheWeapon();
                }
            }
        }
        else {
            if (usableItem.isSelectingItem == false)
            {
                if (Input.GetKeyDown(inputSetup.attack))
                {
                    DrawWeapon();
                }
            }
        }
    }

    void DrawWeapon() {

        shield.SetActive(true);
        weapon.SetActive(true);
        combatMode = true;
        charAnim.SetBool("combatMode", true);
    }

    void SheatheWeapon() {
        combatMode = false;
        charAnim.SetBool("combatMode", false);
        charAnim.SetBool("shielding", false);
        shield.SetActive(false);
        weapon.SetActive(false);
    }

    void Attacking()
    {
        charAnim.SetTrigger("attack");
        isAttacking = true;
        attackCount++;
    }

    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.3f);
        attackCount = 0;
        isAttacking = false;
    }

    public IEnumerator AttackInputHold()
    {
        charAnim.SetBool("isAttacking", false);
        attackInputHold = true;
        isAttacking = true;
        yield return new WaitForSeconds(1);
        attackInputHold = false;
        isAttacking = false;
        attackCount = 0;
    }

    void Shielding() {
        if (Input.GetKey(inputSetup.shielding))
        {
            charAnim.SetBool("shielding", true);
            isShielding = true;
        }
        else
        {
            charAnim.SetBool("shielding", false);
            isShielding = false;
        }
    }
}
