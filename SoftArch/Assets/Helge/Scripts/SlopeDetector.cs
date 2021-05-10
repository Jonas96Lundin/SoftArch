using System.Collections.Generic;
using UnityEngine;

public class SlopeDetector : MonoBehaviour
{
    // Slope detection variables
    const float maxColliderY = -0.60f; // If any collision on collider is done above this value it is ignored. "0" is the middle of character.
    Vector2 highestXColPoint = Vector2.zero, // Collision point furthest to the right
            lowestXColPoint = Vector2.zero;  // Collision point furthest to the left
    Vector2 verifiedPos = Vector2.zero;
    int highestXColPointGOID = -1, lowestXColPointGOID = -1;
    SortedDictionary<int, int> triggeredObjects = new SortedDictionary<int, int>();

    public bool IsOnGround() => highestXColPointGOID != -1 && lowestXColPointGOID != -1;

    private void Update()
    {
        if ((highestXColPointGOID == -1 || lowestXColPointGOID == -1) && (highestXColPointGOID != lowestXColPointGOID))
        {
            Debug.Log("ERROR!, High = " + highestXColPointGOID + ", Low = " + lowestXColPointGOID);
        }
        DrawSlopeDetectLine();
    }
    public float GetAngleOfSlope(bool movingRight)
    {
        if (movingRight)
        {
            if (highestXColPointGOID == -1)
                return 0;
            return GetAngleFromCollisionPoint(ref highestXColPoint);
        }
        else
        {
            if (lowestXColPointGOID == -1)
                return 0;
            return GetAngleFromCollisionPoint(ref lowestXColPoint);
        }
    }

    float GetAngleFromCollisionPoint(ref Vector2 point)
    {
        Physics.Raycast(new Vector3(transform.position.x + point.x, transform.position.y), Vector3.down * -point.y, out RaycastHit info); // Raycast onto collided surface
        if (info.collider == null)
        {
            //Debug.DrawRay(new Vector3(transform.position.x + point.x, transform.position.y), Vector3.down * -point.y, Color.red, Time.fixedDeltaTime);
            Debug.Log("Collider not found!");
            return 0;
        }
        //Debug.DrawRay(new Vector3(transform.position.x + point.x, transform.position.y), Vector3.down * -point.y, Color.blue, Time.fixedDeltaTime);
        Vector2 dir = new Vector2(info.normal.y, -info.normal.x); // Get normal of surface and turn it 90 degrees clockwise (The direction to move)
        dir.Normalize();
        return Mathf.Atan2(dir.y, dir.x);
    }


    private void OnCollisionStay(Collision collision)
    {
        int objID = collision.gameObject.GetInstanceID();
        ResetCollisionValuesOfGOID(objID);

        // Find the collision point at the left and right of collider if any
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).thisCollider.material.frictionCombine != PhysicMaterialCombine.Minimum) // No friction
            {
                Vector2 colPos = collision.GetContact(i).point - transform.position; // Point of collision relative to character
                if (IsCollisionIllogical(ref colPos))
                {
                    continue;
                }
                if (colPos.y < maxColliderY)
                {
                    if (highestXColPointGOID == -1 || colPos.x > highestXColPoint.x)
                    {
                        highestXColPoint = colPos;
                        highestXColPointGOID = objID;
                    }
                    if (lowestXColPointGOID == -1 || colPos.x < lowestXColPoint.x)
                    {
                        lowestXColPoint = colPos;
                        lowestXColPointGOID = objID;
                    }
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        int objID = collision.gameObject.GetInstanceID();
        if (triggeredObjects.ContainsKey(objID))
        {
            triggeredObjects[objID]++;
        }
        else
        {
            triggeredObjects.Add(objID, 1);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        int objID = collision.gameObject.GetInstanceID();
        triggeredObjects[objID]--;
        if (triggeredObjects[objID] == 0)
        {
            triggeredObjects.Remove(objID);
            ResetCollisionValuesOfGOID(objID);
        }
    }

    void ResetCollisionValuesOfGOID(int objID)
    {
        if (objID == highestXColPointGOID)
        {
            if (objID == lowestXColPointGOID)
            {
                highestXColPointGOID = -1;
            }
            else
            {
                highestXColPointGOID = lowestXColPointGOID;
                highestXColPoint = lowestXColPoint;
            }
        }
        if (objID == lowestXColPointGOID)
        {
            if (objID == highestXColPointGOID)
            {
                lowestXColPointGOID = -1;
            }
            else
            {
                lowestXColPointGOID = highestXColPointGOID;
                lowestXColPoint = highestXColPoint;
            }
        }
    }
    
    void DrawSlopeDetectLine()
    {
        Vector3 test = transform.position;
        test.y += maxColliderY;
        test.x -= transform.localScale.x / 2f;
        Debug.DrawRay(test, Vector3.right * transform.localScale.x, Color.green);
    }

    bool IsCollisionIllogical(ref Vector2 collisionPoint)
    {
        float angle = GetAngleFromCollisionPoint(ref collisionPoint);
        if (collisionPoint.x > 0) // Right side of character
        {
            if (angle >= 0) // Angle is pointing up right (0 - 90 degrees)
            {
                return false; // Logical collision
            }
            else
            {
                return true; // Illogical collision
            }
        }
        else // Left side of character
        {
            if (angle <= 0) // Angle is pointing down right (0 - (-90) degrees)
            {
                return false; // Logical collision
            }
            else
            {
                return true; // Illogical collision
            }
        }
    }
}
