//Purpose: This code handles the ice placement by the player.
//Author: Logan Baysinger.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class IceSpell : MonoBehaviour
{

    //LB: Variables and settings for their respective roles.
    [Header("Shooting Settings")]
    [SerializeField] private float range = 50;
    [SerializeField] private float ShotCooldown = 0.2f;

    [Header("Ice to Place")]
    [SerializeField] private GameObject ice;

    [Header("Misc Assignments")]
    [SerializeField] private Transform player; //LB: Not currently used, but probably will be used to shoot the laser from the player
    [SerializeField] private string[] ignoreTags;

    //Varibles that tech's will not see.
    private InputHandler my_inputs;
    private ThirdPersonCam cam; //This actually gets the entire game object but mostly cares about the "ThirdPersonCam" feature.

    

    private float timer;

    //Gets the inputs and the camera
    private void Start()
    {
        my_inputs = GameObject.FindAnyObjectByType<InputHandler>();
        cam = GameObject.FindAnyObjectByType<ThirdPersonCam>();
    }

    //If the trigger is pressed it calls the place ice fire.
    void Update()
    {
        timer -= Time.deltaTime;
        if (my_inputs.altFireTriggered && timer <= 0)
        {
            PlaceIce();
        }

    }

    /*Author: Logan Baysinger.
     * Function: This function fires a raycast and checks for collisions. It checks the array ignoreTags which is defined in the editor in Unity.
     * If the ray hits something with a tag from the ignoreTags it returns and cancels the function. This stops it from hitting objects behind it.
     * It checks with an overlap sphere if the object is too close to an unplaceable surface to prevent it from clipping.
     * The ray shoots from the camera and *can* hit the player in the back of the head. This will need to be changed at some point when a firing indicator is added.
     */
    void PlaceIce()
    {
        RaycastHit hit;
        Debug.Log("Ice Fire check #1");
        //if the Raycast hits something and that something is not in the ignoreTags array
        if (Physics.Raycast(cam.transform.position, cam.transform.forward * range, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.yellow, 50f);

            if (hit.transform.gameObject.tag != "Player")
            {
                if (Array.IndexOf(ignoreTags, hit.collider.gameObject.tag) != -1) return;
                Debug.Log("Ice Fire did hit");
                GameObject obj = Instantiate(ice, hit.point, ice.transform.rotation);
                Collider[] cols = Physics.OverlapSphere(obj.transform.position, 1f);
                for (int i = 0; i < cols.Length; i++)
                {
                    if (Array.IndexOf(ignoreTags, cols[i].tag) != -1)
                    {
                        Destroy(obj);
                        return;
                    }
                }
                timer = ShotCooldown;
            }
        }
        else
        { //Debug code, this does not display in game.
            Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.yellow, 10f);
            Debug.Log("Ice Fire did not hit");
        }
    }
}
