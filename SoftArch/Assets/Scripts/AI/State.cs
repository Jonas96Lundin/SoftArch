using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson & Helge Herrström
/// </summary>
public abstract class State
{ 
    //Johan
    protected Context _context;
    protected GameObject ai;

    protected CharController master;
    protected Vector3 masterPosition;
    protected float masterSpeed;
    protected float masterVelocityHorizontal;
    protected float lastHorizontalposPos;

    protected float speed, idleSpeed = 3.0f, catchUpSpeed = 8.0f, followSpeed = 5.0f;
    protected float attentionSpan;
    protected float timeToChange;

    //protected bool moveRight, moveLeft/*, jump*/;
    protected bool followMaster;


    //protected Vector3 moveDirection;

    //protected static int wallMask = 1 << 8;
    //protected static int playerMask = 1 << 9;
    //protected static int turnPointMask = 1 << 10;
    //protected static int comboMask = wallMask | playerMask;

    //protected float layerMaskHitDistance;
    //protected RaycastHit hit;

    //protected static Vector3 lastSeenPos;
    //protected static Vector3 nextPos;

    //protected static string nextTurn;



    //Helge
    protected /*const*/ float velocityHorizontal/* = 5.0f*/;//, The character will not add force on movement if the absolute velocity would be higher than this variable
    protected const float forceJump = 500.0f,
                          groundDetectRayLength = 1.1f,
                          breakMaxVelocity = 1f;
    protected const ForceMode horizontalForceMode = ForceMode.VelocityChange;
    protected bool jumpOnFixedUpdate = false,
                    brakeIsAllowed = true; // The character will not brake if this is false and glide further. Is usefull when applying forces on character and not have the force abruptly stop.
    protected bool enableBreakWhenVelocityIsZero = false;
    protected bool moveLeftOnFixedUpdate = false,
         moveRightOnFixedUpdate = false;

    Vector3 addedForces = Vector3.zero;

    //Johan
    public void SetContext(Context context)
    {
        _context = context;
    }

    //Helge
    public bool GetSetBrakeIsAllowed { get => brakeIsAllowed; set => brakeIsAllowed = value; }
    /// <summary>
    /// The character will not brake if this is called, and will glide further. Is usefull when applying forces on character and not have the force abruptly stop. Friction/movement and other forces will still stop the player.
    /// </summary>
    /// <param name="enableBreakWhenVelocityIsZero">Break is automatically turned on when the player has stopped</param>
    public void DoNotBrake(bool enableBreakWhenVelocityIsZero)
    {
        brakeIsAllowed = false;
        this.enableBreakWhenVelocityIsZero = enableBreakWhenVelocityIsZero;
    }
    /// <summary>
    /// Manually allow the character to brake.
    /// </summary>
    public void EnableBrake() => brakeIsAllowed = true;

