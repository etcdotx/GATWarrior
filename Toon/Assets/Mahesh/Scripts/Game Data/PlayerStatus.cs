using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    public GameObject playerStatusView;

    public float maxHealth;
    public float curHealth;
    public GameObject healthIndicator;
    public Image curHealthImg;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        playerStatusView = transform.GetChild(0).Find("PlayerStatusView").gameObject;
        curHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerStatusView.SetActive(true);
        RefreshHp();
    }

    public void RefreshHp() {
        curHealthImg.fillAmount = curHealth / maxHealth;
        if (curHealth < 0)
            curHealth = 0;
    }
}
