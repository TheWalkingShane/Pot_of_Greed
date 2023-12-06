using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimations : MonoBehaviour
{
    // Hover effect parameters
    public float hoverSpeed = 1f;
    public float hoverAmplitude = 0.1f;
    private Vector3 originalPosition;

    // Timing
    private float timeCounter = 0f;

    void Start()
    {
        // Set original position for the hover effect
        originalPosition = transform.position;
    }

    void Update()
    {
        // Hover effect: Move the coin in a circular pattern on the XZ plane
        Vector3 hoverPosition = originalPosition;
        hoverPosition.y += Mathf.Cos(timeCounter * hoverSpeed) * hoverAmplitude;
        //hoverPosition.z += Mathf.Sin(timeCounter * hoverSpeed) * hoverAmplitude;
        transform.position = hoverPosition;

        // Increment the time counter based on the time since the last frame
        timeCounter += Time.deltaTime;
    }
}
