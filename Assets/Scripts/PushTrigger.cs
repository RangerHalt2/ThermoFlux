// Purpose: This code handles push triggers and is used for fans and conveyor belts
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTrigger : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float pushForce; // The amount of force applied to an object which enters a push trigger
    [SerializeField] private Vector3 pushDirection; // Determines the direction force will be applied. Put a positive or negative 1 for the respecrive axis you would like to push along

    // While an object is in the trigger
    private void OnTriggerStay(Collider other)
    {
        // If the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Attempt to get the Rigidbody component of the player
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            
            // If the player has a rigidbody
            if (playerRb != null)
            {
                // Apply constant force to the object in the pushDirection
                playerRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Force);
            }
        }

        // If the object entering the trigger is an ice block
        if (other.CompareTag("Ice"))
        {
            MeltingObject meltingObject = other.GetComponent<MeltingObject>();

            if (meltingObject != null && meltingObject.canBePushed)
            {
                // Attempt to get the Rigidbody component of the player
                Rigidbody iceRb = other.GetComponent<Rigidbody>();
            
                // If the player has a rigidbody
                if (iceRb != null)
                {
                    // Apply constant force to the object in the pushDirection
                    iceRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Force);
                }
            }
        }
    }

    // When an object exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ice"))
        {
            MeltingObject meltingObject = other.GetComponent<MeltingObject>();

            if (meltingObject != null && meltingObject.canBePushed)
            {
                Rigidbody iceRb = other.GetComponent<Rigidbody>();

                if (iceRb != null)
                {
                    // Stop movement of ice block
                    iceRb.velocity = Vector3.zero;
                    iceRb.angularVelocity = Vector3.zero; 
                }
            }
        }
    }
}
