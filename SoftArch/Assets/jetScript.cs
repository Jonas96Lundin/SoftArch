using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetScript : MonoBehaviour
{
    public FlipGravity fg;
    
    public ParticleSystem ps;

    public GameObject thisObj;

    public enum charType { player, ai };
    public charType thisCharType;

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
