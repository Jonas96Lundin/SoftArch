using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeDetectorSpherical : MonoBehaviour
{
    // Slope detection variables
    const float maxColliderY = -0.5f; // If any collision on collider is done above this value it is ignored. "0" is the middle of character.
    Vector2 highestXColPoint = Vector2.zero, // Collision point furthest to the right
            lowestXColPoint = Vector2.zero;  // Collision point furthest to the left
    Vector2 verifiedPos = Vector2.zero;
    int highestXColPointGOID = -1, lowestXColPointGOID = -1;
    float angleLeft = 0, angleRight = 0;
    SortedDictionary<int, int> triggeredObjects = new SortedDictionary<int, int>();

    public bool IsOnGround() => highestXColPointGOID != -1 && lowestXColPointGOID != -1;

    public float GetAngleOfSlope(bool movingRight)
    {
        if (movingRight)
        {
            float aTemp = angleRight;
            angleRight = 0;
            angleLeft = 0;
            return aTemp;
        }
        else
        {
            float aTemp = angleLeft;
            angleLeft = 0;
            angleRight = 0;
            return aTemp;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        int objID = collision.gameObject.GetInstanceID();
        ResetCollisionValuesOfGOID(objID);

        // Find the collision point at the left and right of collider if any
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).thisCollider.material.frictionCombine != PhysicMaterialCombine.Minimum) // No friction
            {
                Vector2 colPos = collision.GetContact(i).point - collision.GetContact(i).thisCollider.bounds.center; // Point of collision relative to character

                //Debug.DrawRay(collision.GetContact(i).thisCollider.bounds.center, Vector3.right * transform.localScale.x/2f, Color.green);
                //Debug.DrawRay(collision.GetContact(i).thisCollider.bounds.center, Vector3.right * -transform.localScale.x/2f, Color.green);

                Vector2 dir = new Vector2(collision.GetContact(i).normal.y, -collision.GetContact(i).normal.x); // Get normal of surface and turn it 90 degrees clockwise (The direction to move)
                dir.Normalize();
                float tempA = Mathf.Atan2(dir.y, dir.x);

                if (colPos.y <= 0)
                {
                    //Debug.DrawRay(collision.GetContact(i).point, collision.GetContact(i).normal, Color.blue, Time.fixedDeltaTime);
                    if (highestXColPointGOID == -1 || colPos.x >= highestXColPoint.x)
                    {
                        angleRight = Mathf.Atan2(dir.y, dir.x);
                        highestXColPoint = colPos;
                        highestXColPointGOID = objID;
                    }
                    if (lowestXColPointGOID == -1 || colPos.x <= lowestXColPoint.x)
                    {
                        angleLeft = Mathf.Atan2(dir.y, dir.x);
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
                angleRight = angleLeft;
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
                angleLeft = angleRight;
            }
        }
    }
}