    //Johan
    public abstract void UpdateState();
    //Helge
    public void FixedUpdateState()
    {
        Rigidbody rigidbody = ai.GetComponent<Rigidbody>();

        // Movement
        if (moveRightOnFixedUpdate)
        {
            MoveRight(ref rigidbody);
        }
        else if (moveLeftOnFixedUpdate)
        {
            MoveLeft(ref rigidbody);
        }
        else // If not moving
        {
            Brake(ref rigidbody);
            //if (brakeIsAllowed) // If allowed to brake
            //{
            //	Brake(ref rigidbody);
            //}
        }
        // Jump
        if (jumpOnFixedUpdate)
        {
            Jump(ref rigidbody);
            jumpOnFixedUpdate = false;
        }

        // Extra forces
        AddExtraForces(ref rigidbody);
        addedForces = Vector3.zero; // Clear added forces
    }
    //Johan
    public abstract void ChangeDirection();
    protected void TurnForward()
    {
        //Debug.Log("Turn Forward");
        ai.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        moveRightOnFixedUpdate = false;
        moveLeftOnFixedUpdate = false;
        velocityHorizontal = 0f;
    }
    protected void TurnBack()
    {
        //Debug.Log("Turn Back");
        ai.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        moveRightOnFixedUpdate = false;
        moveLeftOnFixedUpdate = false;
    }
    protected void TurnRight()
    {
        //Debug.Log("Turn Right");
        ai.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        moveRightOnFixedUpdate = true;
        moveLeftOnFixedUpdate = false;
    }
    protected void TurnLeft()
    {
        //Debug.Log("Turn Left");
        ai.transform.eulerAngles = new Vector3(0f, -90f, 0f);
        moveRightOnFixedUpdate = false;
        moveLeftOnFixedUpdate = true;
    }
	protected bool MasterInput()
	{
		if (Input.GetKeyDown("f")) /*;Input.GetButtonDown("Follow"))*/
		{
			followMaster = !followMaster;
            //
			Debug.Log(followMaster.ToString());
            //
			if (followMaster)
            {
                if (Mathf.Abs(master.transform.position.x - ai.transform.position.x) < 4)
                {
                    _context.TransitionTo(new FollowState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, true));
                }
				else
				{
                    _context.TransitionTo(new CatchUpState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, true));
                }
            }
            else if (Mathf.Abs(master.transform.position.x - ai.transform.position.x) < 10)
            {
                _context.TransitionTo(new IdleState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, false));
            }
			else
			{
                _context.TransitionTo(new CatchUpState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, false));
            }
            return true;
        }
        return false;
	}

	//Helge
	void Brake(ref Rigidbody rigidbody) => rigidbody.AddForce(Vector2.right * CalculateNextVelocity(rigidbody.velocity.x, (-1) * rigidbody.velocity.x, breakMaxVelocity), horizontalForceMode);
    /// <summary>
    /// Add force to rigidbody. They will be processed on FixedUpdate.
    /// Filtertype used is ForceMode.Force
    /// </summary>
    /// <param name="force">Force to apply</param>
    public void AddForce(Vector3 force) => addedForces += force;
    /// <summary>
    /// Applies a force to rigidbody. The direction of force is relative to Vector Right.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody to receive forces</param>
    void MoveLeft(ref Rigidbody rigidbody) => rigidbody.AddForce(Vector2.right * CalculateNextVelocity(rigidbody.velocity.x, -velocityHorizontal, velocityHorizontal), horizontalForceMode);
    /// <summary>
    /// Applies a force to rigidbody. The direction of force is relative to Vector Right.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody to receive forces</param>
    void MoveRight(ref Rigidbody rigidbody) => rigidbody.AddForce(Vector2.right * CalculateNextVelocity(rigidbody.velocity.x, velocityHorizontal, velocityHorizontal), horizontalForceMode);
    /// <summary>
    /// Applies a force to rigidbody. The direction of force is relative to the gameobjects transforms up.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody to receive forces</param>
    void Jump(ref Rigidbody rigidbody) => rigidbody.AddForce(ai.transform.up * forceJump);
    /// <summary>
    /// Add all forces added by the "AddForce" method.
    /// </summary>
    /// <param name="rigidbody"></param>
    void AddExtraForces(ref Rigidbody rigidbody) => rigidbody.AddForce(addedForces, ForceMode.Force);
    /// <summary>
    /// Calculate the next velocity. Verifies so the velocity stays within max velocity.
    /// </summary>
    /// <param name="velocityFrom">The current Velocity</param>
    /// <param name="additionalVelocity">How much to increase Velocity</param>
    /// <param name="maxVelocity">Maximum velocity. The returned Velocity can NOT be Less than (-maxVelocity) or Larger than (maxVelocity)</param>
    /// <returns>Returns next velocity. Returns 0 if velocity is above maxVelocity.</returns>
    float CalculateNextVelocity(float velocityFrom, float additionalVelocity, float maxVelocity)
    {
        if (additionalVelocity > 0) // Direction of added velocity is to the Right
        {
            if (velocityFrom >= maxVelocity) // Velocity is already above max
            {
                return 0;
            }
            else if (velocityFrom + additionalVelocity > maxVelocity) // Velocity + added Velocity is above max
            {
                return maxVelocity - velocityFrom; // Return the difference in velocity to achieve max velocity
            }
            else // Velocity + added Velocity is within velocityrange
            {
                return additionalVelocity;
            }
        }
        else // Direction of added velocity is to the Left
        {
            if (velocityFrom <= -maxVelocity) // Velocity is already below min
            {
                return 0;
            }
            else if (velocityFrom + additionalVelocity < -maxVelocity) // Velocity + added Velocity is below min
            {
                return (-maxVelocity) - velocityFrom; // Return the difference in velocity to achieve max velocity
            }
            else // Velocity + added Velocity is within velocityrange
            {
                return additionalVelocity;
            }
        }
    }
}
