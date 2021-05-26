using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCheckpointOffGround : MonoBehaviour
{
    bool onGroundLastFrame = true;
    // Update is called once per frame
    void Update()
    {
        if (TryGetComponent(out SlopeDetector sd))
        {
            if (sd.IsOnGround())
            {
                onGroundLastFrame = true;
            }
            else
            {
                if (onGroundLastFrame)
                {
                    onGroundLastFrame = false;
                    if (TryGetComponent(out CheckpointCollector collector))
                    {
                        Vector2 checkpointPosition = transform.position;
                        bool isFlipped = GetComponent<FlipGravity>().GetSetFlippedGravity;
                        int flipCount = GetComponent<PowerupCollector>().GetFlipAmount;
                        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
                        collector.AddCheckPoint(ref checkpointPosition, collector.GetCurrentPriority, isFlipped, flipCount, ref powerups);
                    }
                }
            }
        }
    }
}
