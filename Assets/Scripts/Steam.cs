using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : MonoBehaviour
{

    [Header("Steam Settings")]
    [SerializeField] private float speedMulti = 1.2f;
    [SerializeField] private int[] layersToIgnore;
    [SerializeField] private int steamLayer;
    [SerializeField] private ParticleSystem steamParticles;

    [SerializeField] private float yToDespawn;

    private Rigidbody rb;

    private float timer;
    private float cooldown = 0.15f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        for(int i = 0; i < layersToIgnore.Length; i++)
        {
            Physics.IgnoreLayerCollision(layersToIgnore[i], steamLayer);
        }
        if(steamParticles != null)
            steamParticles.Play(); //they should never stop playing.
    }

    private void Update()
    {
        rb.AddForce(gameObject.transform.up *  speedMulti, ForceMode.Force);
        timer -= Time.deltaTime;


        if (gameObject.transform.position.y > yToDespawn)
            Destroy(gameObject);
    }

    //There's some gravity shenanigans that require the object being pushed by steam to have a little boost
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody collRb = collision.collider.gameObject.GetComponent<Rigidbody>();
        if (collRb != null)
        {
            if (timer > 0) return;
            collRb.AddForce(gameObject.transform.up * speedMulti/2, ForceMode.Force);
            timer = cooldown;
        }
    }

}
