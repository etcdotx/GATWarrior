using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHolder : MonoBehaviour
{
    public static bool isInputHolded;

    // Update is called once per frame
    void Update()
    {
        if (isInputHolded == true)
        {
            StartCoroutine(InputHolded());
        }
    }

    public IEnumerator InputHolded()
    {
        yield return new WaitForSeconds(0.1f);
        isInputHolded = false;
    }
}
