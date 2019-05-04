using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadStack : MonoBehaviour
{
    public int m_HorizontalStack = 20;
    public float m_CloudHeight;
    public Mesh m_quadMesh;
    public Material m_cloudMaterial;
    float offset;

    public int m_layer;
    public new Camera m_camera;
    private Matrix4x4 m_matrix;

    // Update is called once per frame
    void Update()
    {
        offset = m_CloudHeight / m_HorizontalStack / 2f;
        Vector3 t_startPosition = transform.position + (Vector3.up * (offset * m_HorizontalStack * 2f));
        for (int i = 0; i < m_HorizontalStack; i++) {
            m_matrix = Matrix4x4.TRS(t_startPosition - (Vector3.up * offset * i), transform.rotation, transform.localScale);
            Graphics.DrawMesh(m_quadMesh, m_matrix, m_cloudMaterial, m_layer, m_camera, 0, null, true, false, false);
        }
        
    }
}
