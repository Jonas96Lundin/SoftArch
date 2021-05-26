using UnityEngine;
using UnityEngine.UI;

public class PowerupDisplayer : MonoBehaviour
{
    PowerupCollector displayedPowerupCollector;
    [SerializeField] Text flipText;
    private void Awake()
    {
        displayedPowerupCollector = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerupCollector>();
    }
    // Update is called once per frame
    void Update()
    {
        flipText.text = "X " + displayedPowerupCollector.GetFlipAmount;
    }
}
