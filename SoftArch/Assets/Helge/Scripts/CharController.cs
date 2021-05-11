using UnityEngine;
/*
 *  Av Helge Herrström
 */
public class CharController : MonoBehaviour
{
    /*
     * Variables
     */
    SlopeDetectorSpherical sd;
    Rigidbody rb;

    const float defaultMaxVelocityHorizontal = 4.0f,
                defaultForceJump = 250.0f,
                groundDetectRayLength = 1.1f;

    [SerializeField]
    float maxVelocityHorizontal = 4.0f, // The character will not add force on movement if the absolute velocity would be higher than this variable
          forceJump = 250.0f;

    bool jumpOnFixedUpdate = false,
         moveOnFixedUpdate = false,
         addExtraForcesOnFixedUpdate = false;

    // Variables used every Fixed Update
    Vector3 addedForces = Vector3.zero; // Forces added by other scripts through AddForce method 
    float additionalVelocityHorizontal = 0; // Change in x velocity

    //Debug variables
    [SerializeField]
    bool debugRayMovementDirection = false;
    /*
     * Methods
     */
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sd = GetComponent<SlopeDetectorSpherical>();
    }
    /*
     * Public Methods
     */
    /// <summary>
    /// Returns true if rigidbody is moving horizontally
    /// </summary>
    public bool RBHorizontalMovementActive => rb.velocity.x != 0;
    /// <summary>
    /// Returns true if rigidbody is moving to the right
    /// </summary>
    public bool RBRightMovementActive => rb.velocity.x > 0;
    /// <summary>
    /// Returns true if rigidbody is moving to the left
    /// </summary>
    public bool RBLeftMovementActive => rb.velocity.x < 0;
    /// <summary>
    /// Returns true if user input is moving rigidbody horizontally
    /// </summary>
    public bool InputHorizontalMovementActive => moveOnFixedUpdate;
    /// <summary>
    /// Returns true if user input is moving rigidbody to the right
    /// </summary>
    public bool InputRightMovementActive => additionalVelocityHorizontal > 0;
    /// <summary>
    /// Returns true if user input is moving rigidbody to the left
    /// </summary>
    public bool InputLeftMovementActive => additionalVelocityHorizontal < 0;
    /// <summary>
    /// Returns the velocity that will be applied to player the next fixed update.
    /// </summary>
    //public Vector2 InputMovement => GetMovement(rb.velocity, additionalVelocityHorizontal, sd.GetAngleOfSlope(additionalVelocityHorizontal > 0), maxVelocityHorizontal);

    /// <summary>
    /// Changes the velocity limit that can be achieved through player input into default value.
    /// The max velocity can still be overtaken through external forces or by calling AddForce()
    /// </summary>
    public void ChangeMaxVelocityHorizontalIntoDefaultValue() => maxVelocityHorizontal = defaultMaxVelocityHorizontal;
    /// <summary>
    /// Changes the velocity limit that can be achieved through player input.
    /// The max velocity can still be overtaken through external forces or by calling AddForce()
    /// </summary>
    /// <param name="value">Replacement velocity</param>
    public void ChangeMaxVelocityHorizontal(float value) => maxVelocityHorizontal = value;
    /// <summary>
    /// Changes the force used while jumping into default value.
    /// </summary>
    public void ChangeForceJumpIntoDefaultValue() => forceJump = defaultForceJump;
    /// <summary>
    /// Changes the force used while jumping.
    /// </summary>
    /// <param name="value">Replacement force</param>
    public void ChangeForceJump(float value) => forceJump = value;
    /// <summary>
    /// Add force to rigidbody. They will be processed on FixedUpdate.
    /// Filtertype used is ForceMode.Force
    /// </summary>
    /// <param name="force">Force to apply</param>
    public void AddForce(Vector3 force)
    {
        addExtraForcesOnFixedUpdate = true;
        addedForces += force;
    }

    /*
     * Update Methods
     */
    private void Update()
    {
        //Jump
        if (!jumpOnFixedUpdate)
        {
            if (sd.IsOnGround()) // Jump restrictions
            {
                jumpOnFixedUpdate = Input.GetButtonDown("Jump"); // Jump on user input
            }
        }
        // Movement
        float inputX = Input.GetAxisRaw("Horizontal");
        if (!moveOnFixedUpdate && inputX != 0)
        {
            moveOnFixedUpdate = true;
            additionalVelocityHorizontal = inputX * maxVelocityHorizontal;
        }
    }

    void FixedUpdate()
    {
        float movementAngle = sd.GetAngleOfSlope(additionalVelocityHorizontal > 0);
        Vector2 movementForces = Vector2.zero;
        
        // Movement
        if (moveOnFixedUpdate)
        {
            movementForces += GetMovement(rb.velocity, additionalVelocityHorizontal, movementAngle, maxVelocityHorizontal);
            //Move(ref rb, movementAngle);
            moveOnFixedUpdate = false;
            additionalVelocityHorizontal = 0;
        }

        // Jump
        if (jumpOnFixedUpdate)
        {
            movementForces.y += 5f;
            //Jump(ref rb);
            jumpOnFixedUpdate = false;
        }

        AddForce(ref movementForces, ref rb);

        // Extra forces
        if (addExtraForcesOnFixedUpdate)
        {
            AddExtraForces(ref rb);
            addedForces = Vector3.zero; // Clear added forces
            addExtraForcesOnFixedUpdate = false;
        }
    }

    /*
     * Force Methods
     */
    /// <summary>
    /// Add all forces added by the "AddForce" method.
    /// </summary>
    /// <param name="rigidbody"></param>
    void AddExtraForces(ref Rigidbody rigidbody) => rigidbody.AddForce(addedForces, ForceMode.Force);
    /// <summary>
    /// Applies a force to rigidbody. The direction of force is relative to Vector Right.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody to receive forces</param>
    //void Move(ref Rigidbody rigidbody, float angle) => rigidbody.AddForce(GetMovement(rigidbody.velocity, additionalVelocityHorizontal, angle, maxVelocityHorizontal), ForceMode.VelocityChange);
    void AddForce(ref Vector2 forceVector, ref Rigidbody rigidbody) => rb.AddForce(forceVector, ForceMode.VelocityChange);
    /// <summary>
    /// Applies a force to rigidbody. The direction of force is relative to the gameobjects transforms up.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody to receive forces</param>
    void Jump(ref Rigidbody rigidbody) => rigidbody.AddForce(transform.up * forceJump);

    /*
     * MISC Methods
     */
    Vector2 GetMovement(Vector2 velocityBefore, float additionalVelocity, float additionalVelocityAngle, float maxVelocity)
    {
        float additionalVelocityX = Mathf.Cos(additionalVelocityAngle) * additionalVelocity,
              additionalVelocityY = Mathf.Sin(additionalVelocityAngle) * additionalVelocity;
        float maxVelocityX = Mathf.Abs(Mathf.Cos(additionalVelocityAngle) * maxVelocity),
              maxVelocityY = Mathf.Abs(Mathf.Sin(additionalVelocityAngle) * maxVelocity);

        Vector2 movement = new Vector2
        {
            x = ClampVelocity(velocityBefore.x, additionalVelocityX, maxVelocityX),
            y = ClampVelocity(velocityBefore.y, additionalVelocityY, maxVelocityY)
        };

        if(debugRayMovementDirection)
            Debug.DrawRay(transform.position, movement, Color.red, Time.deltaTime);
        return movement;
    }
    float ClampVelocity(float velocityBefore, float additionalVelocity, float maxVelocity)
    {
        if (additionalVelocity > 0) // Direction of added velocity is to the Right
        {
            if (velocityBefore >= maxVelocity) // Velocity is already above max
            {
                return 0;
            }
            else if (velocityBefore + additionalVelocity > maxVelocity) // Velocity + added Velocity is above max
            {
                return maxVelocity - velocityBefore; // Return the difference in velocity to achieve max velocity
            }
            else // Velocity + added Velocity is within velocityrange
            {
                return additionalVelocity;
            }
        }
        else // Direction of added velocity is to the Left
        {
            if (velocityBefore <= -maxVelocity) // Velocity is already below min
            {
                return 0;
            }
            else if (velocityBefore + additionalVelocity < -maxVelocity) // Velocity + added Velocity is below min
            {
                return (-maxVelocity) - velocityBefore; // Return the difference in velocity to achieve max velocity
            }
            else // Velocity + added Velocity is within velocityrange
            {
                return additionalVelocity;
            }
        }
    }
}
