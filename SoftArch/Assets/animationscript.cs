using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
 * By Tinea Larsson
 * 
 * Script to update the parameters needed in the animation controller
 */

// TO DO: 

// Change FlipAnimation() to only occur if character has landed from it's previous jump

// Add idleTimer function

public class animationscript : MonoBehaviour
{
    float vX;
    float vY;
    public float idleTimer;
    public Rigidbody rb;

    bool isFacingRight;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateParameters();
    }

    private void FixedUpdate()
    {
        UpdateParameters();
    }

    void UpdateParameters()
    {
        // rotate animation to match its direction, only when switching direction
        if ((isFacingRight && rb.velocity.x < 0 )||(!isFacingRight && rb.velocity.x > 0))
        {
            FlipAnimation();
        }

        // Set velocity X to always be positive, direction does not matter for selecting the correct animation
        vX = (float)Math.Round(Mathf.Abs(rb.velocity.x) * 100f / 100f);

        //Debug.Log(vX);

        // if gravity is currently set to normal
        if (rb.useGravity)
        {
            vY = (float)Math.Round(rb.velocity.y * 100f / 100f);
            animator.SetBool("GravityFlipped", false);
        }
        else
        {
            vY = rb.velocity.y; //
            animator.SetBool("GravityFlipped", true);
        }


        animator.SetFloat("VelocityX", vX);
        animator.SetFloat("VelocityY", vY);

        

        //Debug.Log(animator.GetFloat("VelocityX"));
    }

    void FlipAnimation()
    {
        // switches the bool
        isFacingRight = !isFacingRight;

        // switches the local scale to an inversion of itself
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
