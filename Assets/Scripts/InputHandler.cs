using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //LB: This header is the name of the action map and requires an in-editor assignment for the input actions.
    [Header("Inputs Assignment")]
    [SerializeField] private InputActionAsset playerControls;
    [SerializeField] private string actionMapName = "Gameplay";

    //LB: Tech's can change the names of these in the editor, otherwise these are the default action names.
    [Header("Input Action Names")]
    [SerializeField] private string fire = "Fire";
    [SerializeField] private string altFire = "AltFire";

    //These are the input actions and will be set to subscribed later.
    private InputAction fireAction;
    private InputAction altFireAction;

    //These are the public values of whether or not the value is triggered.
    public bool fireTriggered { get;  set; }
    public bool altFireTriggered { get;  set; }

    public static InputHandler Instance { get; private set; } //Instance handler, this just makes sure there's only ever 1 of them. Might be extraneous.

    public void Awake()
    {
        //LB: manage additional instances
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //LB: Destroy(gameObject); //This removes the extra instance, but I currently do not quite understand the full logic of this piece. Ignore for now.
        }

        //LB: Assign and find the actions to the variables.
        fireAction = playerControls.FindActionMap(actionMapName).FindAction(fire);
        altFireAction = playerControls.FindActionMap(actionMapName).FindAction(altFire);
        RegisterInputActions();
    }

    //LB: Register the correct values for the inputs and subscribe them to their respect performed and canceled events.
    void RegisterInputActions()
    {
        //LB: When the fire action is performed, the value will be set to true. Further, it's set to false when it's cancelled.
        fireAction.performed += context => fireTriggered = true;
        fireAction.canceled += context => fireTriggered = false;

        //LB: When the alt fire action is performed, the value will be set to true. Further, it's set to false when it's cancelled.
        altFireAction.performed += context => altFireTriggered = true;
        altFireAction.canceled += context => altFireTriggered = false;
    }

    //Set the on enable and on disable for the action map to set them on and off when the game calls for it.
    private void OnEnable()
    {
        fireAction.Enable();
        altFireAction.Enable();
    }

    private void OnDisable()
    {
        fireAction.Disable();
        altFireAction.Disable();
    }

}
