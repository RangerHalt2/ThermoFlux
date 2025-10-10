using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTrigger : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float pushForce; // The amount of force applied to an object which enters a push trigger
    [SerializeField] private Vector3 pushDirection; // Determines the direction force will be applied. Put a positive or negative 1 for the respecrive axis you would like to push along
    [SerializeField] private float maxVelocity = 7;
    public float playerPush = 10.5f;

    [SerializeField] private string[] tagsToPush;


    [HideInInspector] public bool isOn;

    public float cooldown;
    private float timer;

    private void Start()
    {
        isOn = true;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    // While an object is in the trigger
    private void OnCollisionStay(Collision other)
    {
        if (!isOn) return;

        // If the object entering the trigger is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Attempt to get the Rigidbody component of the player
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();

            // If the player has a rigidbody
            if (playerRb != null)
            {
                // Apply constant force to the object in the pushDirection
                playerRb.AddForce(pushDirection.normalized * pushForce*playerPush, ForceMode.Force);
                timer = cooldown;
                Debug.Log("Added force");
            }
            return;
        }

        //Check for each tag in the array if the object matches
        //This code is essentially the same as the player, but it's seperated from the player push triggers
        for (int i = 0; i < tagsToPush.Length; i++)
        {
            if (other.gameObject.CompareTag(tagsToPush[i]))
            {
                //If it matches, it's an object that should be pushed
                Rigidbody objRb = other.gameObject.GetComponentInParent<Rigidbody>();

                if (objRb != null)
                {
                    objRb.linearVelocity = new Vector3(pushDirection.x * pushForce, pushDirection.y * pushForce, pushDirection.z * pushForce);
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < tagsToPush.Length; i++)
        {
            if (other.CompareTag(tagsToPush[i]))
            {
                //If it matches, it's an object that should be pushed
                Rigidbody objRb = other.GetComponent<Rigidbody>();

                if (objRb != null)
                {
                    objRb.linearVelocity = Vector3.zero;
                    objRb.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}
