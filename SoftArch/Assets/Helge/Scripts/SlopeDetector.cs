using System.Collections.Generic;
using UnityEngine;
/*
 *  Av Helge Herrström
 */
public class SlopeDetector : MonoBehaviour
{

    /*
     * Variables
     */
    float highestXColPoint = 0, // x of Collision furthest to the right
          lowestXColPoint = 0;  // x of Collision furthest to the left
    int highestXColPointGOID = -1, // ID of GameObject to the right
        lowestXColPointGOID = -1; // ID of GameObject to the left
    float angleRight = 0, // Angle of Collision to the right
          angleLeft = 0; // Angle of Collision to the left
    public bool FlippedCollisionDetection { get; set; } = false;
    SortedDictionary<int, int> triggeredObjects = new SortedDictionary<int, int>(); // Stores number of current collisions with objects. Key is GameObject IDs.

    
    /*
     * Public Methods
     */
    /// <summary>
    /// Returns true if collisions values of slopes are stored.
    /// </summary>
    /// <returns>Returns true if on ground</returns>
    public bool IsOnGround() => highestXColPointGOID != -1 && lowestXColPointGOID != -1;

    /// <summary>
    /// Must be called every fixed update. Returns angle of slope under collider.
    /// </summary>
    /// <param name="movingRight">If moving right</param>
    /// <returns>Returns angle of slope under collider</returns>
    public float GetAngleOfSlope(bool movingRight)
    {
        float aTemp;
        if (movingRight)
            aTemp = angleRight;
        else
            aTemp = angleLeft;
        angleLeft = 0;
        angleRight = 0;
        return aTemp;
    }

    /*
     * Collision Events
     */
    void OnCollisionStay(Collision collision)
    {
        int objID = collision.gameObject.GetInstanceID(); // Get ID
        EmptyCollisionValuesOfGOID(objID); // Empty stored values of the object in collision.

        // Find values of collisions, and replace currently stored values if collision are further to the left or right.
        for (int i = 0; i < collision.contactCount; i++) // For each collision
        {
            if (collision.GetContact(i).thisCollider.material.frictionCombine != PhysicMaterialCombine.Minimum) // Skip collisions with characters colliders that has no friction
            {
                Vector2 colPos = collision.GetContact(i).point - collision.GetContact(i).thisCollider.bounds.center; // Point of collision relative to character
                Vector2 dir;
                if (FlippedCollisionDetection)
                {
                    dir = new Vector2(-collision.GetContact(i).normal.y, collision.GetContact(i).normal.x); // Get normal of surface and turn it 90 degrees counter clockwise (The direction to move)
                }
                else
                {
                    dir = new Vector2(collision.GetContact(i).normal.y, -collision.GetContact(i).normal.x); // Get normal of surface and turn it 90 degrees clockwise (The direction to move)
                }
                dir.Normalize();

                if ((colPos.y <= 0 && !FlippedCollisionDetection) || (colPos.y >= 0 && FlippedCollisionDetection)) // If collision is below middle of collider
                {
                    if (highestXColPointGOID == -1 || colPos.x >= highestXColPoint) // Collision is further to the right
                    {
                        // Replace current collision values with the new collision
                        angleRight = Mathf.Atan2(dir.y, dir.x);
                        highestXColPoint = colPos.x;
                        highestXColPointGOID = objID;
                    }
                    if (lowestXColPointGOID == -1 || colPos.x <= lowestXColPoint) // Collision is further to the left
                    {
                        // Replace current collision values with the new collision
                        angleLeft = Mathf.Atan2(dir.y, dir.x);
                        lowestXColPoint = colPos.x;
                        lowestXColPointGOID = objID;
                    }
                }
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        int objID = collision.gameObject.GetInstanceID(); // Get ID
        if (triggeredObjects.ContainsKey(objID)) // If list contains key for object
        {
            triggeredObjects[objID]++; // Add to number of collisions
        }
        else // If list DO NOT contain key for object
        {
            triggeredObjects.Add(objID, 1); // Add key and number of collisions to list
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        int objID = collision.gameObject.GetInstanceID(); // Get ID
        triggeredObjects[objID]--; // Count down number of collisions
        if (triggeredObjects[objID] == 0) // If no collisions with object
        {
            triggeredObjects.Remove(objID); // Remove object from list
            EmptyCollisionValuesOfGOID(objID); // Remove collision values with objects
        }
    }

    /*
     * MISC Methods
     */
     /// <summary>
     /// Empties stored collision values of gameobject with said ID. The values will be replaced with another objects that is colliding.
     /// </summary>
     /// <param name="objID"></param>
    void EmptyCollisionValuesOfGOID(int objID)
    {
        if (objID == highestXColPointGOID) // ID is game object to the right
        {
            if (objID == lowestXColPointGOID) // ID is game object to the left
            {
                highestXColPointGOID = -1; // Set to a "null" value. No values to replace with. 
            }
            else // Is NOT game object to the left
            {
                // Replace values with collision values to the left
                highestXColPointGOID = lowestXColPointGOID;
                highestXColPoint = lowestXColPoint;
                angleRight = angleLeft;
            }
        }
        if (objID == lowestXColPointGOID) // ID is game object to the left
        {
            if (objID == highestXColPointGOID) // ID is game object to the right
            {
                lowestXColPointGOID = -1; // Set to a "null" value. No values to replace with.
            }
            else // Is NOT game object to the right
            {
                //Replace values with collision values to the right
                lowestXColPointGOID = highestXColPointGOID;
                lowestXColPoint = highestXColPoint;
                angleLeft = angleRight;
            }
        }
    }
}
