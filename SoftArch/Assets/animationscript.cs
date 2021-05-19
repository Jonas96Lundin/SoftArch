using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
 * By Tinea Larsson
 * 
 * Script to update the parameters needed in the animation controller
 *
 */

 //TO DO: Clean up

public class animationscript : MonoBehaviour
{
    public float secondsIdle = 0.0f;
    bool isFacingRight, gravityFlipped;


    public Rigidbody rb;
    Animator animator;

    float vX;
    float vY;

    [Tooltip("Amount of seconds the character has to be idle before playing the animation")]
    [Range(0.0f, 20.0f)]
    [SerializeField]
    private float secondsIdleUntilWave = 10.0f; // Amount of seconds the character has to be idle before playing the animation

  
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // if not moving with normal gravity
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            secondsIdle += Time.deltaTime;
            secondsIdle = secondsIdle % 60;
        }
        else
        {
            secondsIdle = 0.0f;
        }

        // if idle for the selected amount of seconds, play animation
        if (secondsIdle >= secondsIdleUntilWave)
        {
            if (isFacingRight)
            {
                animator.Play("wave_animation_mirrored"); //ÄNDRA
            }
            else
            {
                animator.Play("wave_animation");
            }

            secondsIdle = 0.0f;
        }

        UpdateParameters();
    }

    void UpdateParameters()
    {
        if ((isFacingRight && rb.velocity.x < 0 )|| (!isFacingRight && rb.velocity.x > 0))
        {
            ChangeDirection();
        }

        // Set velocity X to always be positive, direction does not matter for selecting the correct animation
        vX = (float)Math.Round(Mathf.Abs(rb.velocity.x) * 100f / 100f);
        vY = (float)Math.Round(Mathf.Abs(rb.velocity.y) * 100f / 100f);
       
        // if gravity is currently set to normal
        if (rb.useGravity)
        {
            gravityFlipped = false;
            animator.SetBool("GravityFlipped", false);
        }
        else
        {
            gravityFlipped = true;
            //Debug.Log((float)Math.Round(rb.velocity.y * 100f / 100f));
            animator.SetBool("GravityFlipped", true);
        }

        animator.SetFloat("VelocityX", vX);
        animator.SetFloat("VelocityY", vY);
    }


    // Might delete this as rotation now happens in Rotationscript instead
    void FlipAnimation()
    {
        // switches the local scale to an inversion of itself 
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    void ChangeDirection() 
    {
        // switches the bool
        isFacingRight = !isFacingRight;
    }
}
