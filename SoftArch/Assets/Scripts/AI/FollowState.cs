using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class FollowState : State
{
	public FollowState(GameObject ai, float attentionSpan, float idleSpeed ,float catchUpSpeed, CharController master, bool followMaster)
	{
		this.ai = ai;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		speed = catchUpSpeed;
		this.master = master;
		this.followMaster = followMaster;
		masterPosition = master.transform.position;
	}
	public FollowState(GameObject ai)
	{
		this.ai = ai;
	}
	public override void UpdateState()
	{
		ChangeDirection();

		//Match player speed???
		masterSpeed = master.transform.position.x - masterPosition.x;
		masterPosition = master.transform.position;

		masterSpeed = 20;
		Move(masterSpeed);

		//Vector3 deltaMovement = masterPosition - master.transform.position;
		//ai.transform.position += new Vector3(deltaMovement.x * parallaxEffect.x, deltaMovement.y * parallaxEffect.y, 0);
		//lastCameraPos = cameraTransform.position;
	}

	public override void ChangeDirection()
	{
		if (MasterInput())
			return;

		if (moveRight)
		{
			if (ai.transform.position.x > master.transform.position.x)
			{
				TurnLeft();
			}
		}
		else if (moveLeft)
		{
			if (ai.transform.position.x < master.transform.position.x)
			{
				TurnRight();
			}
		}
		else
		{
			if (ai.transform.position.x > master.transform.position.x)
			{
				TurnLeft();
			}
			else if (ai.transform.position.x < master.transform.position.x)
			{
				TurnRight();
			}
			else
			{
				Debug.Log("Turn Up or Down?");
			}
		}

		


		//if (Mathf.Abs(master.transform.position.x - ai.transform.position.x) < 4)
		//{
		//	//_context.TransitionTo(new IdleState(ai));
		//	_context.TransitionTo(new IdleState(ai, attentionSpan, idleSpeed, catchUpSpeed, master));
		//}

		//Vector2 direction = ((Vector2)Camera.main.transform.position - (Vector2)ai.transform.position).normalized;
		//ai.transform.eulerAngles = new Vector3(0f, -90f, 0f);
		//ai.transform.eulerAngles = (ai.transform.position - Camera.main.transform.position).normalized * 360f;
	}


}
