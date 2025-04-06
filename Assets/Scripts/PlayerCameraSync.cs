//Author: Logan Baysinger
//Purpose: 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraSync : MonoBehaviour
{
    [SerializeField] private Transform playerBob;
    [SerializeField] private Transform obj;
    [SerializeField] private Transform orientation;

    private InputHandler myInputs;

    private void Start()
    {
        myInputs = GameObject.FindAnyObjectByType<InputHandler>();
    }

    private void Update()
    {
        if(myInputs.altFireTriggered || myInputs.fireTriggered)
        {
            playerBob.forward = orientation.forward;
            obj.forward = orientation.forward;
        }
    }
}
