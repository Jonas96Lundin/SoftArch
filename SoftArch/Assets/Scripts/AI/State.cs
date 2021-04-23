using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public abstract class State
{

    protected Context _context;
    protected GameObject ai;

    protected CharController master;
    protected Vector3 masterPosition;
    protected float masterSpeed;

    protected float speed, idleSpeed, catchUpSpeed/*, followSpeed*/;
    protected float attentionSpan;
    protected float timeToChange;

    protected bool moveRight, moveLeft/*, jump*/;
    protected bool followMaster;


    //protected Vector3 moveDirection;

    //protected static int wallMask = 1 << 8;
    //protected static int playerMask = 1 << 9;
    //protected static int turnPointMask = 1 << 10;
    //protected static int comboMask = wallMask | playerMask;

    //protected float layerMaskHitDistance;
    //protected RaycastHit hit;

    //protected static Vector3 lastSeenPos;
    //protected static Vector3 nextPos;

    //protected static string nextTurn;









    public void SetContext(Context context)
    {
        _context = context;
    }

    public abstract void UpdateState();

    public abstract void ChangeDirection();

    protected void TurnForward()
    {
        Debug.Log("Turn Forward");
        ai.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        moveRight = false;
        moveLeft = false;
    }
    protected void TurnBack()
    {
        Debug.Log("Turn Back");
        ai.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        moveRight = false;
        moveLeft = false;
    }
    protected void TurnRight()
    {
        Debug.Log("Turn Right");
        ai.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        moveRight = true;
        moveLeft = false;
    }
    protected void TurnLeft()
    {
        Debug.Log("Turn Left");
        ai.transform.eulerAngles = new Vector3(0f, -90f, 0f);
        moveRight = false;
        moveLeft = true;
    }

    protected void Move(float speed)
    {
        Vector3 velocity = ai.transform.forward * speed;
        ai.transform.position += velocity * Time.deltaTime;
	}

    protected bool MasterInput()
	{
		if (Input.GetButtonDown("Follow"))
		{
            followMaster = !followMaster;
			if (followMaster)
			{
                if (Mathf.Abs(master.transform.position.x - ai.transform.position.x) < 4)
                {
                    //_context.TransitionTo(new IdleState(ai));
                    _context.TransitionTo(new FollowState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, true));
                }
                else
                {
                    _context.TransitionTo(new CatchUpState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, true));
                }
            }
			else
			{
                if (Mathf.Abs(master.transform.position.x - ai.transform.position.x) < 10)
                {
                    //_context.TransitionTo(new IdleState(ai));
                    _context.TransitionTo(new IdleState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, false));
                }
                else
                {
                    _context.TransitionTo(new CatchUpState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, false));
                }
            }
            return true;
        }

        return false;
	}
}
