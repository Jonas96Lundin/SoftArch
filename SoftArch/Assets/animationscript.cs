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
    bool isFacingRight, gravityFlipped, moving;

    public enum charType { player, ai };
    public charType thisCharType;

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
        switch (thisCharType)
        {
            case charType.player:
                if (rb.velocity.x != 0 || rb.velocity.y != 0)
                {
                    moving = true;
                }
                break;

            case charType.ai:
                //
                moving = false; // 
                break;
        }

        UpdateParameters();

        // if not moving (with normal gravity)
        if (!moving)
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
            switch (thisCharType)
            {
                case charType.player:
                    if (isFacingRight)
                    {
                        animator.Play("wave_animation_mirrored");
                    }
                    else
                    {
                        animator.Play("wave_animation");
                    }
                    secondsIdle = 0.0f;
                    break;

                case charType.ai:

                    //Play sad 
                    animator.Play("ai_turning_to_sad");

                    //Play happy
                    //animator.Play("ai_turning_to_happy");
                    secondsIdle = 0.0f;
                    break;
            }
        }
    }

    void UpdateParameters()
    {
        switch (thisCharType)
        {
            case charType.player:

                if ((isFacingRight && rb.velocity.x < 0) || (!isFacingRight && rb.velocity.x > 0))
                {
                    ChangeDirectionPlayer();
                }
                vX = (float)Math.Round(Mathf.Abs(rb.velocity.x) * 100f / 100f);
                vY = (float)Math.Round(rb.velocity.y * 100f / 100f);

                break;

            case charType.ai:
                // if needed change direction

                // vX = 
                // vY = 


                break;
        }


        //// if gravity is currently set to normal
        //if (rb.useGravity)
        //{
        //    gravityFlipped = false;
        //    animator.SetBool("GravityFlipped", false);
        //}
        //else
        //{
        //    gravityFlipped = true;
        //    //Debug.Log((float)Math.Round(rb.velocity.y * 100f / 100f));
        //    animator.SetBool("GravityFlipped", true);
        //}

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

    // switches the bool
    void ChangeDirectionPlayer(){ isFacingRight = !isFacingRight;}

}
