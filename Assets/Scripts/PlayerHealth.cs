// Purpose: This code handles the player's health
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth; // The maximum amount of health a player has (Used for if certain damage sources should hurt but not kill the player)
    private float currentHealth; // Tracks the current health the player has

    [Header("Damage Settings")]
    [SerializeField] private float damagePerSecond; // Determines how much damage the player recieves every second
    private bool isTakingDamage; // Determines if the player is actively taking damage

    [Header("Regeneration Settings")]
    [SerializeField] private float regenerationDelay; // The time in seconds before health starts regenerating
    [SerializeField] private float regenerationRate; // The amount of health restored per second
    private float regenerationTimer; // Tracks the time since the last damage

    [Header("Respawn Settings")]
    [SerializeField] private float respawnDelay; // Determines how long the code waits before respawning the player
    private bool isRespawning = false; // Determines if the player is already respawning

    private SceneController sceneController; // Reference to the scene controller script
    private PlayerInput playerControls; // Reference to the player's Input Manager

    [Header("Visual Settings")]
    [SerializeField] private GameObject healthVisualOverlay; // Reference to the visual overlay used to track the player's current health 
    [SerializeField] private TextMeshProUGUI healthTracker; // Reference to the health tracker UI element (Optional)

    private Image healthOverlayImage; // Reference to the Image component of the health overlay (UI Panel)

    private AudioManager bgm;
    private SpellSounds spellSFX;

    // Start is called before the first frame update
    void Start()
    {
        // Access the SceneController Script
        sceneController = GameObject.FindObjectOfType<SceneController>();
        // Get the PlayerInput component attached to this GameObject
        playerControls = GetComponent<PlayerInput>();

        // Check if there is a healthTracker assigned
        if (healthTracker == null)
        {
            Debug.Log("Health Tracker UI Element is not assigned. Health will not be displayed as a number on screen.");
        }

        // Get Image Compnenet from healthVisualOverlay
        if (healthVisualOverlay != null)
        {
            healthOverlayImage = healthVisualOverlay.GetComponent<Image>();
        }

        // Set currentHealth to maxHealth
        currentHealth = maxHealth;

        bgm = GameObject.FindObjectOfType<AudioManager>();
        spellSFX = GameObject.FindObjectOfType<SpellSounds>();

    }

    // Update is called once per frame
    void Update()
    {
        // If the player's health is less than or equal to 0, and the player is not already reaspawning
        if(currentHealth <= 0 && !isRespawning)
        {
            // Respawn the player
            StartCoroutine(Respawn());
        }
        // If the player is in a hazard, start taking damage
        if(isTakingDamage)
        {
            // Damage the player
            Damage();
            regenerationTimer = 0f;
        }
        else
        {
            // Track how long the player has not been taking damage for
            regenerationTimer += Time.deltaTime;

            // If the player has not taken any damage for long enough, start regenerating health
            if(regenerationTimer >= regenerationDelay)
            {
                RegenerateHealth();
            }
        }
        // Update the Health Tracker UI
        UpdateHealthTrackerUI();
    }

    // Steadily decreases the player's health over time while isTakingDamage is true
    private void Damage()
    {
        Debug.Log("Player took damage!");

        Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, 3);
        bool isHazard = false;
        foreach (Collider col in cols)
        {
            if (col.CompareTag("Hazard"))
                isHazard = true;
        }
        if (!isHazard)
        {
            isTakingDamage = false;
            return;
        }
        // Do not deal damage if player health is set to -1
        if (currentHealth > 0)
        {
            // Reduce the player's current health
            currentHealth -= damagePerSecond * Time.deltaTime;
        }
    }

    private void RegenerateHealth()
    {
        // If player is not at max health
        if(currentHealth < maxHealth)
        {
            Debug.Log("Player healed!");
            // Increase the player's health by regenerationRate
            currentHealth += regenerationRate * Time.deltaTime;

            // Clamp health so it doesn not exceed the player's maxHealth
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }

    // Updates the HealthTracker UI with the current health
    private void UpdateHealthTrackerUI()
    {
        if (healthOverlayImage != null)
        {
            // Calculate opacity based on current health
            float opacity = 1 - (currentHealth / maxHealth); // 0 for max health, 1 for no health
            Color color = healthOverlayImage.color;
            color.a = opacity; // Set the alpha value of the color
            healthOverlayImage.color = color;
        }

        // Check if healthTracker is assigned, if it is not then do nothing
        if (healthTracker != null)
        {
            // Update Health Tracker's text to reflect current Health Value
            healthTracker.text = "Health: " + currentHealth.ToString("F1");
        }
    }

    // Waits a certain amount of time, before reloading the current scene
    private IEnumerator Respawn()
    {
        Debug.Log("Respawning Player");
        // Set isRespawning to true in order to prevent multiple simultaneous Respawn coroutines from running
        isRespawning = true;

        // Disable Player Inputduring the reapwn sequence
        playerControls.enabled = false;
        currentHealth = 0;
        UpdateHealthTrackerUI();
        // Wait for the respawn delay
        yield return new WaitForSeconds(respawnDelay);

        // Reload the current scene using the SceneController
        sceneController.RestartCurrentScene();

        // Re-Enable player Input post respawn
        playerControls.enabled = true;

        // Set isRespawning to false now that respawn process is complete
        isRespawning = false;
        bgm.Stop();
        spellSFX.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        PushTrigger steam = other.GetComponent<PushTrigger>();
        if (steam != null && !steam.isOn)
        {
            isTakingDamage = false;
        }
        if(steam != null && steam.isOn)
        {
            if (other.CompareTag("Hazard"))
            {
                isTakingDamage = true;
            }
        }
    }



    private void OnTriggerEnter (Collider other)
    {
        // If player makes contact with a hazard
        if(other.CompareTag("Hazard"))
        {
            Debug.Log("Player made contact with a Hazard! They are taking damage!");
            // Set isTakingDamage to true
            isTakingDamage = true;
        }
        // If Player enters the KillPlane
        if(other.CompareTag("Kill Plane"))
        {
            Debug.Log("Player made contact with the Kill Plane! Calling Respawn!");
            // Call the Respawn method

            StartCoroutine(Respawn());
        }
    }

    private void OnTriggerExit (Collider other)
    {
        // If player stops making contact with a hazard
        if (other.CompareTag("Hazard"))
        {
            Debug.Log("Player has stopped making contact with a hazard!");
            // Set isTakingDamage to true
            isTakingDamage = false;
        }
    }
}
