using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTrigger : MonoBehaviour
{
    public static PlantTrigger instance;

    /// <summary>
    /// digunakan ketika player ingin menggunakan item
    /// sebagai indicator bahwa item tool usable atau tidak
    /// </summary>
    public GameObject target;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    /// <summary>
    /// function untuk add target
    /// menentukan jika player sedang dekat dengan soil (berpengaruh dengan penggunaan item)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Soil"))
        {
            if (other.isTrigger == true)
            {
                other.GetComponentInParent<Soil>().selectedIndicator.SetActive(true);
                target = other.gameObject.transform.parent.gameObject;
                UsableItem.instance.CheckIfItemIsUsable();
            }
        }
    }

    /// <summary>
    /// function untuk meremove target
    /// dan menentukan jika player tidak dekat dengan soil (berpengaruh dengan penggunaan item)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Soil"))
        {
            if (other.isTrigger == true)
            {
                other.GetComponentInParent<Soil>().selectedIndicator.SetActive(false);
                target = null;
                UsableItem.instance.CheckIfItemIsUsable();
            }
        }
    }
}
