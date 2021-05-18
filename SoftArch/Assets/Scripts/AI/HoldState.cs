using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class HoldState : State
{
	public HoldState(NavMeshAgent agent, CharController master, float attentionSpan, float idleSpeed, float catchUpSpeed)
	{
		this.agent = agent;
		this.master = master;

		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;

		this.agent.speed = idleSpeed;
	}

	public override void UpdateState()
	{
		MasterInput();

		if (followMaster)
		{
			if (distanceToMaster > 12)
			{
				objectFound = false;
				_context.TransitionTo(new CatchUpState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
			}
		}
		else if (distanceToMaster > 50)
		{
			objectFound = false;
			_context.TransitionTo(new CatchUpState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
		}
	}

	public override void SetTargetPosition() { }
}
