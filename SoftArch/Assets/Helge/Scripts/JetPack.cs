using UnityEngine;
/*
 *  Av Helge Herrström
 */
public class JetPack : MonoBehaviour
{
    CharController controller;
    FlipGravity fg;
    [SerializeField] bool leftClickEffectEnabled = true;
    Vector2 forceUp = new Vector2(0, 500);
    Vector2 forceDown = new Vector2(0, -500);

    void Awake()
    {
        controller = GetComponent<CharController>();
        fg = GetComponent<FlipGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move up
        if (leftClickEffectEnabled && Input.GetButtonDown("Fire1") && controller != null)
        {
            if (fg.GetSetFlippedGravity)
            {
                controller.AddForce(ref forceDown);
            }
            else
            {
                controller.AddForce(ref forceUp);
            }
        }
    }
}
