using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTrigger : MonoBehaviour
{
    public static PlantTrigger instance;
    public GameObject target;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Soil"))
        {
            if (other.isTrigger == true)
            {
                other.GetComponentInParent<Soil>().selectedIndicator.SetActive(true);
                target = other.gameObject.transform.parent.gameObject;
                PlayerData.instance.stateNearSoil = true;
                UsableItem.instance.CheckIfItemIsUsable();
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
                PlayerData.instance.stateNearSoil = false;
                UsableItem.instance.CheckIfItemIsUsable();
            }
        }
    }
}
