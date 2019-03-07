using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util{
    public static void ResetTransform(this Transform transform) {
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        //transform.localScale = Vector3.zero;
    }
}
