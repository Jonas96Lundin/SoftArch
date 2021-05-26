using UnityEngine;

public class ReturnToCheckpointObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CheckpointCollector collector))
        {
            collector.ReturnToCheckpoint();
        }
    }
}
