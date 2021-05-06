using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class FollowState : State
{
	public FollowState(MeshRenderer mesh, NavMeshAgent agent, CharController master, float attentionSpan, float idleSpeed, float catchUpSpeed)
	{
		this.mesh = mesh;
		this.agent = agent;
		this.master = master;
		//rb = agent.GetComponent<Rigidbody>();

		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;

		this.agent.speed = catchUpSpeed;
	}

	public override void UpdateState()
	{
		if (MasterInput())
			return;

		if (!moveOnFixedUpdate)
		{
			SetTargetPosition();
		}
	}

	public override void SetTargetPosition()
	{
		float distance = agent.transform.position.x - master.transform.position.x;

		if (distance < -3.0f)
		{
			if (!master.RBLeftMovementActive || distance < -(followDistance + 1))
			{
				targetPos = new Vector3(master.transform.position.x - followDistance, master.transform.position.y, master.transform.position.z);
				moveOnFixedUpdate = true;
			}
		}
		else if (distance > 3.0f)
		{
			if (!master.RBRightMovementActive || distance > (followDistance +1 ))
			{
				targetPos = new Vector3(master.transform.position.x + followDistance, master.transform.position.y, master.transform.position.z);
				moveOnFixedUpdate = true;
			}
		}
		else
		{
			if (targetPos != agent.transform.position)
			{
				targetPos = agent.transform.position;
				moveOnFixedUpdate = true;
			}
		}
	}
}
