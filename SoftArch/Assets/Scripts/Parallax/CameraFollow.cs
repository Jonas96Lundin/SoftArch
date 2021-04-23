using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private CharController player;

    Vector3 cameraoffset;
    // Start is called before the first frame update
    void Start()
    {
        cameraoffset = new Vector3(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y, transform.position.z - player.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 deltaMovement = lastCameraPos - cameraTransform.position;
        //transform.position += new Vector3(deltaMovement.x * parallaxEffect.x, deltaMovement.y * parallaxEffect.y, 0);
        //lastCameraPos = cameraTransform.position;

        //transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 13, player.transform.position.z - 30);

        transform.position = player.transform.position + cameraoffset;
    }
}
