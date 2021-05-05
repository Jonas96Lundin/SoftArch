using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class OldIdleState : OldState
{


	public OldIdleState(GameObject ai, CharController master, bool followMaster, float attentionSpan, float idleSpeed, float catchUpSpeed, NavMeshAgent agent, MeshRenderer mesh)
	{
		this.ai = ai;
		this.master = master;
		this.followMaster = followMaster;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		maxVelocityHorizontal = idleSpeed;
		rb = ai.GetComponent<Rigidbody>();


		timeToChange = attentionSpan;
		//TurnForward();
		maxVelocityHorizontal = 0;
		currentVelocityHorizontal = maxVelocityHorizontal;
		moveOnFixedUpdate = true;

		this.agent = agent;
		this.mesh = mesh;
		this.agent.speed = idleSpeed;
	}

	public OldIdleState(GameObject ai, CharController master, bool followMaster, float attentionSpan, float idleSpeed, float catchUpSpeed)
	{
		this.ai = ai;
		this.master = master;
		this.followMaster = followMaster;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		maxVelocityHorizontal = idleSpeed;

		//rb = ai.GetComponent<Rigidbody>();


		//timeToChange = attentionSpan;
		////TurnForward();
		//maxVelocityHorizontal = 0;
		//currentVelocityHorizontal = maxVelocityHorizontal;
		//moveOnFixedUpdate = true;

		timeToChange = attentionSpan;
		targetPos = ai.transform.position;
		moveOnFixedUpdate = true;
	}
	public OldIdleState(GameObject ai)
	{
		this.ai = ai;
	}

	public override void UpdateState()
	{
		if (MasterInput())
			return;

		if (Mathf.Abs(master.transform.position.x - ai.transform.position.x) > 30)
		{
			//_context.TransitionTo(new FollowState(ai));
			_context.TransitionTo(new OldCatchUpState(ai, master, false, attentionSpan, idleSpeed, catchUpSpeed, agent, mesh));
			return;
		}

		//if (!moveOnFixedUpdate)
		//{
		//	if (timeToChange <= 0)
		//	{
		//		ChangeDirection();
		//		timeToChange = attentionSpan;
		//	}
		//	moveOnFixedUpdate = true;
		//}

		if (!moveOnFixedUpdate)
		{
			if (timeToChange <= 0)
			{
				SetTargetPosition();
				timeToChange = attentionSpan;
				moveOnFixedUpdate = true;
			}
		}
		//Change when timer
		timeToChange -= Time.deltaTime;
	}

	public void SetTargetPosition()
	{
		while (true)
		{
			int newDir = Random.Range(0, 3);
			if (newDir == 0)
			{
				targetPos = new Vector3(ai.transform.position.x - 5, ai.transform.position.y, 0);
				break;
			}
			else if (newDir == 1)
			{
				targetPos = new Vector3(ai.transform.position.x + 5, ai.transform.position.y, 0);
				break;
			}
			else if (newDir == 2)
			{
				targetPos = ai.transform.position;
				TurnForward();
				break;
			}
		}
	}
	public override void ChangeDirection()
	{
		while (true)
		{
			int newDir = Random.Range(0, 3);
			if (newDir == 0)
			{
				if (!RBLeftMovementActive)
					TurnLeft();

				maxVelocityHorizontal = idleSpeed;
				currentVelocityHorizontal = -maxVelocityHorizontal;
				break;
			}
			else if (newDir == 1)
			{
				if (!RBLeftMovementActive)
					TurnRight();

				maxVelocityHorizontal = idleSpeed;
				currentVelocityHorizontal = maxVelocityHorizontal;
				break;
			}
			else if (newDir == 2)
			{
				if (RBLeftMovementActive || RBLeftMovementActive)
					TurnForward();

				maxVelocityHorizontal = 0;
				currentVelocityHorizontal = maxVelocityHorizontal;
				break;
			}
		}
	}
}
