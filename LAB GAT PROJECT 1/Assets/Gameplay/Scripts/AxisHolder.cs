using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisHolder : MonoBehaviour
{
    public static bool isAxisHolded;

    // Update is called once per frame
    void Update()
    {
        if (isAxisHolded == true)
        {
            StartCoroutine(AxisHolded());
        }
    }
    public IEnumerator AxisHolded()
    {
        Debug.Log("Hold");
        yield return new WaitForSeconds(0.2f);
        isAxisHolded = false;
    }
}
