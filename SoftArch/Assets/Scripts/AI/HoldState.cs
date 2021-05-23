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

		this.agent.stoppingDistance = 0.0f;
		this.agent.speed = followSpeed;

		isHolding = false;
		moveOnFixedUpdate = true;
		timeToChange = 0.0f;
	}

	public override void UpdateState()
	{
		MasterInput();

		if (!isHolding && !moveOnFixedUpdate)
		{
			Vector3 endPos = new Vector3(agent.pathEndPosition.x, agent.transform.position.y, agent.pathEndPosition.z);
			if (agent.transform.position == endPos)
			{
				Debug.Log("HOLDING");
				moveToIndicator.enabled = false;
				isHolding = true;
			}
			else
			{
				if(timeToChange < indicationTime)
				{
					if (moveToIndicator.spotAngle <= 1)
						spotlightAngleChange = Mathf.Abs(spotlightAngleChange);
					else if (moveToIndicator.spotAngle >= 40)
						spotlightAngleChange = -spotlightAngleChange;

					moveToIndicator.spotAngle += spotlightAngleChange;
					timeToChange += Time.deltaTime;
				}
				else
				{
					moveToIndicator.enabled = false;
					isHolding = true;
					timeToChange = 0;
				}
			}
		}
	}


	protected override void SetTargetPosition() { }

	protected override void MasterInput()
	{
		if (Input.GetButtonDown("Fire2") || Input.GetKeyDown("g"))
		{
			moveToIndicator.enabled = false;
			isHolding = false;
			GravityFlip();
			_context.TransitionTo(new IdleState(agent, master, moveToIndicator));
		}
		else if (Input.GetKeyDown("f"))
		{
			moveToIndicator.enabled = false;
			isHolding = false;
			_context.TransitionTo(new FollowState(agent, master, moveToIndicator));
		}
		else if (Input.GetKeyDown("h"))
		{
			MoveToHoldPosition();
			isHolding = false;
			timeToChange = 0;
			moveOnFixedUpdate = true;
			

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
		else if (!isHolding && other.tag == "Player")
		{
			if (other.transform.position.z >= 0)
			{
				agent.velocity = new Vector3(agent.velocity.x, agent.velocity.y, agent.velocity.z - 2.5f);
			}
			else
			{
				agent.velocity = new Vector3(agent.velocity.x, agent.velocity.y, agent.velocity.z + 2.5f);
			}
			//float distance = agent.transform.position.x - master.transform.position.x;
			//if (distance > 0)
			//{
			//	targetPos = new Vector3(master.transform.position.x + avoidOffset, agent.transform.position.y, master.transform.position.z - avoidOffset);
			//}
			//else
			//{
			//	targetPos = new Vector3(master.transform.position.x - avoidOffset, agent.transform.position.y, master.transform.position.z - avoidOffset);
			//}
			//agent.speed = catchUpSpeed;
			//moveOnFixedUpdate = true;
		}
	}
}
