using UnityEngine;

public class FlipPowerup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PowerupCollector collector))
        {
            collector.IncreaseAmountOfFlips();
            gameObject.SetActive(false);
        }
    }
}
