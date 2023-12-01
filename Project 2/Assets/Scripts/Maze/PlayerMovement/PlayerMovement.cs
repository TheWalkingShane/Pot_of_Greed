using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Stamina")]
    public Slider staminaBar;
    public float stamina;
    private float maxStamina;
    public float runCost;
    private bool staminaOn = false;
    
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

        // setting up variable for stamina 
        maxStamina = stamina;
        staminaBar.maxValue = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if there is ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHight * 0.5f + 0.2f, whatIsGround);
        
        Inputs();
        SpeedControl();
        StateHandler();
        
        // Tried to solve problem with boolean but got same result of stamina not starting right away when game starts
        /*if (staminaOn)
        {
            moveSpeed = sprintSpeed;
            DecreaseEnergy();
        }
        else
        {
            moveSpeed = walkSpeed;
            IncreaseEnergy();
        }
        
        staminaBar.value = stamina;
        */
        
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
            
            // checks if the stamina is less than zero, then go to walking speed
            // else sprint and decrease energy
            if (stamina < 0)
            {
                //staminaOn = false;
                moveSpeed = walkSpeed;
            }
            else
            {
                //staminaOn = true;
                moveSpeed = sprintSpeed;
                DecreaseEnergy();
            }
            
        }
        // When walking
        else if (grounded)
        {
            state = MovementState.walking;
            //staminaOn = false;
            moveSpeed = walkSpeed;
            IncreaseEnergy();
        }

        staminaBar.value = stamina;
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

    // function to decrease the stamina 
    private void DecreaseEnergy()
    {
        if (stamina != 0)
        {
            stamina -= runCost * Time.deltaTime;
        }
    }
    
    // function to increase the stamina
    private void IncreaseEnergy()
    { 
        stamina += runCost * Time.deltaTime;
    }
}
