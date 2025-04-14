// Purpose: This code allows for the changing of scenes
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [HideInInspector] public bool cheatsJump;
    [HideInInspector] public bool cheatsSpeed;
    // Unlocks and renables the cursor for the purpose of menu navigation
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Loads the Main Menu
    public void MainMenu ()
    {
        EnableCursor();
        SceneManager.LoadScene("MainMenu");
    }

    // Loads the Hub Level
    public void GoToHubLevel()
    {
        SceneManager.LoadScene("HomeLevel");
    }

    // Loads the First Level
    public void GoToLevelOne()
    {
        SceneManager.LoadScene("Himeys Level");
    }

    // Loads the Second Level
    public void GoToLevelTwo()
    {
        SceneManager.LoadScene("BeltLevel");
    }

    // Loads the Third Level
    public void GoToLevelThree()
    {
        SceneManager.LoadScene("SteamLevel");
    }

    // Loads the Test Level Scene
    public void GoToTestLevel()
    {
        SceneManager.LoadScene("TestLevel");
    }

    // Loads the Tutorial Scene
    public void Tutorial()
    {
        EnableCursor();
        SceneManager.LoadScene("Tutorial");
    }

    // Loads the Options Scene
    public void Options()
    {
        EnableCursor();
        SceneManager.LoadScene("Options");
    }

    // Loads the Win Screen
    public void Win()
    {
        EnableCursor();
        SceneManager.LoadScene("WinScreen");
    }

    // Loads the Game Over Screen
    public void GameOver()
    {
        EnableCursor();
        SceneManager.LoadScene("LoseScreen");
    }

    // Loads the Beta Level
    public void BetaLevel()
    {
        EnableCursor();
        SceneManager.LoadScene("Himeys Level");
    }

    // Restarts the current scene (used for respawning)
    public void RestartCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // Closes out of the Game
    // Only works in built project, not Unity Editor
    public void Quit()
    {
        Application.Quit();
    }

    public void cheatsToggleSpeed()
    {
        cheatsSpeed = !cheatsSpeed;
    }

    public void cheatsToggleJump()
    {
        cheatsJump = !cheatsJump;
    }
}
