// Purpose: This code handles push triggers and is used for fans and conveyor belts
// Author: Ryan Lupoli
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTrigger : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float pushForce; // The amount of force applied to an object which enters a push trigger
    [SerializeField] private Vector3 pushDirection; // Determines the direction force will be applied. Put a positive or negative 1 for the respecrive axis you would like to push along

    [SerializeField] private string[] tagsToPush;

    [HideInInspector]public bool isOn;

    public float cooldown;
    private float timer;

    private void Start()
    {
        isOn = true;
    }

    private void Update(){
        timer -= Time.deltaTime;
    }

    // While an object is in the trigger
    private void OnTriggerStay(Collider other)
    {
        if (!isOn) return;

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
                timer = cooldown;
                Debug.Log("Added force");
            }
            return;
        }

        //Check for each tag in the array if the object matches
        //This code is essentially the same as the player, but it's seperated from the player push triggers
        for(int i = 0; i < tagsToPush.Length; i++)
        {
            if (other.CompareTag(tagsToPush[i]))
            {
                //If it matches, it's an object that should be pushed
                Rigidbody objRb = other.GetComponent<Rigidbody>();

                if (objRb != null)
                {
                    objRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Force);
                }
            }
        }

    }
}
