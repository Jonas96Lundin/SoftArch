using UnityEngine;
/// <summary>
/// Jonas script
/// </summary>
public class FlipGravity : MonoBehaviour
{
    /*
     * Variables to notify about gravity change
     */
    SlopeDetector sd;
    RotationManager rm;
    PowerupCollector pc;
    /*
     * Variables
     */
    bool flippedLastUpdate = false;
    [SerializeField] bool flippedGravity = false;
    /*
     * Properties and Methods
     */
    public bool GetSetFlippedGravity
    {
        get => flippedGravity;
        set => flippedGravity = value;
    }
    private void Awake()
    {
        sd = GetComponent<SlopeDetector>();
        rm = GetComponent<RotationManager>();
        pc = GetComponent<PowerupCollector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2")) // Change gravity on user input
        {
            if (pc.GetFlipAmount > 0)
            {
                pc.DecreaseAmountOfFlips();
				GetSetFlippedGravity = !flippedGravity;
			}
        }

        if (flippedGravity != flippedLastUpdate) // If gravity has changes since last update
        {
            // Notify components
            sd.FlippedCollisionDetection = flippedGravity;
            rm.IsUpsideDown = flippedGravity;
            flippedLastUpdate = flippedGravity; // Update variable
        }

        if (!flippedGravity)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }

    }

    private void FixedUpdate()
    {
        if (flippedGravity)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity);
        }
    }
}
