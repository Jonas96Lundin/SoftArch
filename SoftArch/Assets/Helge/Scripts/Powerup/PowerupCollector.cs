using UnityEngine;

public class PowerupCollector : MonoBehaviour
{
    int flipAmount = 0;
    public int GetFlipAmount { get => flipAmount; }
    /// <summary>
    /// Increases the amount of flips by one
    /// </summary>
    public void IncreaseAmountOfFlips() => ++flipAmount;
    /// <summary>
    /// Decrease the amount of flips by one
    /// </summary>
    public void DecreaseAmountOfFlips() => --flipAmount;
    
}
