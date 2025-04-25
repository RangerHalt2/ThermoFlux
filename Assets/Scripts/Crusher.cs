// Purpose: This code manages the crushers
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    [Header("Crusher Settings")]
    [SerializeField] private float moveSpeed = 1f; // How fast the crusher moves
    [SerializeField] private float lerpRate = 0.1f; // Lerp smoothness
    [SerializeField] private float delayTime = 1f;  // The amount of time the crusher should wait between crushes
    [SerializeField] private Transform crusherHead; // The moving part of the crusher. If nothing is set, the whole crusher moves
    [SerializeField] private GameObject crusherTrigger; // The trigger on the crusher used for hit detection
    [SerializeField] private Vector3 targetPoint; // The target point to crush toward. Should be placed where you want the crusher to stop
    [SerializeField] private LayerMask collisionLayers; // Any object on in a collision layer will stop the crusher when it makes contact

    private CrusherState state = CrusherState.waiting; // The current state of the crusher
    public enum CrusherState
    {
        crushing,
        retracting,
        waiting

    }

    private Vector3 startPosition; // The starting position of the crusher
    private bool shouldRetract = false; // Determines if the crusher should begin the retraction process

    // Start is called before the first frame update
    void Start()
    {
        // If no Crusher head is assigned
        if (crusherHead == null)
        {
            // Default to moving the whole crusher
            crusherHead = this.transform;
        }
        // Assign the starting position of the crusher to the crusher's current position
        startPosition = crusherHead.position;

        if (crusherTrigger != null)
        {
            CrusherTrigger triggerComponent = crusherTrigger.GetComponent<CrusherTrigger>();
            if (triggerComponent != null)
            {
                triggerComponent.Initialize(this);
            }
            else
            {
                Debug.LogError("CrusherTrigger GameObject must have a CrusherTrigger component.");
            }
        }

        // Begin crushing process after a short delay
        StartCoroutine(WaitBeforeCrushing());
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            // If the crusher is in the crushing state
            case CrusherState.crushing:
                // Gradually move the crusher downwards
                crusherHead.position = Vector3.Lerp(crusherHead.position, targetPoint, lerpRate * Time.deltaTime * moveSpeed);

                // If the crusher has reached its target, or shouuldRetract is true
                if (Vector3.Distance(crusherHead.position, targetPoint) < 0.05f || shouldRetract)
                {
                    // Change state to retracting
                    state = CrusherState.retracting;
                    shouldRetract = false;
                }
                break;
            // If the crusher is in the retracting state
            case CrusherState.retracting:
                // Gradually move crusher upwards
                crusherHead.position = Vector3.Lerp(crusherHead.position, startPosition, lerpRate * Time.deltaTime * moveSpeed);
                // If crusher has reached the starting position
                if (Vector3.Distance(crusherHead.position, startPosition) < 0.05f)
                {
                    // Change state to waiting
                    state = CrusherState.waiting;
                    // Start crushing process again after a short delay
                    StartCoroutine(WaitBeforeCrushing());
                }
                break;
            // If crusher is in the waiting state
            case CrusherState.waiting:
                // Do nothing, waiting for coroutine
                break;
        }
    }

    private IEnumerator WaitBeforeCrushing()
    {
        // Wait a set delay
        yield return new WaitForSeconds(delayTime);
        // Change state to crushing
        state = CrusherState.crushing;
    }


    public void OnCrusherTriggerEnter(Collider other)
    {
        if (state != CrusherState.crushing) return;

        // If the object is tagged as a "Flammable Object", destroy it
        if (other.CompareTag("Flammable Object"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            other.gameObject.transform.localScale = new Vector3(other.gameObject.transform.localScale.x, 0.2f, other.gameObject.transform.localScale.z);
        }
        // If the object is on a collision layer
        else if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            // Set shouldRetract to true to start retraction
            shouldRetract = true;
        }
    }
}
