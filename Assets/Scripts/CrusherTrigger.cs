// Purpose: This code manages the crusher's trigger
// Author: Ryan Lupoli
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherTrigger : MonoBehaviour
{
    private Crusher crusher;

    public void Initialize(Crusher crusherRef)
    {
        crusher = crusherRef;
    }

    private void OnTriggerEnter(Collider other)
    {
        crusher?.OnCrusherTriggerEnter(other);
    }
}
