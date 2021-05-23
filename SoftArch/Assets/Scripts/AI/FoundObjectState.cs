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

		this.agent.stoppingDistance = 4.0f;
		this.agent.speed = followSpeed;

		objectFound = true;
		moveOnFixedUpdate = true;
	}

	public override void UpdateState()
	{
		MasterInput();

		if (followMaster)
		{
			if (distanceToMaster > 12)
			{
				objectFound = false;
				isHolding = false;
				_context.TransitionTo(new CatchUpState(agent, master, moveToIndicator));
			}
		}
		else if (distanceToMaster > 50)
		{
			objectFound = false;
			isHolding = false;
			_context.TransitionTo(new CatchUpState(agent, master, moveToIndicator));
		}
	}

	protected override void SetTargetPosition() { }

	protected override void MasterInput()
	{
		if (Input.GetButtonDown("Fire2") || Input.GetKeyDown("g"))
		{
			objectFound = false;
			GravityFlip();
			_context.TransitionTo(new IdleState(agent, master, moveToIndicator));
		}
		else if (Input.GetKeyDown("f"))
		{
			objectFound = false;
			_context.TransitionTo(new FollowState(agent, master, moveToIndicator));
		}
		else if (Input.GetKeyDown("h"))
		{
			objectFound = false;
			MoveToHoldPosition();
			_context.TransitionTo(new HoldState(agent, master, moveToIndicator));
		}
	}

	public override void HandleProximityTrigger(Collider other)
	{
		if (isFalling)
		{
			if (!invertedGravity && other.tag != "WalkableObject")
			{
				agent.GetComponent<Rigidbody>().AddForce(-(other.transform.position - agent.transform.position).normalized * catchUpSpeed, ForceMode.Force);
			}
			else if (invertedGravity && other.tag != "WalkableObject180")
			{
				agent.GetComponent<Rigidbody>().AddForce(-(other.transform.position - agent.transform.position).normalized * catchUpSpeed, ForceMode.Force);
			}
		}
		else if (other.tag == "Player")
		{
			float distance = agent.transform.position.x - master.transform.position.x;
			if (distance > 0)
			{
				targetPos = new Vector3(master.transform.position.x + avoidOffset, agent.transform.position.y, master.transform.position.z - avoidOffset);
			}
			else
			{
				targetPos = new Vector3(master.transform.position.x - avoidOffset, agent.transform.position.y, master.transform.position.z - avoidOffset);
			}
			agent.speed = catchUpSpeed;
			moveOnFixedUpdate = true;
		}
	}
}
