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
		this.agent.stoppingDistance = 4.0f;
		followMaster = true; 
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

	protected override void MasterInput()
	{
		if (Input.GetButtonDown("Fire2") || Input.GetKeyDown("g"))
		{
			followMaster = false;
			GravityFlip();
		}
		else if (Input.GetKeyDown("f"))
		{
			followMaster = false;

			if (distanceToMaster < 10)
				_context.TransitionTo(new IdleState(agent, master, moveToIndicator));
			else
				_context.TransitionTo(new CatchUpState(agent, master, moveToIndicator));
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






//objectFound = false;
//followMaster = false;
//isHolding = false;
//Debug.Log("Follow: " + followMaster);
//Debug.Log("Jumping: " + isJumping);
//Debug.Log("Falling: " + isFalling);
//Debug.Log("Holing: " + isHolding);
//Debug.Log("Flyback: " + isFlyBack);
//Debug.Log("InvertedGrav: " + invertedGravity);
//Debug.Log("ObjetFound: " + objectFound);