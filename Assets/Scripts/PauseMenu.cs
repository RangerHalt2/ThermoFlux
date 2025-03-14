// Purpose: This code handles the logic for the pause menu
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    // Pause Panel Game Object
    [Header("References")]
    [SerializeField] private GameObject pausePanel; // Reference to the he pause menu UI asset
    [SerializeField] private GameObject player; // Reference to the player in order to access the player's inputs

    [Header("Keybind Settings")]
    [SerializeField] private PlayerInput playerControls; // Reference to the player's Input Manager
    private InputAction pause;

    public static bool isPaused = false; // Determines whether or not the game is currently paused

    // Start is called before the first frame update
    void Start()
    {
        // Initialize actions from PlayerControls assets
        pause = playerControls.actions["Gameplay/Pause"];
        // Disables pause screen
        pausePanel.SetActive(false);
        // Ensure the scene does not start paused
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(pause.triggered)
        {
            // If the game is currently paused...
            if(isPaused)
            {
                // Lock player's cursor to the center of the screen
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Debug.Log("Disabled the cursor to allow for smooth gameplay.");
                // Resume the game
                ResumeGame();
                Debug.Log("Game Un-Paused.");
            }
            // If the game is not currently paused...
            else
            {
                //Enable the Cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("Enabled the cursor to allow for menu navigation.");
                // Pause the game
                PauseGame();
                Debug.Log("Game Paused.");
            }
        }
    }

    // Pauses the game
    public void PauseGame()
    {
        // Un-hides the pause panel
        pausePanel.SetActive(true);
        // Sets timescale to 0 to prevent gameplay
        Time.timeScale = 0f;
        // Update isPaused to show game is paused
        isPaused = true;
    }

    // Resumes the game
    public void ResumeGame()
    {
        // Hides the pause panel
        pausePanel.SetActive(false);
        // Sets timescale to return speed to normal
        Time.timeScale = 1f;
        // Update isPaused to show game is not paused
        isPaused = false;
    }
}
