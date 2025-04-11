// Purpose: This code handles the Bootstrapper and the storing of Data across Scenes
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    private float moveSpeed; // Determines how fast the player is able to move
    [SerializeField] private float walkSpeed; // Determines the speed the player moves at while walking
    [SerializeField] private float sprintSpeed; // Determines the speed the player moves at while sprinting
    [SerializeField] private float groundDrag; // Determines the amount of drag experienced whilst grounded

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce; // Determines the amount of force applied when jumping
    [SerializeField] private float jumpCooldown; // Determines the amount of time between jumps
    [SerializeField] private float airMultiplier; // Determines how speed is effected while airborne
    bool canJump; // Determines if the player is currently allowed to jump

    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeed; // Determines the player's move speed whilst crouching
    [SerializeField] private float crouchYScale; // Determines the player's Y Scale whilst crouching
    private float startYScale; // Tracks the player's initial/default Y Scale

    [Header("Controls")]
    [SerializeField] private PlayerInput playerControls; // Reference to the player's Input Manager
    private float horizontalInput; // Determines the player's current Horizontal Input
    private float verticalInput; // Determines the player's current Vertical Input
    [HideInInspector] public InputAction moveAction; // Reference to the player's Move action
    private InputAction jumpAction; // Reference to the player's Jump action
    private InputAction crouchAction; // Reference to the player's Crouch action
    private InputAction sprintAction; // Reference to the player's Sprint action

    [Header("Ground Check")]
    [SerializeField] private float playerHeight; // Stores the player's height
    [SerializeField] private LayerMask whatIsGround; // Determines what surfaces are the ground
    bool grounded; // Determines if the player is currently in a grounded state

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle; // The maximum angle a surface can be at before a player ceases being able to move up it
    private RaycastHit slopeHit; // Checks the slope of the current surface
    private bool exitingSlope; // Determines if the player is actively exiting a slope

    [Header("Misc.")]
    [SerializeField] private Transform orientation; // Handles the player's current orientation
    [SerializeField] private float iceSpeed; // Handles speed when touching ice
    

    Vector3 moveDirection; // Determines the direction the player is moving in

    private bool touchingIce; // Determines if player is touching ice

    private Animator pcAnim;

    private float iceMod; // Ice stuff

    private bool walkInput { set; get; }

    Rigidbody rb; // Reference to the player's rigidbody

    public MovementState state; // Stores the player's current Movement State

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    // Start is called before the first frame update
    void Start()
    {
        // Assign the rigidbody and freeze its rotation
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Get player's current Y-Scale
        startYScale = transform.localScale.y;

        // Initialize actions from the PlayerControls asset
        moveAction = playerControls.actions["Gameplay/Move"];
        jumpAction = playerControls.actions["Gameplay/Jump"];
        crouchAction = playerControls.actions["Gameplay/Crouch"];
        sprintAction = playerControls.actions["Gameplay/Sprint"];

        moveAction.performed += context => walkInput = true;
        moveAction.canceled += context => walkInput = false;

        // Enable input actions
        moveAction.Enable();
        jumpAction.Enable();
        crouchAction.Enable();
        sprintAction.Enable();

        pcAnim = GetComponentInChildren<Animator>();

        // Set the player's ability to jump to true
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Checking if player is currently grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        // Getting Player Input
        MyInput();
        // Limit Player's Max Speed
        SpeedControl();
        // Get Player's current State
        StateHandler();
        // Apply Drag
        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        Debug.Log(pcAnim.GetParameter(0).name);
        //Debug.Log("isRunning is " + pcAnim.GetBool(0));
        Debug.Log("Walk Input should be" + walkInput);

        //Sets the bool for the parameters to tell the animator the player is walking
        if (walkInput)
        {
            pcAnim.SetBool("isRunning", true);
        }
        else
        {
            pcAnim.SetBool("isRunning", false);
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Gets player's input
    private void MyInput()
    {
        horizontalInput = moveAction.ReadValue<Vector2>().x;
        verticalInput = moveAction.ReadValue<Vector2>().y;

        // Determines when the player jumps
        if(jumpAction.triggered && canJump && grounded)
        {
            // Disable the player's ability to jump
            canJump = false;

            Jump();

            // Re-enable the ability to jump after jumpCooldown
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        // Determines when player starts crouching
        if(crouchAction.triggered)
        {
            // Changes player Scale
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            // Adds force to player to ground them
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        // Determines when the player stops crouching
        if(!crouchAction.ReadValue<float>().Equals(0f))
        {
            // Changes player Scale back to default
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // Set Mode to crouching
        if(crouchAction.ReadValue<float>() > 0f)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // Set Mode to sprinting
        else if(grounded && sprintAction.ReadValue<float>() > 0f)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // Set Mode to walking
        else if(grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        // Set Mode to air
        else
        {
            state = MovementState.air;
        }
    }

    // Moves the player along the X and Z axis
    private void MovePlayer()
    {
        // Calculate Movement Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Normalize the direction to avoid speed boost on diagonals
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }
        
        // If the player is moving on a Slope
        if(OnSlope() && !exitingSlope)
        {
            // Add force in the direction of the slope
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            // Add downward force to prevent "bumpiness"
            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        iceMod = touchingIce ? iceSpeed:1f;


        // If the player is grounded
        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * iceMod, ForceMode.Force);
        }
        // If the player is airborne
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier * iceMod, ForceMode.Force);
        }

        // Disable Gravity whilst on Slope
        rb.useGravity = !OnSlope();
    }

    // Limits player Speed
    private void SpeedControl()
    {
        // Limit Speed on Slope
        if(OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        // Limiting Speed on ground or in air
        else
        {
            // Getting the flat velocity of the rigidbody
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // If this velocity exceeds the moveSpeed, limit the speed
            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    // Handles the player's ability to Jump
    private void Jump()
    {
        exitingSlope = true;

        // Reset the player's Y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Add jumpForce Once
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // Reset the player's ability to jump
    private void ResetJump()
    {
        canJump = true;

        exitingSlope = false;
    }

    // Check if the player is on a slope
    private bool OnSlope() 
    {
        // Use a raycast to determine if the player is on a sloped surface
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            // Return true if the player is on a sloped surface with an angle less than the maxSlopeAngle, but not 0
            return angle < maxSlopeAngle && angle != 0;
        }
        // If nothing is hit, return false.
        return false;
    }

    // Project normal movement direction to the given slope
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    // Checks to see if player is touching ice
    private void OnTriggerEnter(Collider collider){
        if(collider.CompareTag("Ice")){
            touchingIce = true;
        }
    }

    // Checks to see if player is NOT touching ice
    private void OnTriggerExit(Collider collider){
        if(collider.CompareTag("Ice")){
            touchingIce = false;
        }
    }
}
