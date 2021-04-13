using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Jonas script
/// </summary>
public class FlipGravity : MonoBehaviour
{
    bool flippedGravity = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            flippedGravity = !flippedGravity;
            if (!flippedGravity)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (flippedGravity)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity);

        }
    }
}
