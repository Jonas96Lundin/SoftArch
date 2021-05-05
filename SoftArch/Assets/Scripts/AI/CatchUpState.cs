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
			moveOnFixedUpdate = true;
		}
	}

	public override void SetTargetPosition()
	{
		float distance = agent.transform.position.x - master.transform.position.x;
		if (distance < -3f)
		{
			targetPos = new Vector3(master.transform.position.x - 2, master.transform.position.y, master.transform.position.z);
		}
		else if (distance > 3f)
		{
			targetPos = new Vector3(master.transform.position.x + 2, master.transform.position.y, master.transform.position.z);
		}
		else
		{
			_context.TransitionTo(new IdleState(mesh, agent, master, attentionSpan, idleSpeed, catchUpSpeed));
			return;
		}
	}
}
