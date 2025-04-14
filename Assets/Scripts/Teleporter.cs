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
    [SerializeField] private bool locked; // Determines whether or not the teleporter is locked A locked teleporter is non functional.
    [SerializeField] private bool SetCompletionFlag; // Determines whether or not using the teleporter should set a flag for completing the given level.

    private AudioManager am;
    private SpellSounds ss;
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
        am = GameObject.FindObjectOfType<AudioManager>();
        ss = GameObject.FindObjectOfType<SpellSounds>();
    }

    // Sets the level completion flag for the current level
    void SetLevelCompletionFlag(LevelDestination destination)
    {
        if (BootstrappedData.Instance != null)
        {
            am.StopBGM();
            ss.Stop();
            string currentSceneName = SceneManager.GetActiveScene().name;
            switch (currentSceneName)
            {
                case "Himeys Level":
                    Debug.Log("Level 1: Completed!");
                    if(BootstrappedData.Instance.LevelOneComplete == false) // Checks if level has already been beaten
                    {
                        BootstrappedData.Instance.levelsCompleted++; // if so, increase levelsCompleted var by 1
                    }
                    BootstrappedData.Instance.LevelOneComplete = true; // sets bool to true 
                    if(BootstrappedData.Instance.levelsCompleted == BootstrappedData.Instance.reqLevels) // checks if that was last required level
                    {
                        BootstrappedData.Instance.gameBeatable = true; // if so, game becomes beatable
                    }
                    break;
                case "BeltLevel":
                    Debug.Log("Level 2: Completed!");
                    if(BootstrappedData.Instance.LevelTwoComplete == false)
                    {
                        BootstrappedData.Instance.levelsCompleted++;
                    }
                    BootstrappedData.Instance.LevelTwoComplete = true;
                    if(BootstrappedData.Instance.levelsCompleted == BootstrappedData.Instance.reqLevels)
                    {
                        BootstrappedData.Instance.gameBeatable = true;
                    }
                    break;
                case "SteamLevel":
                    Debug.Log("Level 3: Completed!");
                    if(BootstrappedData.Instance.LevelThreeComplete == false)
                    {
                        BootstrappedData.Instance.levelsCompleted++;
                    }
                    BootstrappedData.Instance.LevelThreeComplete = true;
                    if(BootstrappedData.Instance.levelsCompleted == BootstrappedData.Instance.reqLevels)
                    {
                        BootstrappedData.Instance.gameBeatable = true;
                    }
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
        if(!locked)
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
        else
        {
            if(BootstrappedData.Instance.gameBeatable == true) // checks to see if game is beatable
            {
                locked = false; // if so, teleporter is active
            }
        }
    }    
}
