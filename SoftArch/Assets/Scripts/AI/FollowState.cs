using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class FollowState : State
{
	public FollowState(NavMeshAgent agent, CharController master, Light moveToIndicator)
	{
		this.agent = agent;
		this.master = master;
		this.moveToIndicator = moveToIndicator;

		this.agent.speed = followSpeed;
		this.agent.stoppingDistance = followDistance;

		followMaster = true;
	}

	public override void UpdateState()
	{
		if (!GravityFlip() && !isFalling)
		{
			MasterInput();

			if (!isFalling && !moveOnFixedUpdate)
			{
				if (distanceToMaster <= rayDistance && LookForPlayerAround())
					AvoidPlayer();
				else
					SetTargetPosition();
			}
		}
	}

	private void SetTargetPosition()
	{
		targetPos = master.transform.position;
		agent.stoppingDistance = followDistance;
		moveOnFixedUpdate = true;
	}

	protected override void MasterInput()
	{
		//if (Input.GetKeyDown("f"))
		//{
		//	followMaster = false;
		//	_context.TransitionTo(new IdleState(agent, master, moveToIndicator));
		//}
		/*else */if (Input.GetKeyDown("e"))
		{
			followMaster = false;
			SetHoldPosition();
			_context.TransitionTo(new HoldState(agent, master, moveToIndicator));
		}
	}

	public override void HandleProximityTrigger(Collider other)
	{
		if (other.tag == "InteractableObject")
		{
			objectPos = new Vector3(other.transform.position.x, agent.transform.position.y, other.transform.position.z);
			targetPos = objectPos;
			_context.TransitionTo(new FoundObjectState(agent, master, moveToIndicator));
		}
	}
}