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


public class animationscript : MonoBehaviour
{
    public float secondsIdle = 0.0f;
    bool isFacingRight;

    public Rigidbody rb;
    Animator animator;

    float vX;
    float vY;

    [Tooltip("Amount of seconds the character has to be idle before playing the animation")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    const float secondsIdleUntilWave = 6.0f; // Amount of seconds the character has to be idle before playing the animation

  
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // if not idle
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
            animator.Play("wave_animation");
            secondsIdle = 0.0f;
        }

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
