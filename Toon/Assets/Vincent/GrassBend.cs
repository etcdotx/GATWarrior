using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrassBend : MonoBehaviour
{
    public Material mat;
    public string pos;
    public Transform player;

    // Start is called before the first frame update

    private void LateUpdate()
    {
        if (player != null)
        {
            mat.SetVector(pos, player.position);
        }
        else {
            Debug.Log("Test");
        }
    }
}
