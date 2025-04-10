//Purpose: This code handles the ice placement by the player.
//Author: Logan Baysinger.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class IceSpell : MonoBehaviour
{

    //LB: Variables and settings for their respective roles.
    [Header("Shooting Settings")]
    [SerializeField] private float range = 50;
    [SerializeField] private float ShotCooldown = 0.2f;
    [SerializeField] private float growthMulti = 1.1f;

    [Header("Ice to Place")]
    [SerializeField] private GameObject ice;
    [SerializeField] private ParticleSystem iceParticles;

    [Header("Misc Assignments")]
    [SerializeField] private Transform player; //LB: Not currently used, but probably will be used to shoot the laser from the player
    [SerializeField] private string[] ignoreTags;

    //Varibles that tech's will not see.
    private InputHandler my_inputs;
    private ThirdPersonCam cam; //This actually gets the entire game object but mostly cares about the "ThirdPersonCam" feature.

    private EnemyHealth my_enemy;
    private FlammableObject my_flammableObj;

    private float timer;

    private SpellSounds sound;
    private float audioTimer;
    private float audioCoodlown = 2;

    private float growTimer;
    private float growCooldown = 0.5f;

    private Animator pcAnim;

    private GameObject obj;

    private bool canPlaceIce;

    //Gets the inputs and the camera
    private void Start()
    {
        my_inputs = GameObject.FindAnyObjectByType<InputHandler>();
        cam = GameObject.FindAnyObjectByType<ThirdPersonCam>();
        sound = GameObject.FindAnyObjectByType<SpellSounds>();
        pcAnim = GetComponentInChildren<Animator>();
    }

    //If the trigger is pressed it calls the place ice fire.
    void Update()
    {
        timer -= Time.deltaTime;
        growTimer -= Time.deltaTime;
        if (my_inputs.altFireTriggered && timer <= 0)
        {
            if(canPlaceIce)
                ShootIce();
            if(audioTimer <= 0)
            {
                if (sound != null)
                {
                    sound.IceSound();
                    audioTimer = audioCoodlown;
                }
            }
        }

        if(my_inputs.altFireTriggered && !canPlaceIce)
        {
            if(growTimer <= 0)
                ScaleIce();
        }

        audioTimer -= Time.deltaTime;

        if (my_inputs.altFireTriggered)
        {
            if (!iceParticles.isPlaying)
            {
                iceParticles.Play();
                Debug.Log("Start Playing Ice Particles");
            }
        }
        else
        {
            canPlaceIce = true;
        }

        //tells the animator if the trigger is pressed.
        if(my_inputs.altFireTriggered || my_inputs.fireTriggered)
        {
            pcAnim.SetBool("isCasting", true);
        }
        else
        {
            pcAnim.SetBool("isCasting", false);
            if (iceParticles.isPlaying)
            {
                iceParticles.Stop();
                Debug.Log("Stop Playing Ice Particles");
            }
        }
    }

    //Increases the Scale of the ice object most recently placed while the button is held down.
    void ScaleIce()
    {
        if (obj == null) return;
        Transform currTrans = obj.transform;
        currTrans.localScale = new Vector3(currTrans.localScale.x * growthMulti, currTrans.localScale.y * growthMulti, currTrans.localScale.z * growthMulti);
        growTimer = growCooldown;
    }


    /*Author: Logan Baysinger.
     * Function: This function fires a raycast and checks for collisions. It checks the array ignoreTags which is defined in the editor in Unity.
     * If the ray hits something with a tag from the ignoreTags it returns and cancels the function. This stops it from hitting objects behind it.
     * It checks with an overlap sphere if the object is too close to an unplaceable surface to prevent it from clipping.
     * The ray shoots from the camera and *can* hit the player in the back of the head. This will need to be changed at some point when a firing indicator is added.
     */
    void ShootIce()
    {
        RaycastHit hit;
        
        Debug.Log("Ice Fire check #1");
        //if the Raycast hits something and that something is not in the ignoreTags array
        if (Physics.Raycast(cam.transform.position, cam.transform.forward * range, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.yellow, 50f);
            //If the raycast hit a "Hazard" check if it's one marked with health and destructible and then do damage if so
            if (hit.collider.gameObject.CompareTag("Hazard"))
            {
                my_enemy = hit.collider.gameObject.GetComponent<EnemyHealth>();
                if(my_enemy != null && !my_enemy.immuneToIce)
                {
                    my_enemy.TakeDamage(my_enemy.iceDamage);
                    return;
                }
            }

            //If the flammable object is hit by ice, extinguish the fire.
            if(hit.collider.gameObject.CompareTag("Flammable Object"))
            {
                my_flammableObj = hit.collider.gameObject.GetComponent<FlammableObject>();
                if(my_flammableObj != null)
                {
                    my_flammableObj.IceHit();
                }
                return;
            }

            if (hit.transform.gameObject.CompareTag("Steam"))
            {
                FreezePipe steam = hit.transform.gameObject.GetComponent<FreezePipe>();

                if (steam != null)
                {
                    steam.TurnOff();
                }
            }

            //Default behavrior for placing down the block.
            if (hit.transform.gameObject.tag != "Player")
            {
                if (Array.IndexOf(ignoreTags, hit.collider.gameObject.tag) != -1) return;
                Debug.Log("Ice Fire did hit");
                obj = Instantiate(ice, hit.point, ice.transform.rotation);
                Collider[] cols = Physics.OverlapSphere(obj.transform.position, 1f);
                for (int i = 0; i < cols.Length; i++)
                {
                    if (Array.IndexOf(ignoreTags, cols[i].tag) != -1)
                    {
                        //Destroy(obj);
                        return;
                    }
                }
                timer = ShotCooldown;
                canPlaceIce = false;
                //Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.yellow, 10f);
            }
        }
        else
        { //Debug code, this does not display in game.
            //Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.yellow, 10f);
            Debug.Log("Ice Fire did not hit");
        }
    }
}
