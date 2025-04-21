using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    
    public BootstrappedData bootstrappedData;
    private Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        bootstrappedData = GameObject.FindAnyObjectByType<BootstrappedData>();
        
        toggle = GetComponent<Toggle>();
        if (bootstrappedData != null)
        {
            toggle.isOn = bootstrappedData.cheatsJump;
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    void OnToggleChanged(bool value)
    {
        bootstrappedData.cheatsJump = value;
    }
}
