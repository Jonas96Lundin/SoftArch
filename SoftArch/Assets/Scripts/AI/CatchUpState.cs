using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatchUpState : State
{
	public CatchUpState(MeshRenderer mesh, NavMeshAgent agent, CharController master, float attentionSpan, float idleSpeed, float catchUpSpeed)
	{
		this.mesh = mesh;
		this.agent = agent;
		this.master = master;
		//rb = agent.GetComponent<Rigidbody>();

		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		this.agent.speed = catchUpSpeed;
	}
	public override void UpdateState()
	{
		if (MasterInput())
			return;

		if (!moveOnFixedUpdate)
		{
			SetTargetPosition();
		}
	}

	public override void SetTargetPosition()
	{
		float distance = agent.transform.position.x - master.transform.position.x;
		if (distance < -followDistance || distance > followDistance)
		{
			targetPos = master.transform.position;
			moveOnFixedUpdate = true;
		}
		else
		{
			_context.TransitionTo(new IdleState(mesh, agent, master, attentionSpan, idleSpeed, catchUpSpeed));
			return;
		}
	}
}
