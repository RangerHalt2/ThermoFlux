using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanPush : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float pushForce; // The amount of force applied to an object which enters a push trigger
    [SerializeField] private Vector3 pushDirection; // Determines the direction force will be applied. Put a positive or negative 1 for the respecrive axis you would like to push along

    [SerializeField] private string[] tagsToPush;

    [HideInInspector] public bool isOn;

    [SerializeField] private Transform raycastPoint;

    private LayerMask lmask;
    private void Start()
    {
        isOn = true;
        lmask = LayerMask.GetMask("Ignore Raycast");
    }

    // While an object is in the trigger
    private void OnTriggerStay(Collider other)
    {
        if (!isOn) return;

        // If the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Attempt to get the Rigidbody component of the player
            Rigidbody playerRb = other.GetComponentInParent<Rigidbody>();

            raycastPoint.transform.position = new Vector3(playerRb.position.x, raycastPoint.position.y, raycastPoint.position.z);

            // If the player has a rigidbody
            if (playerRb != null && canSeePlayer(playerRb))
            {
                // Apply constant force to the object in the pushDirection
                PlayerController playerController = playerRb.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.pushMovement = pushDirection.normalized * pushForce;

                }
                Debug.Log("Added force");
            }
            return;
        }

        //Check for each tag in the array if the object matches
        //This code is essentially the same as the player, but it's seperated from the player push triggers
        for (int i = 0; i < tagsToPush.Length; i++)
        {
            if (other.CompareTag(tagsToPush[i]))
            {
                //If it matches, it's an object that should be pushed
                Rigidbody objRb = other.GetComponent<Rigidbody>();

                if (objRb != null)
                {
                    objRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Force);
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
                    PlayerController playerController = objRb.GetComponent<PlayerController>();
                    if(playerController != null)
                    {
                        playerController.isDecay = true;
                    }
                    objRb.linearVelocity = Vector3.zero;
                    objRb.angularVelocity = Vector3.zero;
                }
            }
        }
    }

    private bool canSeePlayer(Rigidbody playerRB)
    {
        if (playerRB == null) return false;
        Vector3 direction = -(raycastPoint.transform.position - playerRB.transform.position).normalized;
        RaycastHit hit;
        Debug.DrawRay(raycastPoint.transform.position, direction*300, Color.yellow, 50f);
        if (Physics.Raycast(raycastPoint.transform.position, direction*300, out hit, Mathf.Infinity, ~lmask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.CompareTag("Player")) return true;
            else return false;
        }
        else
        {
            return false;
        }
    }

}
