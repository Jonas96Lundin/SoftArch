using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchUpState : State
{
	public CatchUpState(GameObject ai, float attentionSpan, float idleSpeed, float catchUpSpeed, CharController master, bool followMaster)
	{
		this.ai = ai;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		speed = catchUpSpeed;
		this.master = master;
		this.followMaster = followMaster;
	}
	public CatchUpState(GameObject ai)
	{
		this.ai = ai;
	}
	public override void UpdateState()
	{
		ChangeDirection();
		Move(speed);
	}

	public override void ChangeDirection()
	{
		if (MasterInput())
			return;

		if (moveRight)
		{
			if (ai.transform.position.x > Camera.main.transform.position.x)
			{
				TurnLeft();
			}
		}
		else if (moveLeft)
		{
			if (ai.transform.position.x < Camera.main.transform.position.x)
			{
				TurnRight();
			}
		}
		else
		{
			if (ai.transform.position.x > Camera.main.transform.position.x)
			{
				TurnLeft();
			}
			else if (ai.transform.position.x < Camera.main.transform.position.x)
			{
				TurnRight();
			}
			else
			{
				Debug.Log("Turn Up or Down?");
			}
		}

		if (followMaster)
		{
			if (Mathf.Abs(Camera.main.transform.position.x - ai.transform.position.x) < 4)
			{
				_context.TransitionTo(new FollowState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, true));
			}
		}
		else if (Mathf.Abs(Camera.main.transform.position.x - ai.transform.position.x) < 2)
		{
			//_context.TransitionTo(new IdleState(ai));
			_context.TransitionTo(new IdleState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, false));
		}

		//Vector2 direction = ((Vector2)Camera.main.transform.position - (Vector2)ai.transform.position).normalized;
		//ai.transform.eulerAngles = new Vector3(0f, -90f, 0f);
		//ai.transform.eulerAngles = (ai.transform.position - Camera.main.transform.position).normalized * 360f;
	}
}
