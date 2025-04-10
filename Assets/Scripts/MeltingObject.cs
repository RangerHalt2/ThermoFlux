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
    [SerializeField] private float vaporizationTime; // How long fire needs to make contact with an object before it vaporizes
    private float vaporizationTimer = 0f; // How long fire has made contact with an object. Used for vaporization

    [Header("Particle Settings")]
    public bool emitsParticles; // Determines whether or not the object emits particles while melting
    [SerializeField] private ParticleSystem meltingParticlePrefab; // Melting particle prefab to instantiate
    private ParticleSystem meltingParticles; // Instance of the melting particles

    [Header("Melting Duration Settings")]
    [SerializeField] private float residualMeltTime; // Determines how long after the Fire stops collding with the object before it stops melting
    private float meltTimer = 0f; // Keeps track of how much time has passed since the object has started to melt
    private bool isMeltingTimerActive = false; // Tracks if the residual melt time is still active
    
    [Header("Natural Melting Settings")]
    [SerializeField] private bool meltsNaturally; // Determines if the ice will melt naturally over time (True) or if it requires the player to melt it (False)
    [SerializeField] private float meltingDelay; // Determines how long before the object will begin to naturally melt
    private float naturalMeltTimer = 0f; // Timer to track time before the object starts melting naturally

    [Header("Misc")]
    public bool canBePushed; // Determines if the object can be pushed by a push trigger
    
    // Start is called before the first frame update
    void Start()
    {
        // If an object is set to melt naturally
        if (meltsNaturally)
        {
            // Set the natural melting delay timer to the specified meltingDelay
            naturalMeltTimer = meltingDelay; 
        }
        else
        {
            // If an object does not melt naturally set the natural meltTimer to 1 so that the object does not start to melt naturally
            naturalMeltTimer = 1;
        }

        // Ensure particles are stopped initially
        if (meltingParticles != null)
        {
            meltingParticles.Stop();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log("vaporization timer: " + vaporizationTimer);
        // If the object can melt naturally, and the natural melt timer is not 0
        if (meltsNaturally && naturalMeltTimer > 0f)
        {
            // Count down the delay before the object starts to melt naturally
            naturalMeltTimer -= Time.deltaTime; 
        }
        // Else if the object can melt naturally, and the timer is over
        else if (meltsNaturally && naturalMeltTimer <= 0f && naturalMeltTimer != -1f)
        {
            naturalMeltTimer = -1f;
            Debug.Log(gameObject.name + " has started to melt naturally!");
        }

        // If fire is colliding...
        if(fireColliding)
        {
            // Reset fireColliding to allow for repeate activation
            fireColliding = false;
            // Activate isMelting to start the melting process
            isMelting = true;
            // Start the residual melting timer
            isMeltingTimerActive = true;
            // Reset melt timer when fire collides
            meltTimer = 0f; 
        }

        // If residual melt time has elapsed, stop the residual melting
        if (isMeltingTimerActive && meltTimer > residualMeltTime)
        {
            isMeltingTimerActive = false;
            isMelting = false;
            Debug.Log(gameObject.name + " has stopped residual melting.");
        }
        else
        {
            meltTimer += Time.deltaTime; 
        }

        // If object is Melting
        if (isMelting || naturalMeltTimer <= 0f)
        {
            // Melt the object
            Melt();

            // Play melting particle effect if not already playing
            if (meltingParticles == null && emitsParticles)
            {
                meltingParticles = Instantiate(meltingParticlePrefab, transform.position, Quaternion.Euler(90f, 0f, 0f));
                meltingParticles.transform.SetParent(transform);
            }
            // Set the particle system's scale to match the parent object
            if (meltingParticles != null && emitsParticles)
            {
                meltingParticles.transform.localScale = transform.localScale;
                meltingParticles.Play();
            }
        }
        else
        {
            // Stop particle effect if not melting anymore
            if (meltingParticles != null && emitsParticles)
            {
                // Stop the Melting Particles
                meltingParticles.Stop();
                // Destroy the melting particles
                Destroy(meltingParticles.gameObject);
                meltingParticles = null;
            }
        }

        // If enough time has passed to vaporize the object
        if (vaporizationTimer >= vaporizationTime)
        {
            // Destroy the object
            RestartPipes();
            Destroy(gameObject);
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
            RestartPipes();
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

    private void OnTriggerStay(Collider other)
    {
        // Check if the object that stays in the collider has the "Fire" tag
        if (other.CompareTag("Fire"))
        {
            // Increment vaporizationTimer
            vaporizationTimer += Time.deltaTime;
        }
    }

    //Very simply this creates a small collider checker and grabs all the colliders in it's area, checking if it's touching a pipe that it needs to turn back on.
    private void RestartPipes()
    {
        Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, 3);
        foreach (Collider col in cols)
        {
            FreezePipe pipe = col.gameObject.GetComponent<FreezePipe>();
            if(pipe != null)
            {
                pipe.TurnOn();
            }
        }
    }


}
