using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private CharacterController characterController;

    [SerializeField] private Transform headPoint;

    [Header("Base Movement Fields")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float sprintMultiplier = 2f;

    [SerializeField] private float sensitivity = 10f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private float terminalVelocity = -60f;

    [HideInInspector]
    public Vector3 pushMovement;
    public bool isDecay = false;
    private InputManager inputs;

    private float rotationY;
    private float verticalForce = 0;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private float groundDistance = 0.01f;

    private Animator pcAnim;

    public static PlayerController Instance;

    #endregion

    #region Getters/Setters
    public CharacterController GetCharacterController() { return characterController; }
    //Jitters the player down miniscually to confirm their grounded state.
    public void JitterDown()
    {
        characterController.Move(new Vector3(0f, -0.01f, 0f));
    }
    #endregion

    void Awake() 
    {
  
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //This might better belong on a different script? Unsure
        characterController = GetComponent<CharacterController>();
        inputs = GameObject.FindAnyObjectByType<InputManager>();
        pcAnim = GetComponentInChildren<Animator>();
        if (terminalVelocity > 0) terminalVelocity = -terminalVelocity; //Just makes it negative
        JitterDown();
    }

    private bool IsGrounded()
    {
        bool ret = false;
        ret = Physics.Raycast(groundPoint.position, Vector3.down, groundDistance, whatIsGround);
        return ret;
    }

    private void Move(Vector2 MovementVector)
    {
        //Default Base Case
        Vector3 move = transform.forward * MovementVector.y + transform.right * MovementVector.x;
        move = movementSpeed * (inputs.SprintInput? sprintMultiplier : 1) * Time.deltaTime * move;

        //If the move is greater than 0 after reading the inputs, then we're moving!
        if(move.magnitude > 0)
        {
            pcAnim.SetBool("isRunning", true);
        }
        else
            pcAnim.SetBool("isRunning", false);

        //Debug.Log("Move: " + move);

        verticalForce = verticalForce + gravity * Time.deltaTime;
        verticalForce = Mathf.Clamp(verticalForce, terminalVelocity, -terminalVelocity);

        move += (pushMovement * Time.deltaTime);

        move.y = (verticalForce + pushMovement.y) * Time.deltaTime;

        characterController.Move(move);
        if (characterController.isGrounded){
            pcAnim.SetBool("isInAir", false);
            verticalForce = -0.1f; //Vertical Force must always be slightly negative to double check grounded.
        }
        else
        {
            if(!IsGrounded())
                pcAnim.SetBool("isInAir", true);
        }

        //Debug.Log("Momentum: " + momentum);
        //Debug.Log("Vertical Force: " + verticalForce);
    }

    private void Rotate(Vector2 RotationVector)
    {
        rotationY += RotationVector.x * sensitivity * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckHeadBump();

        DecayMomentum();

        if (inputs.JumpInput == true && characterController.isGrounded)
        {
            //Debug.Log("Attempting Jump");
            verticalForce = jumpForce; //The Jump itself is handled in the Move() method handling gravity, making use of CharacterController instead of RigidBody
        }

        Rotate(inputs.LookInput);
        Move(inputs.MoveInput);

        TimerDecrement();
    }

    private void DecayMomentum()
    {
        if (isDecay)
        {
            pushMovement.x = pushMovement.x - ((pushMovement.x < 0) ? -0.1f : 0.1f);
            pushMovement.z = pushMovement.z - ((pushMovement.z < 0) ? -0.1f : 0.1f);
            pushMovement.y = pushMovement.y - ((pushMovement.y < 0) ? -0.1f : 0.1f);
            if (Mathf.Abs(pushMovement.x) < 0) pushMovement.x = 0;
            if (Mathf.Abs(pushMovement.z) < 0) pushMovement.z = 0;
            if (Mathf.Abs(pushMovement.y) < 0) pushMovement.y = 0;


            if (pushMovement == Vector3.zero) isDecay = false;
        }
    }

    private void CheckHeadBump()
    {
        if (Physics.Raycast(headPoint.position, headPoint.up, 0.1f))
        {
            //Debug.Log("Did HIT");
            if (verticalForce > 0)
                verticalForce = 0;
        }
    }

    //LB: Any and all future timers for the player will be managed in this area
    private void TimerDecrement()
    {
        
    }
}
