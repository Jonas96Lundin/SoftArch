using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class IdleState : State
{
	public IdleState(NavMeshAgent agent, CharController master, Light moveToIndicator)
	{
		this.agent = agent;
		this.master = master;
		this.moveToIndicator = moveToIndicator;

		this.agent.speed = idleSpeed;

		targetPos = agent.transform.position;
		timeToChange = attentionSpan;
		this.agent.stoppingDistance = 4.0f;
	}

	public override void UpdateState()
	{
		MasterInput();

		if (distanceToMaster > 30)
		{
			_context.TransitionTo(new CatchUpState(agent, master, moveToIndicator));
			return;
		}

		if (!moveOnFixedUpdate && distanceToMaster > 4)
		{
			if (timeToChange <= 0)
			{
				SetTargetPosition();
				timeToChange = attentionSpan;
			}
			timeToChange -= Time.deltaTime;
		}
	}

	protected override void SetTargetPosition()
	{
		while (true)
		{
			int newDir = Random.Range(0, 3);
			if (newDir == 0)
			{
				targetPos = new Vector3(agent.transform.position.x - 10, agent.transform.position.y, 0);
				agent.speed = idleSpeed;
				moveOnFixedUpdate = true;
				break;
			}
			else if (newDir == 1)
			{
				targetPos = new Vector3(agent.transform.position.x + 10, agent.transform.position.y, 0);
				agent.speed = idleSpeed;
				moveOnFixedUpdate = true;
				break;
			}
			else if (newDir == 2)
			{
				break;
			}
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
		if (isFalling)
		{
			LookForLand(other);
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
		else if (other.tag == "InteractableObject" && !objectFound)
		{
			objectPos = new Vector3(other.transform.position.x, agent.transform.position.y, other.transform.position.z);
			targetPos = objectPos;
			_context.TransitionTo(new FoundObjectState(agent, master, moveToIndicator));
		}
	}
}