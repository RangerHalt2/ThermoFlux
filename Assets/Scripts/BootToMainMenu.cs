using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootToMainMenu : MonoBehaviour
{
    private float timer;
[SerializeField] private float cooldown = 3f;

// Start is called before the first frame update
void Start()
{
    timer = cooldown;
}

private void Update()
{
    if(timer <= 0)
    {
        SceneManager.LoadScene("MainMenu");
    }
    timer -= Time.deltaTime;
}
}
