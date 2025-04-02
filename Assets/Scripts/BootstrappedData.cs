// Purpose: This code handles the Bootstrapper and the storing of Data across Scenes
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PerformBootstrap
{
    const string SceneName = "Bootstrapper";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        // Traverse currently loaded scenes
        for(int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; ++sceneIndex)
        {
            var candidate = SceneManager.GetSceneAt(sceneIndex);

            // Early out if scene is already loaded
            if (candidate.name == SceneName)
            {
                return;
            }
        }

        // Additvely load bootsrapped scene
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }
}

public class BootstrappedData : MonoBehaviour
{
    // Add Variables to this script to allow them to be shared across scenes.

    [Header("Level Completion")]
    [SerializeField] private bool levelOneComplete; // Determines whether level one was completed
    [SerializeField] private bool levelTwoComplete; // Determines whether level two was completed
    [SerializeField] private bool levelThreeComplete; // Determines whether level three was completed

    public static BootstrappedData Instance {get; private set; } = null;
    
    public bool LevelOneComplete
    {
        get { return levelOneComplete; }
        set { levelOneComplete = value; }
    }

    public bool LevelTwoComplete
    {
        get { return levelTwoComplete; }
        set { levelTwoComplete = value; }
    }

    public bool LevelThreeComplete
    {
        get { return levelThreeComplete; }
        set { levelThreeComplete = value; }
    }

    void Awake()
    {
        // Check if an instance already exists
        if(Instance != null)
        {
            // If another instance exists, destroy it
            Debug.LogError("Found another instance of BootstrappedData on " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        // Prevent Data from being unloaded
        DontDestroyOnLoad(gameObject);
        Instance = this; // Assign the singleton instance
    }
}
