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
// Use isOnGround to play run-animation if on slope, currently playing jump-animation

// Fix ai idle animations to support both happy and sad

//Clean up

public class animationscript : MonoBehaviour
{
    float secondsIdle = 0.0f; 
    bool gravityFlipped, moving;

    public enum charType { player, ai };

    [Tooltip("Type of character")]
    [SerializeField]
    private charType thisCharType;

    Animator animator;

    float vX;
    float vY;

    [Tooltip("Rigidbody")]
    [SerializeField]
    private Rigidbody rb;

    [Tooltip("ai Agent")]
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent aiAgent;
    

    [Tooltip("Amount of seconds the character has to be idle before playing the animation")]
    [Range(0.0f, 20.0f)]
    [SerializeField]
    private float secondsIdleUntilWave = 10.0f; 


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        moving = CheckMovement();

        UpdateParameters();

        // if not moving
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
                    if (animator.GetBool("isFacingRight"))
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
                    //animator.Play("ai_turning_to_sad");

                    animator.Play("ai_turning_to_happy");
                    
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

                vX = (float)Math.Round(rb.velocity.x * 100f / 100f);
                vY = (float)Math.Round(rb.velocity.y * 100f / 100f);

                if ((FacingRight() && vX < 0) || (!FacingRight() && vX > 0))
                {
                    ChangeDirection();
                }
                break;

            case charType.ai:

                vX = (float)Math.Round(aiAgent.velocity.x * 100f / 100f);
                vY = (float)Math.Round(aiAgent.velocity.y * 100f / 100f);

                // check where it's facing
                if ((FacingRight() && vX < 0) || (!FacingRight() && vX > 0))                    
                {
                    ChangeDirection();
                }
                break;
        }

        animator.SetFloat("VelocityX", vX);
        animator.SetFloat("VelocityY", vY);
    }

    bool CheckMovement()
    {
        switch (thisCharType)
        {
            case charType.player:
                if (rb.velocity.x != 0 && rb.velocity.y != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case charType.ai:
                if (aiAgent.velocity.x != 0 && aiAgent.velocity.y != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        Debug.Log("No charType Selected"); // DEBUG
        return false;                       
    }

    // switches the bool
    void ChangeDirection()
    {
        bool thisBool = animator.GetBool("isFacingRight");
        animator.SetBool("isFacingRight", !thisBool);
    }

    // fetches bool form animator controller
    bool FacingRight() { return animator.GetBool("isFacingRight"); }

}
