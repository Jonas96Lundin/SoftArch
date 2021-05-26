using UnityEngine;
/*
 *  Av Helge Herrström
 */
public class CharController : MonoBehaviour
{
    /*
     * Variables
     */
    SlopeDetector sd;
    Rigidbody rb;
    FlipGravity fg;

    // Default variable values
    const float defaultMaxVelocityHorizontal = 4.0f,
                defaultForceJump = 5.0f;

    // Used variable values
    [SerializeField]
    float maxVelocityHorizontal = defaultMaxVelocityHorizontal, // The character will not add force on movement if the absolute velocity would be higher than this variable
          forceJump = defaultForceJump;

    [SerializeField]
    bool paused;

    // If to add force variables
    bool jumpOnFixedUpdate = false,
         moveOnFixedUpdate = false,
         addExtraForcesOnFixedUpdate = false;

    // Variables used every Fixed Update
    Vector2 addedForces = Vector2.zero; // Forces added by other scripts through AddForce method 
    float additionalVelocityHorizontal = 0; // Change in x velocity
    bool jumpedLastFixedUpdate = false;

    /*
     * Methods
     */
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sd = GetComponent<SlopeDetector>();
        fg = GetComponent<FlipGravity>();
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
    /// <param name="force">Force to apply to Rigidbody</param>
    public void AddForce(ref Vector2 force)
    {
        addExtraForcesOnFixedUpdate = true;
        addedForces += force;
    }
    /// <summary>
    /// Pauses the game
    /// </summary>
    /// <param name="pause">Force to apply to Rigidbody</param>
    public void Pause()
    {
        paused = true;
    }
    /// <summary>
    /// UnPauses the game
    /// </summary>
    /// <param name="unpause">Force to apply to Rigidbody</param>
    public void UnPause()
    {
        paused = false;
    }

    /*
     * Update Methods
     */
    private void Update()
    {
        if (!paused)
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
            float inputX = Input.GetAxisRaw("Horizontal"); // Read user input
            if (!moveOnFixedUpdate && inputX != 0) // Move restrictions
            {
                moveOnFixedUpdate = true;
                additionalVelocityHorizontal = inputX * maxVelocityHorizontal;
            }
        }
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            float movementAngle = sd.GetAngleOfSlope(additionalVelocityHorizontal > 0);
            Vector2 velocityChange = Vector2.zero; // Velocity to add onto rigidbody

            // Movement
            if (moveOnFixedUpdate)
            {
                GetMovement(ref velocityChange, rb.velocity, additionalVelocityHorizontal, movementAngle, maxVelocityHorizontal); // Add movement velocity
                moveOnFixedUpdate = false;
                additionalVelocityHorizontal = 0;
            }

            // Jump
            jumpedLastFixedUpdate = false;
            if (jumpOnFixedUpdate)
            {
                if (fg.GetSetFlippedGravity)
                {
                    velocityChange.y = Mathf.Min((-forceJump) - rb.velocity.y, 0); // Add jump velocity
                }
                else
                {
                    velocityChange.y = Mathf.Max(forceJump - rb.velocity.y, 0); // Add jump velocity
                }
                jumpOnFixedUpdate = false;
                jumpedLastFixedUpdate = true;
            }

            rb.AddForce(velocityChange, ForceMode.VelocityChange); // Add velocity change onto rigidbody

            // Extra forces
            if (addExtraForcesOnFixedUpdate)
            {
                rb.AddForce(addedForces, ForceMode.Force); // Add force onto rigidbody
                addedForces.Set(0, 0); // Clear added forces
                addExtraForcesOnFixedUpdate = false;
            }
        }
        
    }

    /*
     * MISC Methods
     */
    /// <summary>
    /// Adds velocity change to a vector
    /// </summary>
    /// <param name="output">Vector2 to add with the velocity change</param>
    /// <param name="velocityBefore">Current velocity</param>
    /// <param name="additionalVelocity">Wanted velocity to add</param>
    /// <param name="additionalVelocityAngle">Wanted angle for velocity</param>
    /// <param name="maxVelocity">Velocity to avoid to exceed</param>
    void GetMovement(ref Vector2 output, Vector2 velocityBefore, float additionalVelocity, float additionalVelocityAngle, float maxVelocity)
    {
        float additionalVelocityX = Mathf.Cos(additionalVelocityAngle) * additionalVelocity,
              additionalVelocityY = Mathf.Sin(additionalVelocityAngle) * additionalVelocity;

        float maxVelocityX = Mathf.Abs(Mathf.Cos(additionalVelocityAngle) * maxVelocity),
              maxVelocityY = Mathf.Abs(Mathf.Sin(additionalVelocityAngle) * maxVelocity);

        output.x = ClampVelocity(velocityBefore.x, additionalVelocityX, maxVelocityX); // Add x movement

        if (!jumpedLastFixedUpdate) // Avoid disrupting the jump if jumping.
        {
            output.y = ClampVelocity(velocityBefore.y, additionalVelocityY, maxVelocityY); // Add y movement
        }
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
