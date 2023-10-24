using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    
    public float groundDrag;
    
    [Header("Ground Check")] 
    public float playerHight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Keybinds")] 
    public KeyCode sprintKey = KeyCode.LeftShift;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    public MovementState state;
    
    public enum MovementState
    {
        walking,
        sprinting
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Gets the Rigidbody and freezes the position
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if there is ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHight * 0.5f + 0.2f, whatIsGround);
        
        Inputs();
        SpeedControl();
        StateHandler();
        
        // Handles if their is drag
        if (grounded)
        {
            //Debug.Log("drag");
            rb.drag = groundDrag;
        }
        else
        {
            //Debug.Log("no drag");
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Inputs()
    {
        // gets the horizontal and vertical inputs for the movement
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void StateHandler()
    {
        // When sprinting
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // When walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
    }

    private void MovePlayer()
    {
        // Calculates the movement direction where the player will always walk in the direction they are looking
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        // adds force
        rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        
    }

    private void SpeedControl()
    {
        // Gets the flat velocity of the rigibody
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        // Calculates the max velocity and limits the velocity to that max velocity
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
        }
    }
}
