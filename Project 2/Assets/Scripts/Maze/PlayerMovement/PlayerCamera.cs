using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensetivityX;
    public float sensetivityY;

    public Transform oriebtation;

    private float xRotation;
    private float yRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        // locks the cursor in the middle of screen and is invisable
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensetivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensetivityY;

        yRotation += mouseX;
        xRotation -= mouseY;
        
        // clamps the rotation of the camera view to 90 degrees
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        // rotate the camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        oriebtation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
