using UnityEngine;
/*
 *  Av Helge Herrström
 */
public class CharController : MonoBehaviour
{
    const float velocityHorizontal = 5.0f, // The character will not add force on movement if the absolute velocity would be higher than this variable
                forceJump = 500.0f,
                groundDetectRayLength = 1.1f,
                breakMaxVelocity = 1f;
    const ForceMode horizontalForceMode = ForceMode.VelocityChange;
    bool jumpOnFixedUpdate = false,
         brakeIsAllowed = true; // The character will not brake if this is false and glide further. Is usefull when applying forces on character and not have the force abruptly stop.
    bool enableBreakWhenVelocityIsZero = false;
    bool moveLeftOnFixedUpdate = false,
         moveRightOnFixedUpdate = false;
    
    Vector3 addedForces = Vector3.zero;

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

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            AddForce(new Vector3(400, 100));
        }
        Rigidbody rigidbody = GetComponent<Rigidbody>();
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
        float movement = Input.GetAxis("Horizontal");
        if (!moveRightOnFixedUpdate && movement > 0)
        {
            moveRightOnFixedUpdate = true;
        }
        else if (!moveLeftOnFixedUpdate && movement < 0)
        {
            moveLeftOnFixedUpdate = true;
        }
        //brake
        if (!brakeIsAllowed) // If not alowed to brake
        {
            if (enableBreakWhenVelocityIsZero && rigidbody.velocity.x == 0) // If to turn on brake when character stops
            {
                enableBreakWhenVelocityIsZero = false;
                brakeIsAllowed = true;
            }
        }
    }
    void FixedUpdate()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        // Movement
        if (moveRightOnFixedUpdate)
        {
            MoveRight(ref rigidbody);
            moveRightOnFixedUpdate = false;
        }
        else if (moveLeftOnFixedUpdate)
        {
            MoveLeft(ref rigidbody);
            moveLeftOnFixedUpdate = false;
        }
        else // If not moving
        {
            if (brakeIsAllowed) // If allowed to brake
            {
                Brake(ref rigidbody);
            }
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
    void Brake(ref Rigidbody rigidbody) => rigidbody.AddForce(Vector2.right * CalculateNextVelocity(rigidbody.velocity.x, (-1)*rigidbody.velocity.x, breakMaxVelocity), horizontalForceMode);
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
    void Jump(ref Rigidbody rigidbody) => rigidbody.AddForce(transform.up * forceJump);
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
