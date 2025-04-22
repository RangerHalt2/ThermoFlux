using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObject2 : MonoBehaviour
{

    public float spinSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * spinSpeed * Time.deltaTime);
    }
}
