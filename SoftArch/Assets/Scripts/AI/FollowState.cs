using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class FollowState : State
{
	public FollowState(NavMeshAgent agent, CharController master, float attentionSpan, float idleSpeed, float catchUpSpeed)
	{
		this.agent = agent;
		this.master = master;

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

		if (distance < -followDistance)
		{
			if (!master.RBLeftMovementActive || distance < -(followDistance + 1))
			{
				targetPos = new Vector3(master.transform.position.x - followDistance, master.transform.position.y, master.transform.position.z);
				moveOnFixedUpdate = true;
			}

		}
		else if (distance > followDistance)
		{
			if (!master.RBRightMovementActive || distance > (followDistance + 1))
			{
				targetPos = new Vector3(master.transform.position.x + followDistance, master.transform.position.y, master.transform.position.z);
				moveOnFixedUpdate = true;
			}
		}
	}
}
