using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLine : MonoBehaviour
{
    private LineRenderer iceRay;

    [SerializeField] private float range = 50;
    [SerializeField] private Transform player;
    [SerializeField] private Transform obj;

    private InputHandler my_inputs;
    private ThirdPersonCam cam;

    private void Start()
    {
        my_inputs = GameObject.FindAnyObjectByType<InputHandler>();
        cam = GameObject.FindAnyObjectByType<ThirdPersonCam>();
        iceRay = GetComponentInChildren<LineRenderer>();
        iceRay.startWidth = 2;
        iceRay.endWidth = 2;
    }

    private void Update()
    {
        if (my_inputs.altFireTriggered)
        {

            iceRay.enabled = true;
            BeamIce();
            Debug.Log("num of positions: " + iceRay.positionCount);
            Debug.Log("Player point is: " + obj.position + " end point is: " + iceRay.GetPosition(1));
        }
        else
        {
            iceRay.enabled = false;
        }
    }


    public void BeamIce()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, cam.transform.forward * range, out hit, Mathf.Infinity))
        {
            iceRay.SetPosition(0, obj.position);
            iceRay.SetPosition(1, hit.point);
            
        }
    }

}
