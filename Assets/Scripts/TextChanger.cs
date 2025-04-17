using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextChanger : MonoBehaviour
{
    private TextMeshPro TextMeshPro;
    private BootstrappedData bootStrap;

    private enum LevelChoice
    {
        WindLevel,
        BeltLevel,
        SteamLevel
    };

    [Header("Attached Game Level")]
    [SerializeField] private LevelChoice levelChoice;

    private bool levelIsDone;

    private void Start()
    {
        TextMeshPro = GetComponent<TextMeshPro>();
        bootStrap = GameObject.FindAnyObjectByType<BootstrappedData>();

        Debug.Log(levelChoice.ToString());

        switch (levelChoice.ToString())
        {
            case "WindLevel":
                if (bootStrap.LevelOneComplete) levelIsDone = true;
                break;
            case "BeltLevel":
                if (bootStrap.LevelTwoComplete) levelIsDone = true;
                break;
            case "SteamLevel":
                if (bootStrap.LevelThreeComplete) levelIsDone = true;
                break;
        }

        if (levelIsDone)
        {
            TextMeshPro.fontStyle = FontStyles.Strikethrough;
        }
        else
        {
            TextMeshPro.fontStyle = FontStyles.Normal;
        }

    }

}
