using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class CatchUpState : State
{
	public CatchUpState(NavMeshAgent agent, CharController master, Light moveToIndicator)
	{
		this.agent = agent;
		this.master = master;
		this.moveToIndicator = moveToIndicator;

		this.agent.speed = catchUpSpeed;
		this.agent.stoppingDistance = 4.0f;

	}
	public override void UpdateState()
	{
		MasterInput();

		if (!moveOnFixedUpdate)
		{
			SetTargetPosition();
		}
	}

	protected override void SetTargetPosition()
	{
		if (distanceToMaster > 8)
		{
			targetPos = master.transform.position;
			moveOnFixedUpdate = true;
		}
		else if (followMaster)
		{
			_context.TransitionTo(new FollowState(agent, master, moveToIndicator));
		}
		else
		{
			_context.TransitionTo(new IdleState(agent, master, moveToIndicator));
		}
	}

	protected override void MasterInput()
	{
		if (Input.GetButtonDown("Fire2") || Input.GetKeyDown("g"))
		{
			GravityFlip();
		}
		else if (Input.GetKeyDown("f"))
		{
			_context.TransitionTo(new FollowState(agent, master, moveToIndicator));
		}
		else if (Input.GetKeyDown("h"))
		{
			MoveToHoldPosition();
			_context.TransitionTo(new HoldState(agent, master, moveToIndicator));
		}
	}

	public override void HandleProximityTrigger(Collider other)
	{
		//if (isFalling)
		//{
		//	LookForLand(other);
		//}
		else if (other.tag == "InteractableObject" && !objectFound)
		{
			objectPos = new Vector3(other.transform.position.x, agent.transform.position.y, other.transform.position.z);
			targetPos = objectPos;
			_context.TransitionTo(new FoundObjectState(agent, master, moveToIndicator));
		}
	}
}
