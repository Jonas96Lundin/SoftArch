using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class IdleState : State
{
	public IdleState(GameObject ai, float attentionSpan, float idleSpeed, float catchUpSpeed, CharController master, bool followMaster)
	{
		this.ai = ai;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		speed = catchUpSpeed;
		this.master = master;
		this.followMaster = followMaster;
	}
	public IdleState(GameObject ai)
    {
        this.ai = ai;
    }

    public override void UpdateState()
    {
		if (MasterInput())
			return;
		
        //Change when timer
		if(timeToChange <= 0)
		{
			ChangeDirection();
		}
		timeToChange -= Time.deltaTime;

        Move(speed);


		if (Mathf.Abs(Camera.main.transform.position.x - ai.transform.position.x) > 20)
		{
			//_context.TransitionTo(new FollowState(ai));
			_context.TransitionTo(new CatchUpState(ai, attentionSpan,idleSpeed, catchUpSpeed, master, false));
		}
	}

    public override void ChangeDirection()
    {
		RandomDirection();
		timeToChange = attentionSpan;
	}

	protected void RandomDirection()
	{
		while (true)
		{
			int newDir = Random.Range(0, 3);
			if(newDir == 0)
			{
				TurnForward();
				speed = 0f;
				break;
			}
			else if (newDir == 1)
			{
				TurnRight();
				speed = idleSpeed;
				break;
			}
			else if (newDir == 2)
			{
				TurnLeft();
				speed = idleSpeed;
				break;
			}
		}
	}

	//public override void HandleCollision(Collider other)
	//{
	//    if (nextPos == Vector3.zero && other.gameObject.tag == "CrossRoad")
	//    {
	//        //RandomDirection(other.gameObject.GetComponent<TurnPoint>().canTurnForward, other.gameObject.GetComponent<TurnPoint>().canTurnBack,
	//        //    other.gameObject.GetComponent<TurnPoint>().canTurnRight, other.gameObject.GetComponent<TurnPoint>().canTurnLeft);

	//        RandomDirection(other.gameObject.GetComponent<CrossRoad>().canTurnForward, other.gameObject.GetComponent<CrossRoad>().canTurnBack,
	//            other.gameObject.GetComponent<CrossRoad>().canTurnRight, other.gameObject.GetComponent<CrossRoad>().canTurnLeft);

	//        if (Physics.Raycast(ai.transform.position, ai.transform.TransformDirection(Vector3.forward), out hit, 100, turnPointMask))
	//        {
	//            nextPos = hit.collider.transform.position;
	//        }
	//        //Enter Patrol
	//        ai.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f);
	//        _context.TransitionTo(new PatrolState(ai));
	//    }
	//}
}
