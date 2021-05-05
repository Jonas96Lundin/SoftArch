using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class FollowState : State
{
	public FollowState(MeshRenderer mesh, NavMeshAgent agent, CharController master, float attentionSpan, float idleSpeed, float catchUpSpeed)
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
		if (distance < -3.9)
		{
			targetPos = new Vector3(master.transform.position.x - 4, master.transform.position.y, master.transform.position.z);
		}
		else if (distance > 3.9)
		{
			targetPos = new Vector3(master.transform.position.x + 4, master.transform.position.y, master.transform.position.z);
		}
		else
		{
			targetPos = agent.transform.position;
		}
	}
}
