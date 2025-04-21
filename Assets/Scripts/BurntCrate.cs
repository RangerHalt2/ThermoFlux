using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurntCrate : MonoBehaviour
{
    private Color darkColor = Color.black; // Set the color to darken to
    private float darkenAmount = 0.9f; // Control the darkening effect

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if(renderer != null && renderer.material != null)
        {
            Color originalColor = renderer.material.color;

            Color darkenedColor = new Color(originalColor.r * (1 - darkenAmount), originalColor.g * (1 - darkenAmount), originalColor.b * (1 - darkenAmount));

            renderer.material.SetColor("_Color", darkenedColor);
        }
    }
}
