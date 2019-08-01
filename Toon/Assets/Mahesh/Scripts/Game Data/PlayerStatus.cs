using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    //status ui
    public GameObject playerStatusView;

    [Header("Health Point")]
    public float maxHealth;
    public float curHealth;

    //indicator darah
    public GameObject healthIndicator;
    public Image curHealthImg;

    //untuk lerping darah
    public float healthLerpingSpeed;
    float targetFillAmount;
    bool healthLerping;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        playerStatusView = transform.GetChild(0).Find("PlayerStatusView").gameObject;
        curHealth = maxHealth;
    }

    void Start()
    {
        playerStatusView.SetActive(true);
        RefreshHp();
    }

    private void Update()
    {
        //supaya darah player smooth
        if (healthLerping)
        {
            curHealthImg.fillAmount = Mathf.Lerp(curHealthImg.fillAmount, targetFillAmount, Time.deltaTime*healthLerpingSpeed);
            if (Mathf.Abs(curHealthImg.fillAmount - targetFillAmount) <= 0.005)
            {
                curHealthImg.fillAmount = targetFillAmount;
                healthLerping = false;
            }
        }
    }

    /// <summary>
    /// function untuk refresh hp setiap ada perubahan
    /// </summary>
    public void RefreshHp() {
        targetFillAmount = curHealth / maxHealth;
        if (curHealth < 0)
            curHealth = 0;
        healthLerping = true;
    }
}
