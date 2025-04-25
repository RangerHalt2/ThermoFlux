using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearBoot : MonoBehaviour
{

    private BootstrappedData boot;
    const string SceneName = "Bootstrapper";


    // Start is called before the first frame update
    void Start()
    {
        boot = GameObject.FindAnyObjectByType<BootstrappedData>();
        if (boot != null)
        {
            boot.ClearBoot();
        }
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }
}
