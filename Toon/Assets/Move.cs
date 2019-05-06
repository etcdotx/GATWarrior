using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Move : MonoBehaviour
{

    private void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
           
            transform.Translate(0, 0, 5 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
  
            transform.Translate(-5 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
      
            transform.Translate(5 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {

            transform.Translate(0, 0, -5 * Time.deltaTime);
        }

      
    }
}
