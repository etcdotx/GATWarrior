using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    public float destTime;

    private void Update()
    {
        Destroy(gameObject, destTime);
    }
}
