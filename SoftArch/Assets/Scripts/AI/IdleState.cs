using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class IdleState : State
{


	public IdleState(NavMeshAgent agent, CharController master, float attentionSpan, float idleSpeed, float catchUpSpeed)
	{
		this.agent = agent;
		this.master = master;

		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		this.agent.speed = idleSpeed;

		targetPos = agent.transform.position;
	}

	public override void UpdateState()
	{
		MasterInput();
		
		if (distanceToMaster > 30)
		{
			_context.TransitionTo(new CatchUpState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
			return;
		}

		if (!moveOnFixedUpdate)
		{
			agent.speed = idleSpeed;

			if (timeToChange <= 0)
			{
				SetTargetPosition();
				timeToChange = attentionSpan;
			}
			timeToChange -= Time.deltaTime;
		}
		
	}

	public override void SetTargetPosition()
	{
		while (true)
		{
			int newDir = Random.Range(0, 3);
			if (newDir == 0)
			{
				targetPos = new Vector3(agent.transform.position.x - 10, agent.transform.position.y, 0);			
				moveOnFixedUpdate = true;
				break;
			}
			else if (newDir == 1)
			{
				targetPos = new Vector3(agent.transform.position.x + 10, agent.transform.position.y, 0);
				moveOnFixedUpdate = true;
				break;
			}
			else if (newDir == 2)
			{
				break;
			}
		}
	}
}
