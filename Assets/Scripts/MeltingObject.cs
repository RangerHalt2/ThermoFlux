// Purpose: This code handles the logic for objects which can melt
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltingObject : MonoBehaviour
{
    [Header("Melting Settings")]
    public bool isMelting = false; // Tracks if the object is currently melting
    [SerializeField] private bool fireColliding;
    [SerializeField] private float meltSpeed; // Determines how quickly the object melts. Functions as a reduction in size per second
    [SerializeField] private Vector3 minScale = new Vector3(0f, 0f, 0f);  // The scale at which the object will be destroyed

    [Header("Melting Duration Settings")]
    [SerializeField] private float residualMeltTime; // Determines how long after the Fire stops collding with the object before it stops melting
    private float meltTimer = 0f; // Keeps track of how much time has passed since the object has started to melt
    
    [Header("Natural Melting Settings")]
    [SerializeField] private bool meltsNaturally; // Determines if the ice will melt naturally over time (True) or if it requires the player to melt it (False)
    [SerializeField] private float meltingDelay; // Determines how long before the object will begin to naturally melt
    private float naturalMeltTimer = 0f; // Timer to track time before the object starts melting naturally

    // Start is called before the first frame update
    void Start()
    {
        // If an object is set to melt naturally
        if (meltsNaturally)
        {
            // Set the natural melting delay timer to the specified meltingDelay
            naturalMeltTimer = meltingDelay; 
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // If the object can melt naturally, and the natural melt timer is not 0
        if (meltsNaturally && naturalMeltTimer > 0f)
        {
            // Count down the delay before the object starts to melt naturally
            naturalMeltTimer -= Time.deltaTime; 
        }
        // Else if the object can melt naturally, and the timer is over
        else if (!isMelting && meltsNaturally && naturalMeltTimer <= 0f)
        {
            // Set isMelting to true
            isMelting = true;
            Debug.Log(gameObject.name + " has started to melt naturally!");
        }

        // If fire is colliding then set is Melting to true
        if(fireColliding)
        {
            isMelting = true;
        }

        // If object is Melting
        if (isMelting || meltTimer > 0f)
        {
            // Melt the object
            Melt();
        }
    }

    private void Melt()
    {
        // If the object's current scale is above the minimum, decrease its scale
        if (transform.localScale.x > minScale.x && transform.localScale.y > minScale.y && transform.localScale.z > minScale.z)
        {
            // Gradually reduce the scale of the object
            transform.localScale -= new Vector3(meltSpeed, meltSpeed, meltSpeed) * Time.deltaTime;
        }
        // Else, if the object has reached the minimum acceptable scale
        else if (transform.localScale.x <= minScale.x || transform.localScale.y <= minScale.y || transform.localScale.z <= minScale.z)
        {
            // Destroy the object
            Destroy(gameObject);
            Debug.Log(gameObject.name + " has melted completely and been destroyed!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that enters the collider has the "Fire" tag
        if (other.CompareTag("Fire"))
        {
            Debug.Log("Fire has made contact with a meltable object");
            // Start Melting
            fireColliding = true;
            // Reset Melt Timer
            meltTimer = 0;
        }
    }
}
