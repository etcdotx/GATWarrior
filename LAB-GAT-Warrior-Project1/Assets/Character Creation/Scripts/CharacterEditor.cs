using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterEditor : MonoBehaviour
{
    public Player player;
    enum ApperanceDetail { Gender_Type, Skin_Color, Hair_Type, Hair_Color, Eye_Type }

    [Header("Gender Settings")]
    [SerializeField] private GameObject[] genderType;
    public int genderIndex;
    public string[] genderName;
    public Text genderText;

    [Header("Skin Settings")]
    [SerializeField] private Color32[] skinColor;
    public int skinColorIndex;
    public string[] skinColorName;
    public Text skinColorText;

    [Header("Hair Settings")]
    [SerializeField] private GameObject[] maleHairType;
    [SerializeField] private GameObject[] femaleHairType;
    public int hairIndex;
    public string[] maleHairName;
    public string[] femaleHairName;
    public Text hairTypeName;

    [Header("Hair Color Settings")]
    [SerializeField] private Color32[] hairColor;
    public int hairColorIndex;
    public string[] hairColorName;
    public Text hairColorText;

    [SerializeField] private GameObject[] eyeType;
    public int eyeIndex;

    [Header("Set Object Parent Location")]
    public string genderParent;
    public string hairParent;
    public string eyeParent;

    [Header("Continue Scene Name")]
    public string sceneName;

    GameObject activeGender;
    GameObject activeSkin;
    GameObject activeHair;
    GameObject activeEye;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        applyChange(ApperanceDetail.Gender_Type, 0);
    }

    #region ButtonMethod

    public void plusGender()
    {
        if (genderIndex < genderType.Length - 1)
        {
            genderIndex++;
        }
        else
        {
            genderIndex = 0;
        }
        applyChange(ApperanceDetail.Gender_Type, genderIndex);
    }

    public void minGender()
    {
        if (genderIndex > 0)
        {
            genderIndex--;
        }
        else
        {
            genderIndex = genderType.Length - 1;
        }
        applyChange(ApperanceDetail.Gender_Type, genderIndex);
    }

    public void plusSkin()
    {
        if (skinColorIndex < skinColor.Length - 1)
        {
            skinColorIndex++;
        }
        else
        {
            skinColorIndex = 0;
        }
        applyChange(ApperanceDetail.Skin_Color, skinColorIndex);
    }

    public void minSkin()
    {
        if (skinColorIndex > 0)
        {
            skinColorIndex--;
        }
        else
        {
            skinColorIndex = skinColor.Length - 1;
        }
        applyChange(ApperanceDetail.Skin_Color, skinColorIndex);
    }

    public void plusHair()
    {
        if (genderIndex == 0)
        {
            if (hairIndex < maleHairType.Length - 1)
            {
                hairIndex++;
            }
            else
            {
                hairIndex = 0;
            }
        }
        else
        {
            if (hairIndex < femaleHairType.Length - 1)
            {
                hairIndex++;
            }
            else
            {
                hairIndex = 0;
            }
        }
        applyChange(ApperanceDetail.Hair_Type, hairIndex);
    }

    public void minHair()
    {
        if (genderIndex == 0)
        {
            if (hairIndex > 0)
            {
                hairIndex--;
            }
            else
            {
                hairIndex = maleHairType.Length - 1;
            }
        }
        else
        {
            if (hairIndex > 0)
            {
                hairIndex--;
            }
            else
            {
                hairIndex = femaleHairType.Length - 1;
            }
        }
        applyChange(ApperanceDetail.Hair_Type, hairIndex);
    }

    public void plusHairColor()
    {
        if (hairColorIndex < hairColor.Length - 1)
        {
            hairColorIndex++;
        }
        else
        {
            hairColorIndex = 0;
        }
        applyChange(ApperanceDetail.Hair_Color, hairColorIndex);
    }

    public void minHairColor()
    {
        if (hairColorIndex > 0)
        {
            hairColorIndex--;
        }
        else
        {
            hairColorIndex = hairColor.Length - 1;
        }

        applyChange(ApperanceDetail.Hair_Color, hairColorIndex);
    }

    public void plusEye()
    {
        if (eyeIndex < eyeType.Length - 1)
        {
            eyeIndex++;
        }
        else
        {
            eyeIndex = 0;
        }
        applyChange(ApperanceDetail.Eye_Type, eyeIndex);
    }

    public void minEye()
    {
        if (eyeIndex > 0)
        {
            eyeIndex--;
        }
        else
        {
            eyeIndex = eyeType.Length - 1;
        }
        applyChange(ApperanceDetail.Eye_Type, eyeIndex);
    }

    #endregion

    void applyChange(ApperanceDetail detail, int id)
    {
        //Debug.Log(hairColor[hairColorIndex].r +" "+ hairColor[hairColorIndex].g + " " + hairColor[hairColorIndex].b + " " + hairColor[hairColorIndex].a);

        switch (detail)
        {
            case ApperanceDetail.Gender_Type:
                Destroy(activeGender);
                activeGender = GameObject.Instantiate(genderType[id]);
                activeGender.transform.SetParent(GameObject.Find(genderParent).transform);
                activeGender.transform.ResetTransform();

                genderText.text = genderName[id];

                applyChange(ApperanceDetail.Skin_Color, skinColorIndex);
                applyChange(ApperanceDetail.Hair_Type, 0);
                applyChange(ApperanceDetail.Hair_Color, hairColorIndex);

                player.characterAppearance[0] = genderIndex;
                break;

            case ApperanceDetail.Skin_Color:
                activeGender.GetComponent<Renderer>().material.color = skinColor[id];

                skinColorText.text = skinColorName[id];
                player.characterAppearance[1] = skinColorIndex;
                break;

            case ApperanceDetail.Hair_Type:
                if (activeHair != null)
                {
                    Destroy(activeHair);
                }
                if (genderIndex == 0)
                {
                    activeHair = GameObject.Instantiate(maleHairType[id]);
                    hairTypeName.text = maleHairName[id];
                }
                else
                {
                    activeHair = GameObject.Instantiate(femaleHairType[id]);
                    hairTypeName.text = femaleHairName[id];
                }

                activeHair.transform.SetParent(activeGender.transform.Find(hairParent));
                applyChange(ApperanceDetail.Hair_Color, hairColorIndex);
                player.characterAppearance[2] = hairIndex;
                break;

            case ApperanceDetail.Hair_Color:
                activeHair.GetComponent<Renderer>().material.color = hairColor[id];
                hairColorText.text = hairColorName[id];
                player.characterAppearance[3] = hairColorIndex;
                break;

            case ApperanceDetail.Eye_Type:
                Destroy(activeEye);
                activeEye = GameObject.Instantiate(eyeType[id]);
                activeEye.transform.SetParent(GameObject.Find(eyeParent).transform);
                activeEye.transform.ResetTransform();
                break;
        }
    }

    public void confirmCharacterCreation()
    {
        player.SavePlayer();
        SceneManager.LoadScene(sceneName);
    }
}
