using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatPipe : MonoBehaviour
{
    [Header("Pipe Settings")]
    [SerializeField] private bool isShootingSteam;

    private FreezePipe pipeHole;

    private void Start()
    {
        pipeHole = GetComponentInChildren<FreezePipe>();

        if (isShootingSteam)
            pipeHole.TurnOn();
        else
            pipeHole.TurnOff();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            //Immediately upon turning on the fire, burn away all the ice.
            Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, 30);
            foreach (Collider col in cols)
            {
                if (col.CompareTag("Ice"))
                {
                    Destroy(col.gameObject);
                }
            }

            pipeHole.TurnOn();
        }
    }
}
