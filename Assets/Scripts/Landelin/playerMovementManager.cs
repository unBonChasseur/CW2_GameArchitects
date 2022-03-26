using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovementManager : MonoBehaviour
{
    // Start is called before the first frame update
    private float x_axis;
    private float y_axis;
    [SerializeField] private float mouse_sensitivity; 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        cameraHandler();
        playerMovementHandler();
    }

    private void cameraHandler()
    {
        x_axis = Input.GetAxis("Mouse X") * Time.deltaTime * mouse_sensitivity;
        y_axis = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.up, x_axis);
    }

    private void playerMovementHandler()
    {
        
    }
}
