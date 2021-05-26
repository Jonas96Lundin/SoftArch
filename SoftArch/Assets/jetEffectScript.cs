using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class jetEffectScript : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject thisGameObj;

    Quaternion startRotation;

    // Start is called before the first frame update
    private void Start()
    {
        startRotation = thisGameObj.transform.rotation;

        Debug.Log(startRotation.x);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.x == 0)
        {
            thisGameObj.transform.rotation = startRotation;
        }
        else
        {
            Quaternion newRotation = startRotation;
            newRotation.x = (thisGameObj.transform.rotation.x + 0.01f * rb.velocity.x);
            //newRotation.y = startRotation.y;
            //newRotation.z = startRotation.z;
            thisGameObj.transform.rotation = newRotation;

            Debug.Log(newRotation.x);
        }
    }
}
