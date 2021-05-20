using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupDisplayer : MonoBehaviour
{
    [SerializeField] PowerupCollector displayedPowerupCollector;
    [SerializeField] Text flipText;

    // Update is called once per frame
    void Update()
    {
        flipText.text = "X " + displayedPowerupCollector.GetFlipAmount;
    }
}
