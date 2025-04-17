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

        toggle.isOn = bootstrappedData.cheatsJump;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
