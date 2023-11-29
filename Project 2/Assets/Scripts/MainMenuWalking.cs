using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuWalking : MonoBehaviour
{
    private float walkSpeed = 1.5f;

    public Transform start;
    public Transform end;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.gameObject.transform.position += new Vector3(0, 0, walkSpeed) * Time.deltaTime;
        if (this.gameObject.transform.position.z >= end.position.z)
        {
            this.gameObject.transform.position = start.position;
        }
    }
}
