// Purpose: This code allows for the changing of scenes
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public bool cheatsJump;
    public bool cheatsSpeed;

    private BootstrappedData boot;

    private AudioManager am;
    private SpellSounds ss;

    private float timer;
    private float cooldown = 2;

    private void Start()
    {
        timer = cooldown;
        boot = GameObject.FindAnyObjectByType<BootstrappedData>();
        cheatsJump = boot.cheatsJump;
        cheatsSpeed = boot.cheatsSpeed;

        am = GameObject.FindObjectOfType<AudioManager>();
        ss = GameObject.FindObjectOfType<SpellSounds>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    // Unlocks and renables the cursor for the purpose of menu navigation
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Loads the Main Menu
    public void MainMenu ()
    {
        am.StopBGM();
        ss.Stop();
        EnableCursor();
        SceneManager.LoadScene("MainMenu");
    }

    // Loads the Hub Level
    public void GoToHubLevel()
    {
        am.StopBGM();
        ss.Stop();
        SceneManager.LoadScene("HomeLevel");
    }

    // Loads the First Level
    public void GoToLevelOne()
    {
        am.StopBGM();
        ss.Stop();
        SceneManager.LoadScene("WindLevel");
    }

    // Loads the Second Level
    public void GoToLevelTwo()
    {
        am.StopBGM();
        ss.Stop();
        SceneManager.LoadScene("BeltLevel");
    }

    // Loads the Third Level
    public void GoToLevelThree()
    {
        am.StopBGM();
        ss.Stop();
        SceneManager.LoadScene("SteamLevel");
    }

    // Loads the Test Level Scene
    public void GoToTestLevel()
    {
        am.StopBGM();
        ss.Stop();
        SceneManager.LoadScene("TestLevel");
    }

    // Loads the Tutorial Scene
    public void Tutorial()
    {
        am.StopBGM();
        ss.Stop();
        EnableCursor();
        SceneManager.LoadScene("Tutorial");
    }

    // Loads the Options Scene
    public void Options()
    {
        am.StopBGM();
        ss.Stop();
        EnableCursor();
        SceneManager.LoadScene("Options");
    }

    // Loads the Win Screen
    public void Win()
    {
        am.StopBGM();
        ss.Stop();
        EnableCursor();
        SceneManager.LoadScene("WinScreen");
    }

    // Loads the Game Over Screen
    public void GameOver()
    {
        am.StopBGM();
        ss.Stop();
        EnableCursor();
        SceneManager.LoadScene("LoseScreen");
    }

    // Loads the Beta Level
    public void BetaLevel()
    {
        am.StopBGM();
        ss.Stop();
        EnableCursor();
        SceneManager.LoadScene("Himeys Level");
    }

    // Restarts the current scene (used for respawning)
    public void RestartCurrentScene()
    {
        am.StopBGM();
        ss.Stop();
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
        if (timer > 0) return;
        Debug.Log("Toggling Cheats Speed");
        cheatsSpeed = !cheatsSpeed; //Changes on to off, off to on
        boot.cheatsSpeed = cheatsSpeed;
    }

    public void cheatsToggleJump()
    {
        if(timer > 0) return;
        Debug.Log("Toggling Cheats Jump");
        cheatsJump = !cheatsJump;
        boot.cheatsJump = cheatsJump;
    }
}
