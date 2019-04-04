using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public InputSetup inputSetup;
    public UsableItem usableItem;
    public Animator charAnim;
    public bool isAttacking;
    public Rigidbody rigid;

    public Vector3 curPos;
    public bool buttonInputHold;
    public int attackCountMax;
    public int attackCount;

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
            if (Input.GetKeyDown(inputSetup.attack))
            {
                isAttacking = true;
                attackCount++;
                charAnim.SetTrigger("attack");

                curPos = transform.position;
                rigid.velocity = Vector3.zero;

                StopAllCoroutines();
                StartCoroutine(Attacking());

                if (attackCount == attackCountMax)
                {
                    StartCoroutine(ButtonInputHold());
                }
            }
        }
    }

    IEnumerator Attacking() {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        Debug.Log("finish");
        attackCount = 0;
    }

    void AttackBehaviour(int attackCount) {
        if(attackCount == 1 || attackCount == 2 || attackCount == 3)
        {
            if (Vector3.Distance(curPos, transform.position) < 1)
            {
                rigid.AddForce(transform.forward* force[attackCount], ForceMode.Acceleration);
            }
            else {
                rigid.velocity = Vector3.zero;
            }
        }
    }

    public IEnumerator ButtonInputHold()
    {
        buttonInputHold = true;
        yield return new WaitForSeconds(0.15f);
        attackCount = 0;
        buttonInputHold = false;
    }

}
