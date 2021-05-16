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

		this.agent.speed = followSpeed;
	}

	public override void UpdateState()
	{
		MasterInput();

		if (!moveOnFixedUpdate)
		{
			SetTargetPosition();
		}
	}

	public override void SetTargetPosition()
	{
		if (distanceToMaster > 8)
		{
			agent.speed = catchUpSpeed;
			targetPos = master.transform.position;
			moveOnFixedUpdate = true;
		}
		else if (distanceToMaster > 4)
		{
			agent.speed = followSpeed;
			targetPos = master.transform.position;
			moveOnFixedUpdate = true;
		}
	}
}
