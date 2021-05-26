using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * By Tinea Larsson
 */

public class jetScript : MonoBehaviour
{
    [Tooltip("FlipGravity")]
    [SerializeField]
    private FlipGravity fg;

    [Tooltip("ParticleSystem to move")]
    [SerializeField]
    private ParticleSystem ps;

    [Tooltip("Type of character")]
    [SerializeField]
    private GameObject thisObj;

    enum charType { player, ai };

    [Tooltip("Type of character")]
    [SerializeField]
    private charType thisCharType;

    float normalY, flippedY;

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (thisCharType)
        {
            case charType.player:
                normalY = -0.6f;
                flippedY = 0.6f;
                break;

            case charType.ai:
                normalY = -0.25f;
                flippedY = 0.4f;
                break;
        }

        //if gravity flipped change gravityfactor of particle
        if (fg.GetSetFlippedGravity)
        {
            Vector3 newPos = new Vector3(thisObj.transform.localPosition.x, flippedY, thisObj.transform.localPosition.z);
            thisObj.transform.localPosition = newPos;
            ps.gravityModifier = -1;
        }
        else
        {
            Vector3 newPos = new Vector3(thisObj.transform.localPosition.x, normalY, thisObj.transform.localPosition.z);
            thisObj.transform.localPosition = newPos;
            ps.gravityModifier = 1;
        }
    }
}
