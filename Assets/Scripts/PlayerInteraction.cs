// Purpose: This code allows the player to interact with their surroundings
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInput playerControls; // Reference to the player's Input Manager
    private InputAction interactAction; // Reference to the player's interact action

    public float interactionDistance = 10f; // How far the player can interact. Distance is from the camera not the player

    public Camera playerCamera; // The camera to check for raycasting
    private RaycastHit hit;

    void Start()
    {
        interactAction = playerControls.actions["Gameplay/Interact"];
        interactAction.Enable();
    }

    void Update()
    {
        // Cast a ray from the camera forward
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // If the raycast hits an interactable
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if(interactable != null)
            {
                if (interactAction.triggered)
                {
                    interactable.Activate();
                }
            }
        }
    }
}
