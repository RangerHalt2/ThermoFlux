// Purpose: This code will desapwn crates that enter it
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateKiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If object has the Flammable Object
        if(other.CompareTag("Flammable Object"))
        {
            Destroy(other.gameObject);
        }
    }
}
