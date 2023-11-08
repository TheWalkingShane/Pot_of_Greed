using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Transform cam1;
    public Transform cam2;
    private GameObject cam;
    private float smooth = 7;
    private Vector3 newPosition;
    private Quaternion newRotation;


    // Start is called before the first frame update
    void Start()
    {
        cam = this.gameObject;
        newPosition = transform.position;
        newRotation = transform.rotation;
    }

    
    void Update()
    {
        changePosition();
    }

    private void changePosition()
    {
        Vector3 p1 = cam1.position;
        Quaternion r1 = cam1.rotation;
        Vector3 p2 = cam2.position;
        Quaternion r2 = cam2.rotation;
        
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            newPosition = p2;
            newRotation = r2;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            newPosition = p1;
            newRotation = r1;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smooth);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * smooth);
    }
}
