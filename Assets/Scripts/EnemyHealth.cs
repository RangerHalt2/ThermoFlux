//Purpose: To manage the enemies health and death behavior.
//Author: Logan Baysinger
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //Variables
    [Header("Enemy Health Settings")]
    [SerializeField] private float health;
    [SerializeField] private bool immuneToFire;
    [SerializeField] public bool immuneToIce;
    
    [Header("Damage Numbers")]
    [SerializeField] private float fireDamage;
    [SerializeField] public float iceDamage;

    private float burntimer;
    private float damageBuffer;

    [Header("Damage Immunity Cooldown")]
    [SerializeField] private float burnTickCD = 2;
    [SerializeField] private float damageTimer = 1;

    private FlammableObject my_obj;

    public void Start()
    {
        my_obj = GetComponent<FlammableObject>();
    }

    //This will trigger if it's taking damage from active gouge of flame but only as often as permitted by the damageBuffer timer.
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire") && !immuneToFire)
        {
            if(damageBuffer <= 0)
            {
                TakeDamage(fireDamage);
                damageBuffer = damageTimer;
            }
        }
    }

    //Currently, checks if the object is one fire and a flammable object for DoT damage.
    //Updates the BurnTimer and the DamageBuffer by time.deltatime to make sure enemies don't take damage per game tick.
    public void Update()
    {
        if (my_obj != null && my_obj.isOnFire && !immuneToFire)
        {
            if (burntimer <= 0)
            {
                TakeDamage(fireDamage/2);
                burntimer = burnTickCD;
            }
        }
        burntimer -= Time.deltaTime;
        damageBuffer -= Time.deltaTime;
    }

    //Very simple function, takes in the damage from the paramater and updates health. Calls Die() if it is below 0 after taking the damage.
    //Notice: Ice damage is called in the IceSpell when the raycast hits the hazard.
    public void TakeDamage(float damage)
    {
        if (damageBuffer > 0) return; //Don't perform the damage if the buffer isn't set
        health -= damage;
        damageBuffer = damageTimer;
        Debug.Log("health is: " + health);
        if (health <= 0) Die();
    }

    //Seperate from the Take Damage function so we can add more or less interaction easily.
    public void Die()
    {
        Destroy(gameObject);
    }
}
