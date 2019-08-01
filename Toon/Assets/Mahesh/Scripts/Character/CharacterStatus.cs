using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    public static CharacterStatus instance;

    
    //dipangil ketika sedang terkena serangan atau blocking
    public Animator animator;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// function ketika terkena damage
    /// </summary>
    /// <param name="dmg">menentukan damage yang diterima</param>
    /// <param name="sourceVec">menentukan animasi terserang dari arah sumber</param>
    public void Damaged(float dmg, Vector3 sourceVec)
    {
        animator.SetFloat("damagedPosY", 0);
        animator.SetFloat("damagedPosX", 0);
        Vector3 impact = sourceVec - transform.position;
        if (Mathf.Abs(impact.x) > Mathf.Abs(impact.z))
        {
            if (impact.x > 0)
                animator.SetFloat("damagedPosX", 1);
            else if (impact.x < 0)
                animator.SetFloat("damagedPosX", -1);
        }
        else {
            if (impact.y > 0)
                animator.SetFloat("damagedPosY", 1);
            else if (impact.y < 0)
                animator.SetFloat("damagedPosY", -1);
        }

        animator.SetTrigger("damaged");
        PlayerStatus.instance.curHealth -= dmg;
        PlayerStatus.instance.RefreshHp();
    }

    /// <summary>
    /// function ketika sedang blocking
    /// </summary>
    /// <param name="dmg">menentukan damage yang diterima</param>
    /// <param name="sourceVec">menentukan animasi terserang dari arah sumber</param>
    public void Blocked(float dmg, Vector3 sourceVec)
    {
        animator.SetTrigger("blocking");
    }
}
