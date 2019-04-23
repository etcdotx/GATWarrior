using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        usableItem = GameObject.FindGameObjectWithTag("UsableItem").GetComponent<UsableItem>();
        charAnim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        combatMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (combatMode == true)
        {
            if (usableItem.isSelectingItem == false)
            {
                Shielding();
                if (Input.GetKeyDown(inputSetup.attack) && attackCount < attackCountMax)
                {
                    StopAllCoroutines();
                    Attacking();

                    if (attackCount == attackCountMax)
                    {
                        StartCoroutine(AttackInputHold());
                    }
                }
                if (Input.GetKeyDown(inputSetup.useItem)) {
                    combatMode = false;
                    charAnim.SetBool("combatMode", false);
                    charAnim.SetBool("shielding", false);
                }
            }
        }
        else {
            if (usableItem.isSelectingItem == false)
            {
                if (Input.GetKeyDown(inputSetup.attack))
                {
                    combatMode = true;
                    charAnim.SetBool("combatMode", true);
                }
            }
        }
    }

    void Attacking()
    {
        charAnim.SetTrigger("attack");
        isAttacking = true;
        StartCoroutine(StopAttack());
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
        yield return new WaitForSeconds(0.3f);
        attackInputHold = false;
    }

    void Shielding() {
        if (Input.GetAxisRaw("LT Button") == 1)
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
