// Purpose: This code handles the logic for flammable objects
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObject : MonoBehaviour
{
    [Header("Flammability Settings")]
    public bool isOnFire; // Tracks if the object is currently on Fire
    [SerializeField] private bool isDestructable; // Determines if the object can be destroyed with fire.
    [SerializeField] private float burningTime; // Determines how long a given object can burn before it is destroyed. If burningTime = -1, then object will burn forever
    private float elapsedTime; // Determines how long has passed since object was lit on fire

    [Header("Particle Settings")]
    public bool emitsParticles; // Determines whether or not the object emits particles while on fire
    [SerializeField] private ParticleSystem fireParticlePrefab; // Fire particle prefab to instantiate
    private ParticleSystem fireParticles; // Instance of the fire particles
    
    [Header("Fire Spread Settings")]
    public bool canSpread; // Determines if the fire is allowed to spread to other flammable objects
    [SerializeField] private float fireSpreadRadius; // Radius within which the fire can spread to other flammable objects
    [SerializeField] private float fireSpreadInterval; // Interval to check for nearby flammable objects
    private float fireSpreadTimer; // Timer to control fire spread checks
    [HideInInspector] public float iceBuffer; //Timer will prevent it from being lit if it has recently been extinguished
    [SerializeField] private float iceTimer = 2;

    // Start is called before the first frame update
    void Start()
    {
        // Start fireSpreadTimer at the set interval
        fireSpreadTimer = fireSpreadInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // If object is on fire and burning time is not set to -1
        if(isOnFire && burningTime != -1)
        {
            // Increased elapsed time based on time passed
            elapsedTime += Time.deltaTime;

            // Check if object has burned for the specified amount of time
            if (elapsedTime >= burningTime)
            {
                // If object is destructable, destroy it
                if(isDestructable)
                {
                    Debug.Log(gameObject.name + " has been burnt up and destroyed!");
                    Destroy(this.gameObject);
                }
                // Else automatically extinguish the object
                else
                {
                    Extinguish();
                }
            }
        }
        
        // If the fire is allowed to spread
        if (canSpread)
        {
             // And if the object is on fire
            if (isOnFire)
            {
                // And if the fire spread timer is at 0
                if (fireSpreadTimer <= 0f)
                {
                    // Spread fire to nearby objects
                    SpreadFire();
                    // Reset the fire Spread Timer
                    fireSpreadTimer = fireSpreadInterval;
                }
                // Else decrement the timer
                else
                {
                    // Decrease the fireSpread timer
                    fireSpreadTimer -= Time.deltaTime;
                }
            }
        }

        iceBuffer -= Time.deltaTime;

    }

    // Lights an object on fire
    private void Ignite()
    {
        // Set isOnFire to true
        isOnFire = true;

        // If fire particles are not instantiated, and the object is supposed to emit particles, create them
        if (fireParticles == null && emitsParticles)
        {
            fireParticles = Instantiate(fireParticlePrefab, transform.position, Quaternion.identity);
            // Set the fire particles as a child of the object
            fireParticles.transform.SetParent(transform);
            // Update the scale of the fire particles to match the parent
            fireParticles.transform.localScale = transform.localScale;
        }
        // Play the fire particles
        fireParticles.Play(); 

        Debug.Log(gameObject.name + " has been lit on fire!");
    }

    private void Extinguish()
    {
        // Set isOnFire to false
        isOnFire = false;

        // Stop and destroy fire particles if they exist
        if (fireParticles != null && emitsParticles)
        {
            // Stop the fireParticles
            fireParticles.Stop();
            // Destroy the fire particles after extinguishing
            Destroy(fireParticles.gameObject);
            fireParticles = null;
        }
        // Reset elapsedTime to keep burn time cosistent across ignitions
        elapsedTime = 0;

        // If the object can spread fire
        if(canSpread)
        {
            // Reset the timer manually
            fireSpreadTimer = fireSpreadInterval;
        }

        iceBuffer = iceTimer;
        
        Debug.Log(gameObject.name + " has been extinguished!");
    }

    // Function to handle spreading fire to nearby flammable objects
    private void SpreadFire()
    {
        // Find all colliders in the fire spread radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, fireSpreadRadius);
        foreach (Collider collider in hitColliders)
        {
            // Check if the collider belongs to a flammable object and is not already on fire
            if (collider.CompareTag("Flammable Object"))
            {
                FlammableObject flammableObj = collider.GetComponent<FlammableObject>();
                if (flammableObj != null && !flammableObj.isOnFire && flammableObj.iceBuffer <= 0)
                {
                    // Ignite the nearby object
                    flammableObj.Ignite();
                    Debug.Log("Nearby " + gameObject.name + " has caught fire!");
                }
            }
        }
    }

    public void IceHit()
    {
        // And if object is on fire 
        if (isOnFire)
        {
            // Extinguish the object
            Extinguish();
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        // If the object is hit by a trigger tagged as Fire
        if(other.CompareTag("Fire"))
        {
            // And if object is not already on fire 
            if(!isOnFire)
            {
                // Ignite the object
                Ignite();
            }
        }
        // If the object is hit by a trigger tagged as Ice
        if(other.CompareTag("Ice"))
        {
            // And if object is on fire 
            if(isOnFire)
            {
                // Extinguish the object
                Extinguish();
            }
        }
    }
}
