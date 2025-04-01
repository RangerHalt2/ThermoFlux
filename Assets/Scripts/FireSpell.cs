// Purpose: This code handles the player's Fire Spell
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireSpell : MonoBehaviour
{
    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem _fireParticles; // Reference to the Fire Spell's Particle System
    [SerializeField] private float angleAdjustment; // Adds a flat amount of pitch (x rotation) to better fine tune how the fire spell is shot. Negative values cause particles to fired at an upward angle, and positive at a downward
    private ParticleSystem.MainModule fireMainModule;

    [Header("Keybind Settings")]
    private PlayerInput playerControls; // Reference to the player's controls
    private InputAction castFire;

    [Header("References")]
    [SerializeField] private Transform cameraTransform; // Reference to the camera's Transform to determine the pitch of the flames
    [Space]
    [SerializeField] private GameObject playerObj; // Reference to the player object to know what direction to fire flames from
    [SerializeField] private GameObject attackRadius; // Reference to the flame spell's collider to handle hit detection

    // Attack Radius Toggle
    private float toggleInterval = 0.1f; // Interval in seconds between togles for the attack radius
    private float lastToggleTime; // How much time has passed since the attack radius was last toggled

    private SpellSounds sound;
    private float timer;
    private float audioCoodlown = 2;

    // Start is called before the first frame update
    void Start()
    {
        // Get the PlayerInput component attached to this GameObject
        playerControls = GetComponent<PlayerInput>();

        // Enable the Fire Spell
        castFire = playerControls.actions["Gameplay/Fire"];
        castFire.Enable();

        // Cache the MainModule of the particle system for quick access
        fireMainModule = _fireParticles.main;

        attackRadius.SetActive(false);

        // Initialize the last toggle time
        lastToggleTime = 0f; 
        lastToggleTime = 0f; // Initialize the last toggle time

        sound = GameObject.FindAnyObjectByType<SpellSounds>();
       
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the fire button is being held down
        if (castFire.IsPressed())
        {
            if (!_fireParticles.isPlaying)
            {
                // Start emitting particles
                _fireParticles.Play(); 
                // Enable Fire Spell Hitbox
                attackRadius.SetActive(true);
                if(timer < 0)
                {
                    sound.FireSound();
                    timer = audioCoodlown;
                }
            }

            // Toggle attack radius every 'toggleInterval' seconds
            if (Time.time - lastToggleTime >= toggleInterval)
            {
                // Toggle the active state of the attack radius
                attackRadius.SetActive(!attackRadius.activeSelf);
                // Update the last toggle time
                lastToggleTime = Time.time; 
            }
        }
        else
        {
            if (_fireParticles.isPlaying)
            {
                // Stop emitting particles
                _fireParticles.Stop(); 
                // Discable Fire Spell Hitbox
                attackRadius.SetActive(false);
            }
        }
        // Adjust current angle of particle system
        AdjustParticleAngle();
        // Adjust current angle of attack radius
        AdjustAttackRadiusAngle();
        timer -= Time.deltaTime;
    }

    // Adjusts the angle of the particle system to align with the player and camera
    private void AdjustParticleAngle()
    {
        // Get the camera's rotation, specifically the x value for pitch (looking up and down)
        Quaternion cameraRotation = cameraTransform.rotation;

        // Apply the camera's pitch (x-axis rotation) to the particle system
        float cameraPitch = cameraRotation.eulerAngles.x;

        // Get the player's Y-axis rotation from PlayerObj
        float playerYaw = playerObj.transform.rotation.eulerAngles.y;

        // Set the particle system's rotation: use camera pitch (x-axis) and player yaw (y-axis)
        _fireParticles.transform.rotation = Quaternion.Euler(cameraPitch + angleAdjustment, playerYaw, 0f);
    }

    // Adjusts the angle of the attack radius to align with the player and camera
    private void AdjustAttackRadiusAngle()
    {
        // Get the camera's rotation, specifically the x value for pitch (looking up and down)
        Quaternion cameraRotation = cameraTransform.rotation;

        // Apply the camera's pitch (x-axis rotation) to the attack radius
        float cameraPitch = cameraRotation.eulerAngles.x;

        // Get the player's Y-axis rotation from PlayerObj to face in front of them
        float playerYaw = playerObj.transform.rotation.eulerAngles.y;

        // Set the attack radius's rotation: use camera pitch (x-axis) and player yaw (y-axis)
        attackRadius.transform.rotation = Quaternion.Euler(cameraPitch + angleAdjustment, playerYaw, 0f);
    }
}
