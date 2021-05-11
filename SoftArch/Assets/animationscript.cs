using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationscript : MonoBehaviour
{
    float vX;
    float vY;
    public float idleTimer;
    public Rigidbody rb;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        vX = rb.velocity.x;
        vY = rb.velocity.y;
        animator.SetFloat("VelocityX", vX);
        animator.SetFloat("VelocityX", vY);

    }
}
