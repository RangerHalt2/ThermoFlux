// Purpose: This code controls the behavior of the third person camera
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation; // Reference to the player's orientation. Link to the Orientation child object attatched to the player
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private Transform playerObj; // Reference to the player object. Link to the player's model 
    [SerializeField] private Rigidbody rb; // Reference to the player's rigidbody

    [SerializeField] private PlayerInput playerControls; // Reference to the player's Input Manager
    private PlayerMovement playerMovement; // Reference to the player movement script
    
    [Header("Settings")]
    [SerializeField] private float rotationSpeed; // The speed at which the camera can rotate

    private Vector2 moveInput; // To store movement input

    // Start is called before the first frame update
    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();

        // Lock Player's Mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Get the movement input
         moveInput = playerMovement.moveAction.ReadValue<Vector2>(); 

        // Handle movement if there is input
        if (moveInput != Vector2.zero)
        {
            // Calculate movement direction based on input and orientation
            Vector3 inputDir = orientation.forward * moveInput.y + orientation.right * moveInput.x;

            // Rotate the player object based on movement input direction
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
