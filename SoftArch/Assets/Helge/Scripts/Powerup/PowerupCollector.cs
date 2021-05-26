using UnityEngine;

public class PowerupCollector : MonoBehaviour
{
    int flipAmount = 0;
    [SerializeField] bool infiniteFlips = false;
    public int GetFlipAmount
    {
        get
        {
            if (infiniteFlips)
                return 1;
            return flipAmount;
        }
    }

    /// <summary>
    /// Increases the amount of flips by one
    /// </summary>
    public void IncreaseAmountOfFlips()
    {
        if (!infiniteFlips)
        {
            ++flipAmount;
        }
    }

    /// <summary>
    /// Increases the amount of flips by one
    /// </summary>
    public void SetAmountOfFlips(int value)
    {
        flipAmount = value;
    }
    /// <summary>
    /// Decrease the amount of flips by one
    /// </summary>
    public void DecreaseAmountOfFlips()
    {
        if (!infiniteFlips)
        {
            --flipAmount;
        }
    }
    
}
