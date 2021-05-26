using UnityEngine;

public class CheckpointCollector : MonoBehaviour
{
    /*
     * Checkpoint data
     */
    Vector2 currentCheckpoint = Vector2.zero;
    int currentPriority = 0;

    /*
     * Extra Checkpoint Data
     */
    int currentFlipCount = 0;
    bool currentIsGravityFlipped = false;
    GameObject[] activePowerups = null;
    /*
     * Properties
     */
    public Vector2 GetCurrentCheckpoint => currentCheckpoint;
    public int GetCurrentPriority => currentPriority;


    /*
     * Methods
     */
    /// <summary>
    /// Replaces checkpoint if priority is equal or higher than current priority.
    /// </summary>
    /// <param name="point">Checkpoint position</param>
    /// <param name="priority">Checkpoint priority</param>
    public void AddCheckPoint(ref Vector2 point, int priority, bool isGravityFlipped, int flipCount, ref GameObject[] powerupsActive)
    {
        if (priority >= currentPriority && !point.Equals(currentCheckpoint))
        {
            currentPriority = priority;
            currentCheckpoint.Set(point.x, point.y);
            currentIsGravityFlipped = isGravityFlipped;
            currentFlipCount = flipCount;
            activePowerups = powerupsActive;
        }
    }

    public void ReturnToCheckpoint()
    {
        transform.position = currentCheckpoint;
        GetComponent<FlipGravity>().GetSetFlippedGravity = currentIsGravityFlipped;
        GetComponent<PowerupCollector>().SetAmountOfFlips(currentFlipCount);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        foreach (GameObject go in activePowerups)
        {
            go.SetActive(true);
        }
    }

    private void Awake()
    {
        currentCheckpoint.Set(transform.position.x, transform.position.y);
        activePowerups = GameObject.FindGameObjectsWithTag("Powerup");
    }
}
