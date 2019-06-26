using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public float maxHealth;
    public float curHealth;
    public GameObject healthIndicator;
    public Image curHealthImg;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        RefreshHp();
    }

    public void RefreshHp() {
        curHealthImg.fillAmount = curHealth / maxHealth;
    }
}
