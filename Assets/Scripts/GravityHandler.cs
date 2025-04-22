using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityHandler : MonoBehaviour
{

    private Rigidbody rb;
    private Collider iceCol;

    private Vector3 dir;
    private float dist;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        iceCol = GetComponentInParent<Collider>();
    }

    private void Start()
    {
        iceCol.enabled = false;
        Collider[] cols = Physics.OverlapBox(iceCol.bounds.center, iceCol.bounds.extents, iceCol.transform.rotation);
        foreach (Collider col in cols)
        {
            if (Physics.ComputePenetration(iceCol, iceCol.transform.position, iceCol.transform.rotation, col, col.transform.position, col.transform.rotation, out dir, out dist))
            {
                rb.transform.position += dir * dist;
            }
        }
        Invoke("SetKinematicFalse", 1);
        iceCol.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Steam") || other.CompareTag("Hazard")) return;
        if (other.CompareTag("Ice") || other.CompareTag("Player") || other.CompareTag("Kill Plane")) return;
        if (other.CompareTag("Push"))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            return;
        }

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void OnTriggerExit(Collider other)
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void SetKinematicFalse()
    {
        rb.isKinematic = false;
    }

}
