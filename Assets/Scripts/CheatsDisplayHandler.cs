using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatsDisplayHandler : MonoBehaviour
{

    [SerializeField] private GameObject jumpCheatText;
    [SerializeField] private GameObject speedCheatText;

    private BootstrappedData boot;

    private void Start()
    {
        boot = GameObject.FindAnyObjectByType<BootstrappedData>();

        if (boot.cheatsJump && jumpCheatText)
            jumpCheatText.SetActive(true);
        else
            jumpCheatText.SetActive(false);
        if (boot.cheatsSpeed)
            speedCheatText.SetActive(true);
        else
            speedCheatText.SetActive(false);
    }
}
