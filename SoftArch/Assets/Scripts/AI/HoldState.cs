using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class HoldState : State
{
	public HoldState(NavMeshAgent agent, CharController master, Light moveToIndicator)
	{
		this.agent = agent;
		this.master = master;
		this.moveToIndicator = moveToIndicator;

		this.agent.stoppingDistance = 0.5f;
		this.agent.speed = followSpeed;

		isHolding = false;
		timeToChange = indicationTime;
		moveOnFixedUpdate = true;
	}

	public override void UpdateState()
	{
		if (GravityFlip())
		{
			moveToIndicator.enabled = false;
			isHolding = false;
			_context.TransitionTo(new IdleState(agent, master, moveToIndicator));
		}

		MasterInput();

		if (!isHolding && !moveOnFixedUpdate)
		{			
			if (Vector3.Distance(agent.transform.position, holdPos) > 1.0f)
			{
				//Move Around Player
				if (distanceToMaster <= longRayDistance && Mathf.Abs(agent.transform.position.x - holdPos.x) > 2.0f && LookForPlayerAhead())
				{
					targetPos = master.transform.position + Vector3.back * avoidOffset;
					moveOnFixedUpdate = true;
				}
				else if (targetPos != holdPos)
				{
					targetPos = holdPos;
					moveOnFixedUpdate = true;
				}
					
				//Flashing MoveTo Indicator
				if (moveToIndicator.enabled)
				{
					if (timeToChange > 0)
					{
						if (moveToIndicator.spotAngle <= 1)
							spotlightAngleChange = Mathf.Abs(spotlightAngleChange);
						else if (moveToIndicator.spotAngle >= 40)
							spotlightAngleChange = -spotlightAngleChange;

						moveToIndicator.spotAngle += spotlightAngleChange;
						timeToChange -= Time.deltaTime;
					}
					else
					{
						moveToIndicator.enabled = false;
					}
				}
			}
			else
			{
				moveToIndicator.enabled = false;
				isHolding = true;
			}
		}
	}

	protected override void MasterInput()
	{
		if (Input.GetKeyDown("f"))
		{
			moveToIndicator.enabled = false;
			isHolding = false;
			_context.TransitionTo(new FollowState(agent, master, moveToIndicator));
		}
		else if (Input.GetKeyDown("h"))
		{
			SetHoldPosition();
			moveOnFixedUpdate = true;
			isHolding = false;
			timeToChange = indicationTime;
		}
	}

	public override void HandleProximityTrigger(Collider other)
	{
		if (Vector3.Distance(agent.transform.position, targetPos) < avoidOffset && other.tag == "InteractableObject")
		{
			isHolding = false;
			moveToIndicator.enabled = false;
			objectPos = new Vector3(other.transform.position.x, agent.transform.position.y, other.transform.position.z);
			targetPos = objectPos;
			_context.TransitionTo(new FoundObjectState(agent, master, moveToIndicator));
		}
	}
}
