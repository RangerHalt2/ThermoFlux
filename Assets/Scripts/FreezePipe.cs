using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePipe : MonoBehaviour
{
    private PushTrigger pushTrigger;

    private ParticleSystem particles;

    private void Start()
    {
        pushTrigger = GetComponentInChildren<PushTrigger>();
        particles = GetComponentInChildren<ParticleSystem>();
    }


    //These functions are public and are called in other scripts respectively.
    //Turn off is called in the IceSpell.cs
    public void TurnOff()
    {
        pushTrigger.isOn = false;
        particles.Stop();
    }
    //Turn on is called in the melting objects script & HeatPipe.cs scripts.
    public void TurnOn()
    {
        pushTrigger.isOn = true;
        particles.Play();
    }

}
