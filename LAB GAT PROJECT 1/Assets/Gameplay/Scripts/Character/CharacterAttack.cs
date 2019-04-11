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
    public bool isAttacking;
    public bool attackInputHold;

    [Header("Attack (FILL)")]
    public int attackCountMax;
    public int attackCount;
    public float[] attackAnimationTime;
    public float[] attackAnimationSpeed;

    [Header("Force setiap count attack")]
    public float[] force;

    // Start is called before the first frame update
    void Start()
    {
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        usableItem = GameObject.FindGameObjectWithTag("UsableItem").GetComponent<UsableItem>();
        charAnim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (usableItem.isSelectingItem == false)
        {
            if (isAttacking == true)
            {
                AttackBehaviour(attackCount);
            }
            if (Input.GetKeyDown(inputSetup.attack) && attackCount<attackCountMax)
            {
                StopAllCoroutines();
                StartCoroutine(Attacking());

                if (attackCount == attackCountMax)
                {
                    StartCoroutine(AttackInputHold());
                }
            }
        }
    }

    IEnumerator Attacking()
    {
        charAnim.SetBool("isAttacking",true);
        attackCount++;
        charAnim.SetTrigger("attack");
        charAnim.SetInteger("attackCount",attackCount);
        isAttacking = true;
        yield return new WaitForSeconds((attackAnimationTime[attackCount] / attackAnimationSpeed[attackCount])-0.3f);
        isAttacking = false;
        StartCoroutine(StopAttack());
    }

    IEnumerator StopAttack() {
        yield return new WaitForSeconds(0.3f);
        attackCount = 0;
        isAttacking = false;
    }

    void AttackBehaviour(int attackCount)
    {
        if (attackCount == 1 || attackCount == 2 || attackCount == 3)
        {
            transform.position += transform.forward * force[attackCount] * Time.deltaTime;
        }
    }

    public IEnumerator AttackInputHold()
    {
        attackInputHold = true;
        attackCount = 0;
        yield return new WaitForSeconds(0.3f);
        attackInputHold = false;
    }

}
