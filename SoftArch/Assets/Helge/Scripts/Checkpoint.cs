using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Tooltip("Replace checkpoint position with tranform position")] [SerializeField] bool useTransformPosition = true;
    /// <summary>
    /// Position of checkpoint
    /// </summary>
    [Tooltip("The return position. Not used if using transform position")] [SerializeField] Vector2 checkpointPosition = Vector2.zero;
    /// <summary>
    /// Checkpoints with lower priority won't replace checkpoints with higher priority
    /// </summary>
    [Tooltip("Priority over other checkpoints. Lower won't replace higher checkpoints. Equal replace equal.")] [SerializeField] int checkpointPriority = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out CheckpointCollector collector))
            {
                if (useTransformPosition)
                {
                    checkpointPosition.Set(transform.position.x, transform.position.y);
                }
                bool isFlipped = other.GetComponent<FlipGravity>().GetSetFlippedGravity;
                int flipCount = other.GetComponent<PowerupCollector>().GetFlipAmount;
                GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
                collector.AddCheckPoint(ref checkpointPosition, checkpointPriority, isFlipped, flipCount, ref powerups);
            }
        }
    }
}
