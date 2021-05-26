using UnityEngine;

public class StayWithinMap : MonoBehaviour
{
    [SerializeField] int minY = -25, maxY = 25;

    private void Update()
    {
        if (transform.position.y < minY || transform.position.y > maxY)
        {
            if (TryGetComponent(out CheckpointCollector collector))
            {
                collector.ReturnToCheckpoint();
            }
        }
    }
}
