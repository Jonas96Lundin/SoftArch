using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysVisible : MonoBehaviour
{
    public GameObject camera;
    public GameObject target;
    public LayerMask myLayerMask;


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, 
            (target.transform.position-camera.transform.position).normalized, out hit, Vector3.Distance(camera.transform.position, target.transform.position), myLayerMask))
        {
            Debug.DrawRay(camera.transform.position,
            (target.transform.position - camera.transform.position), Color.green);
            Debug.Log(hit.collider.gameObject.tag);

            if (hit.collider.gameObject.tag == "Sphere")
            {
                target.transform.localScale = new Vector3(0, 0, 0);
            }
            else
            {
                target.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
        }
    }
}
