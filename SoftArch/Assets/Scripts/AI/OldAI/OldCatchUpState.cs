using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OldCatchUpState : OldState
{
	public OldCatchUpState(GameObject ai, CharController master, bool followMaster, float attentionSpan, float idleSpeed, float catchUpSpeed, NavMeshAgent agent, MeshRenderer mesh)
	{
		this.ai = ai;
		this.master = master;
		this.followMaster = followMaster;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		maxVelocityHorizontal = catchUpSpeed;
		rb = ai.GetComponent<Rigidbody>();

		this.agent = agent;
		this.mesh = mesh;
		this.agent.speed = catchUpSpeed;
	}
	public OldCatchUpState(GameObject ai, CharController master, bool followMaster, float attentionSpan, float idleSpeed, float catchUpSpeed)
	{
		this.ai = ai;
		this.master = master;
		this.followMaster = followMaster;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		maxVelocityHorizontal = catchUpSpeed;
		//rb = ai.GetComponent<Rigidbody>();
	}
	public OldCatchUpState(GameObject ai)
	{
		this.ai = ai;
	}
	public override void UpdateState()
	{
		if (MasterInput())
			return;

		if (!moveOnFixedUpdate)
		{
			float distance = ai.transform.position.x - master.transform.position.x;
			if (distance < -3)
			{
				targetPos = new Vector3(master.transform.position.x - 2, master.transform.position.y, master.transform.position.z);
			}
			else if (distance > 3)
			{
				targetPos = new Vector3(master.transform.position.x + 2, master.transform.position.y, master.transform.position.z);
			}
			else
			{
				_context.TransitionTo(new OldIdleState(ai, master, false, attentionSpan, idleSpeed, catchUpSpeed, agent, mesh));
				return;
			}
			moveOnFixedUpdate = true;


			//float distance = ai.transform.position.x - master.transform.position.x;
			//if (Mathf.Abs(distance) > catchUpSpeed)
			//{
			//	maxVelocityHorizontal = catchUpSpeed;
			//}
			//else if(Mathf.Abs(distance) > 5.0f)
			//{
			//	maxVelocityHorizontal = Mathf.Abs(distance)/* - 2*/;
			//}
			//else
			//{
			//	//	//_context.TransitionTo(new IdleState(ai));
			//	_context.TransitionTo(new IdleState(ai, master, false, attentionSpan, idleSpeed, catchUpSpeed));
			//	return;
			//}

			//ChangeDirection();

			//moveOnFixedUpdate = true;
		}
	}

	public override void ChangeDirection()
	{
		if (ai.transform.position.x > master.transform.position.x)
		{
			if (!RBLeftMovementActive)
				TurnLeft();

			currentVelocityHorizontal = -maxVelocityHorizontal;
		}
		else if (ai.transform.position.x < master.transform.position.x)
		{
			if (!RBRightMovementActive)
				TurnRight();

			currentVelocityHorizontal = maxVelocityHorizontal;
		}
		else
		{
			if (RBLeftMovementActive || RBLeftMovementActive)
				TurnForward();

			maxVelocityHorizontal = 0;
			currentVelocityHorizontal = maxVelocityHorizontal;
		}
	}
}
