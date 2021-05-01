using UnityEngine;
/*
 *  Av Helge Herrström
 */
public class CharController : MonoBehaviour
{
    /*
     * Variables
     */
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
    float currentVelocityHorizontal = 0; // Change in x velocity

    /*
     * Methods
     */
    private void Awake() => rb = GetComponent<Rigidbody>();
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
    public bool RBLeftMovementActive => rb.velocity.x > 0;
    /// <summary>
    /// Returns true if user input is moving rigidbody horizontally
    /// </summary>
    public bool InputHorizontalMovementActive => moveOnFixedUpdate;
    /// <summary>
    /// Returns true if user input is moving rigidbody to the right
    /// </summary>
    public bool InputRightMovementActive => currentVelocityHorizontal > 0;
    /// <summary>
    /// Returns true if user input is moving rigidbody to the left
    /// </summary>
    public bool InputLeftMovementActive => currentVelocityHorizontal < 0;
    /// <summary>
    /// Returns the velocity that will be applied to player the next fixed update.
    /// </summary>
    public float InputMovement => GetClampedVelocity(rb.velocity.x, currentVelocityHorizontal, maxVelocityHorizontal);

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
            Physics.Raycast(transform.position, Vector2.down, out RaycastHit info, groundDetectRayLength); // Raycast down
            bool isOnGround = info.collider != null;
            if (isOnGround) // Jump restrictions
            {
                jumpOnFixedUpdate = Input.GetButtonDown("Jump"); // Jump on user input
            }
        }
        // Movement
        float inputX = Input.GetAxisRaw("Horizontal");
        if (!moveOnFixedUpdate && inputX != 0)
        {
            moveOnFixedUpdate = true;
            currentVelocityHorizontal = inputX * maxVelocityHorizontal;
        }
    }

    void FixedUpdate()
    {
        // Movement
        if (moveOnFixedUpdate)
        {
            Move(ref rb);
            moveOnFixedUpdate = false;
            currentVelocityHorizontal = 0;
        }

        // Jump
        if (jumpOnFixedUpdate)
        {
            Jump(ref rb);
            jumpOnFixedUpdate = false;
        }

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
    void Move(ref Rigidbody rigidbody) => rigidbody.AddForce(Vector2.right * GetClampedVelocity(rigidbody.velocity.x, currentVelocityHorizontal, maxVelocityHorizontal), ForceMode.VelocityChange);
    /// <summary>
    /// Applies a force to rigidbody. The direction of force is relative to the gameobjects transforms up.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody to receive forces</param>
    void Jump(ref Rigidbody rigidbody) => rigidbody.AddForce(transform.up * forceJump);

    /*
     * MISC Methods
     */
    /// <summary>
    /// Calculate the next velocity. Verifies so the velocity stays within max velocity after adding "additionalVelocity".
    /// The returned velocity should replace the "additionalVelocity". 
    /// For example, if "velocityBefore" + "additionalVelocity" is within max velocity range, the returned velocity will be the same as "additionalVelocity". 
    /// Or if outside range, the returned velocity is the velocity missing to reach max velocity.
    /// </summary>
    /// <param name="velocityFrom">The current Velocity</param>
    /// <param name="additionalVelocity">How much to increase Velocity</param>
    /// <param name="maxVelocity">Maximum velocity. The returned Velocity can NOT be Less than (-maxVelocity) or Larger than (maxVelocity)</param>
    /// <returns>Returns next velocity to add to current velocity. Returns 0 if above maxVelocity range.</returns>
    float GetClampedVelocity(float velocityBefore, float additionalVelocity, float maxVelocity)
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
