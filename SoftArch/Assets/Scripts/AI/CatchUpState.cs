using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class CatchUpState : State
{
	public CatchUpState(NavMeshAgent agent, CharController master)
	{
		this.agent = agent;
		this.master = master;

		this.agent.speed = catchUpSpeed;
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
			targetPos = master.transform.position;
			moveOnFixedUpdate = true;
		}
		else if (followMaster)
		{
			_context.TransitionTo(new FollowState(agent, master));
		}
		else
		{
			_context.TransitionTo(new IdleState(agent, master));
		}
	}
}
