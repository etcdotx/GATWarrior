using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTrigger : MonoBehaviour
{
    public CharacterInteraction characterInteraction;

    private void Start()
    {
        characterInteraction = gameObject.GetComponentInParent<CharacterInteraction>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Soil"))
        {
            if (other.isTrigger == true)
            {
                other.GetComponentInParent<Soil>().selectedIndicator.SetActive(true);
                characterInteraction.PlantHideButton = false;
                characterInteraction.interactText.text = "Plant";
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
                characterInteraction.PlantHideButton = true;
            }
        }
    }
}
