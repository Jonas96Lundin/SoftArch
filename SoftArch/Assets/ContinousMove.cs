using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * By Tinea Larsson
 */

public class ContinousMove : MonoBehaviour
{
    [Range(-5.0f, 5.0f)]
    public float Movement = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * (Movement * Time.deltaTime), Space.World);
    }
}
