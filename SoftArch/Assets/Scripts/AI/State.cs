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

    protected Vector3 moveDirection;

    //protected static int wallMask = 1 << 8;
    //protected static int playerMask = 1 << 9;
    //protected static int turnPointMask = 1 << 10;
    //protected static int comboMask = wallMask | playerMask;

    //protected float layerMaskHitDistance;
    //protected RaycastHit hit;

    protected static Vector3 lastSeenPos;
    protected static Vector3 nextPos;
    
    protected static string nextTurn;




	

	protected float speed, idleSpeed, followSpeed;
    protected float attentionSpan;
    protected float timeToChange;

    protected bool moveRight, moveLeft/*, jump*/;
    

    public void SetContext(Context context)
    {
        _context = context;
    }

    public abstract void UpdateState();

    public abstract void ChangeDirection();

    protected void TurnForward()
    {
        ai.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        moveRight = false;
        moveLeft = false;
    }
    protected void TurnBack()
    {
        ai.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        moveRight = false;
        moveLeft = false;
    }
    protected void TurnRight()
    {
        ai.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        moveRight = true;
        moveLeft = false;
    }
    protected void TurnLeft()
    {
        ai.transform.eulerAngles = new Vector3(0f, -90f, 0f);
        moveRight = false;
        moveLeft = true;
    }

    protected void Move(float speed)
    {
        Vector3 velocity = ai.transform.forward * speed;
        ai.transform.position += velocity * Time.deltaTime;
	}
}
