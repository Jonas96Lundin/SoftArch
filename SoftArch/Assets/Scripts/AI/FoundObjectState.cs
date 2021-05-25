using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class FoundObjectState : State
{
	public FoundObjectState(NavMeshAgent agent, CharController master, Light moveToIndicator)
	{
		this.agent = agent;
		this.master = master;
		this.moveToIndicator = moveToIndicator;

		this.agent.stoppingDistance = 1.5f;
		this.agent.speed = followSpeed;

		objectFound = true;
		moveOnFixedUpdate = true;
	}

	public override void UpdateState()
	{
		if (GravityFlip())
		{
			objectFound = false;
			_context.TransitionTo(new IdleState(agent, master, moveToIndicator));
		}

		MasterInput();

		if (followMaster && distanceToMaster > 12)
		{
			objectFound = false;
			_context.TransitionTo(new CatchUpState(agent, master, moveToIndicator));
		}
		else if (distanceToMaster > 50)
		{
			objectFound = false;
			_context.TransitionTo(new CatchUpState(agent, master, moveToIndicator));
		}

		if (!moveOnFixedUpdate)
		{
			if (distanceToMaster <= rayDistance && LookForPlayerAround())
			{
				agent.stoppingDistance = followDistance;
				AvoidPlayer();
			}
			else if (targetPos != objectPos && distanceToMaster > 6)
			{
				targetPos = objectPos;
				agent.stoppingDistance = 1.5f;
				moveOnFixedUpdate = true;
			}
		}
		
	}

	protected override void MasterInput()
	{
		if (Input.GetKeyDown("f"))
		{
			objectFound = false;
			_context.TransitionTo(new FollowState(agent, master, moveToIndicator));
		}
		else if (Input.GetKeyDown("h"))
		{
			objectFound = false;
			followMaster = false;
			SetHoldPosition();
			_context.TransitionTo(new HoldState(agent, master, moveToIndicator));
		}
	}

	public override void HandleProximityTrigger(Collider other) { }
}
