using UnityEngine;

public class RotationManager : MonoBehaviour
{
    /*
     * Variables
     */
    [SerializeField] Transform transformToRotate; // Transform to rotate
    readonly Vector3 rotationRightAndFlipped = new Vector3(0, 1, 180),
                     rotationLeftAndFlipped = new Vector3(0, 179, 180),
                     rotationRightNotFlipped = new Vector3(0, -179, 0),
                     rotationLeftNotFlipped = new Vector3(0, -1, 0);
    public bool rotateLeft = true, rotateRight = false;
    bool isUpsideDown = false;
    const float rotationSpeed =  5.0f; // Rotation speed
    public bool IsUpsideDown { get => isUpsideDown; set
        {
            if (value != isUpsideDown)
            {
                // Move colliders
                SphereCollider[] colliderS = GetComponents<SphereCollider>();
                foreach (SphereCollider c in colliderS)
                {
                    c.center = new Vector3(c.center.x, c.center.y * -1, c.center.z);
                }
                CapsuleCollider[] colliderC = GetComponents<CapsuleCollider>();
                foreach (CapsuleCollider c in colliderC)
                {
                    c.center = new Vector3(c.center.x, c.center.y * -1, c.center.z);
                }
                isUpsideDown = value;
            }
        }}

    // Update is called once per frame
    void Update()
    {
        UpdateValues();
        if (IsUpsideDown)
        {
            if (rotateRight)
            {
                transformToRotate.rotation = (Quaternion.Slerp(transformToRotate.rotation, Quaternion.Euler(rotationRightAndFlipped), rotationSpeed * Time.deltaTime));
            }
            else if (rotateLeft)
            {
                transformToRotate.rotation = (Quaternion.Slerp(transformToRotate.rotation, Quaternion.Euler(rotationLeftAndFlipped), rotationSpeed * Time.deltaTime));
            }
        }
        else
        {
            if (rotateRight)
            {
                transformToRotate.rotation = (Quaternion.Slerp(transformToRotate.rotation, Quaternion.Euler(rotationRightNotFlipped), rotationSpeed * Time.deltaTime));
            }
            else if (rotateLeft)
            {
                transformToRotate.rotation = (Quaternion.Slerp(transformToRotate.rotation, Quaternion.Euler(rotationLeftNotFlipped), rotationSpeed * Time.deltaTime));
            }
        }

    }
    
    void UpdateValues()
    {
        float inputX = Input.GetAxisRaw("Horizontal"); // Read user input
        if (inputX > 0)
        {
            rotateLeft = false;
            rotateRight = true;
        }
        else if (inputX < 0)
        {
            rotateRight = false;
            rotateLeft = true;
        }
    }
}
