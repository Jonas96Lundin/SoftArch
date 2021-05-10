using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class CatchUpState : State
{
	public CatchUpState(NavMeshAgent agent, CharController master, float attentionSpan, float idleSpeed, float catchUpSpeed)
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
		if (Mathf.Abs(distance) > followDistance)
		{
			targetPos = master.transform.position;
			moveOnFixedUpdate = true;
		}
		else
		{
			_context.TransitionTo(new IdleState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
			return;
		}
	}
}
