using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class OldFollowState : OldState
{
	public OldFollowState(GameObject ai, CharController master, bool followMaster, float attentionSpan, float idleSpeed, float catchUpSpeed, NavMeshAgent agent, MeshRenderer mesh)
	{
		this.ai = ai;
		this.master = master;
		this.followMaster = followMaster;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		maxVelocityHorizontal = catchUpSpeed;
		rb = ai.GetComponent<Rigidbody>();
		moveOnFixedUpdate = false;
		//timeToChange = attentionSpan;
		//masterPosition = master.transform.position;
		//masterVelocityHorizontal = master.GetComponent<Rigidbody>().velocity.x;

		this.agent = agent;
		this.mesh = mesh;
		this.agent.speed = catchUpSpeed;
	}
	public OldFollowState(GameObject ai, CharController master, bool followMaster, float attentionSpan, float idleSpeed, float catchUpSpeed)
	{
		this.ai = ai;
		this.master = master;
		this.followMaster = followMaster;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		maxVelocityHorizontal = catchUpSpeed;
		//rb = ai.GetComponent<Rigidbody>();
		moveOnFixedUpdate = false;
		//timeToChange = attentionSpan;
		//masterPosition = master.transform.position;
		//masterVelocityHorizontal = master.GetComponent<Rigidbody>().velocity.x;
	}
	public OldFollowState(GameObject ai)
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
			if (distance < -2)
			{
				targetPos = new Vector3(master.transform.position.x - 2, master.transform.position.y, master.transform.position.z);
			}
			else if (distance > 2)
			{
				targetPos = new Vector3(master.transform.position.x + 2, master.transform.position.y, master.transform.position.z);
			}
			else
			{
				targetPos = ai.transform.position;
			}
			moveOnFixedUpdate = true;


			//float distance = ai.transform.position.x - master.transform.position.x;
			//if (Mathf.Abs(distance) < catchUpSpeed)
			//{
			//	maxVelocityHorizontal = Mathf.Abs(distance) - 2;
			//}
			//else
			//{
			//	maxVelocityHorizontal = catchUpSpeed;
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
