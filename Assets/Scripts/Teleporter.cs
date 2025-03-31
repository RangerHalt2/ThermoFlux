// Purpose: This code allows teleporters to function, allowing the player to change scenes by standing at a set location
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [Header("Teleporter Settings")]
    [SerializeField] private LevelDestination levelDestination; //Determines which level the teleporter should send the player to
    [Space]
    [SerializeField] private bool SetCompletionFlag; // Determines whether or not using the teleporter should set a flag for completing the given level.
    

    private SceneController sceneController; // Reference to the scene controller script

    // Potential Destinations for the Teleporter
    private enum LevelDestination
    {
        Hub,
        LevelOne,
        LevelTwo,
        LevelThree,
        LevelTest,
        LevelWin
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneController = GameObject.FindObjectOfType<SceneController>();
    }

    // Sets the level completion flag for the current level
    void SetLevelCompletionFlag(LevelDestination destination)
    {
        if (BootstrappedData.Instance != null)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            switch (currentSceneName)
            {
                case "LevelOne":
                    Debug.Log("Level 1: Completed!");
                    BootstrappedData.Instance.LevelOneComplete = true;
                    break;
                case "LevelTwo":
                    Debug.Log("Level 2: Completed!");
                    BootstrappedData.Instance.LevelTwoComplete = true;
                    break;
                case "LevelThree":
                    Debug.Log("Level 3: Completed!");
                    BootstrappedData.Instance.LevelThreeComplete = true;
                    break;
                default:
                    Debug.Log("No completion flag to set for this level!");
                    break;
            }
        }
        else
        {
            Debug.LogError("BootstrappedData instance not found!");
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        // If Player enters collision for the teleporter
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has entered a teleporter!");
            // Set the completion flag for the current level
            if (SetCompletionFlag)
            {
                SetLevelCompletionFlag(levelDestination);
            }
            // Teleport player to the destination determineed by LevelDestination
            switch (levelDestination)
            {
                // Hub Level
                case LevelDestination.Hub:
                    Debug.Log("Player teleported to Hub!");
                    sceneController.GoToHubLevel();
                    break;
                // Level One
                case LevelDestination.LevelOne:
                    Debug.Log("Player teleported to Level One!");
                    sceneController.GoToLevelOne();
                    break;
                // Level Two
                case LevelDestination.LevelTwo:
                    Debug.Log("Player teleported to Level Two!");
                    sceneController.GoToLevelTwo();
                    break;
                // Level Three
                case LevelDestination.LevelThree:
                    Debug.Log("Player teleported to Level Three!");
                    sceneController.GoToLevelThree();
                    break;
                // Test Level
                case LevelDestination.LevelTest:
                    Debug.Log("Player teleported to the Test Level!");
                    sceneController.GoToTestLevel();
                    break;
                // Test Level
                case LevelDestination.LevelWin:
                    Debug.Log("Player teleported to the win scene!");
                    sceneController.Win();
                    break; 
                default:
                    Debug.LogError("Improper Location Set. Cannot Teleport Player!");
                    break;
            }
        }
    }
}
