using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Transform cam1;
    public Transform cam2;
    private GameObject cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            cam.transform.position = cam2.position;
            cam.transform.rotation = cam2.rotation;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            cam.transform.position = cam1.position;
            cam.transform.rotation = cam1.rotation;
        }
    }
}
