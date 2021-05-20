using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class HoldState : State
{
	public HoldState(NavMeshAgent agent, CharController master)
	{
		this.agent = agent;
		this.master = master;

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
				_context.TransitionTo(new CatchUpState(agent, master));
			}
		}
		else if (distanceToMaster > 50)
		{
			objectFound = false;
			_context.TransitionTo(new CatchUpState(agent, master));
		}
	}

	public override void SetTargetPosition() { }
}
