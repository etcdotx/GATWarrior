using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTrigger : MonoBehaviour
{
    PlayerData playerData;
    UsableItem usableItem;

    public GameObject target;

    private void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        usableItem = GameObject.FindGameObjectWithTag("UsableItem").GetComponent<UsableItem>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Soil"))
        {
            if (other.isTrigger == true)
            {
                other.GetComponentInParent<Soil>().selectedIndicator.SetActive(true);
                target = other.gameObject.transform.parent.gameObject;
                playerData.stateNearSoil = true;
                usableItem.CheckIfItemIsUsable();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Soil"))
        {
            if (other.isTrigger == true)
            {
                other.GetComponentInParent<Soil>().selectedIndicator.SetActive(false);
                target = null;
                playerData.stateNearSoil = false;
                usableItem.CheckIfItemIsUsable();
            }
        }
    }
}
