using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private CharController follow;

    Vector3 cameraoffset;

    void Start()
    {
        cameraoffset = new Vector3(transform.position.x - follow.transform.position.x, transform.position.y - follow.transform.position.y, transform.position.z - follow.transform.position.z);
    }

    void Update()
    {
        transform.position = follow.transform.position + cameraoffset;
    }
}
