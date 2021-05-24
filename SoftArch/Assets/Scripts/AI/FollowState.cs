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

		if (!isFalling && !moveOnFixedUpdate)
		{
			if (CheckProximity())
				AvoidPlayer();
			else
				SetTargetPosition();
		}
	}

	protected override void SetTargetPosition()
	{
		targetPos = master.transform.position;
		moveOnFixedUpdate = true;
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
			followMaster = false;
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
		/*else */if (other.tag == "InteractableObject" && !objectFound)
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