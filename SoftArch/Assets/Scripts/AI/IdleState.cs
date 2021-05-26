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
		this.agent.stoppingDistance = followDistance;

		targetPos = agent.transform.position;
		timeToChange = attentionSpan;
		moveOnFixedUpdate = true;
	}

	public override void UpdateState()
	{
		if (!GravityFlip() || !isFalling)
		{
			MasterInput();

			if (distanceToMaster > 30)
			{
				_context.TransitionTo(new CatchUpState(agent, master, moveToIndicator));
				return;
			}

			if (!moveOnFixedUpdate)
			{
				if (distanceToMaster <= rayDistance && LookForPlayerAround())
				{
					AvoidPlayer();
				}
				else if (distanceToMaster > followDistance)
				{
					if (timeToChange <= 0)
					{
						SetTargetPosition();
						timeToChange = attentionSpan;
					}
					timeToChange -= Time.deltaTime;
				}
			}
		}
		
	}

	private void SetTargetPosition()
	{
		while (true)
		{
			int newDir = Random.Range(0, 3);
			if (newDir == 0)
			{
				targetPos = new Vector3(agent.transform.position.x - 10, agent.transform.position.y, 0);
				agent.stoppingDistance = 0.5f;
				moveOnFixedUpdate = true;
				break;
			}
			else if (newDir == 1)
			{
				targetPos = new Vector3(agent.transform.position.x + 10, agent.transform.position.y, 0);
				agent.stoppingDistance = 0.5f;
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
		if (Input.GetKeyDown("f"))
		{
			_context.TransitionTo(new FollowState(agent, master, moveToIndicator));
		}
		else if (Input.GetKeyDown("h"))
		{
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