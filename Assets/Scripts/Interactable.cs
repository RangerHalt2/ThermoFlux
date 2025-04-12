// Purpose: This code determines the behavior of interactables
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{

    private SceneController sceneController; // Reference to the scene Controller

    [SerializeField] private Purpose purpose; // What does the object do

    public enum Purpose
    {
        returnToHub,

    }

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    
    // Sets the level completion flag for the current level
    void SetLevelCompletionFlag()
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


    // Does something outlined by the object's enum
    public void Activate()
    {
        switch(purpose)
        {
            case Purpose.returnToHub:
                // Set the current level as complete
                SetLevelCompletionFlag();
                // Return player to hub level
                Debug.Log("Player teleported to Hub!");
                sceneController.GoToHubLevel();
                return;
            default:
                return;
        }

    }
}
