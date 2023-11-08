using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinVisuals : MonoBehaviour
{
    // Rotation parameters
    public float rotationSpeed = 50f;
    private int spinDirection = 1;

    // Hover effect parameters
    public float hoverSpeed = 1f;
    public float hoverAmplitude = 0.1f;
    private Vector3 originalPosition;

    // Color shift parameters
    public Color startColor = Color.yellow;
    public Color endColor = Color.red;
    private Renderer coinRenderer;
    private float colorChangeSpeed = 1.0f;

    // Timing
    private float timeCounter = 0f;

    void Start()
    {
        // Set original position for the hover effect
        originalPosition = transform.position;

        // Randomize the spin direction
        spinDirection = Random.Range(0, 2) * 2 - 1;

        // Get the renderer component for the color shift effect
        coinRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Rotate the coin around the y-axis
        transform.Rotate(0f, spinDirection * rotationSpeed * Time.deltaTime, 0f, Space.World);

        // Hover effect: Move the coin in a circular pattern on the XZ plane
        Vector3 hoverPosition = originalPosition;
        hoverPosition.y += Mathf.Cos(timeCounter * hoverSpeed) * hoverAmplitude;
        //hoverPosition.z += Mathf.Sin(timeCounter * hoverSpeed) * hoverAmplitude;
        transform.position = hoverPosition;

        // Increment the time counter based on the time since the last frame
        timeCounter += Time.deltaTime;

        // Color shift effect: Change the color of the coin over time
        float lerpFactor = Mathf.PingPong(Time.time * colorChangeSpeed, 1);
        coinRenderer.material.color = Color.Lerp(startColor, endColor, lerpFactor);
    }
}
