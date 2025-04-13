// Purpose: This code handles the spawning of crates
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cratePrefab; // Reference to crate prefab
    [SerializeField] private float spawnTimer; // How often to spawn a new crate
    private float elapsedTime = 0f; // How much time has passed since last crate spawn

    // Update is called once per frame
    void Update()
    {
        // Increment Timer
        elapsedTime += Time.deltaTime;

        // If enough time has passed
        if(elapsedTime >= spawnTimer)
        {
            SpawnCrate();
            // Reset Elapsed Time
            elapsedTime = 0f;
        }
    }
    // Spawns a crate at the object's current position
    private void SpawnCrate()
    {
        if (cratePrefab != null)
        {
            Instantiate(cratePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Crate Prefab is not assigned in the inspector.");
        }
    }
}
