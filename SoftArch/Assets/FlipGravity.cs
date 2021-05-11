using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Jonas script
/// </summary>
public class FlipGravity : MonoBehaviour
{
    [SerializeField] bool flippedGravity = false;

    public bool GetSetFlippedGravity { get => flippedGravity; set => flippedGravity = value; }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!flippedGravity)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
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
