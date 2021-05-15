using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class FollowState : State
{
	public FollowState(NavMeshAgent agent, CharController master, float attentionSpan, float idleSpeed, float catchUpSpeed)
	{
		this.agent = agent;
		this.master = master;

		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;

		this.agent.speed = followSpeed;
	}

	public override void UpdateState()
	{
		if (MasterInput())
			return;

		if (!moveOnFixedUpdate)
		{
			//if (AvoidObjects())
			//	return;

			SetTargetPosition();
		}
	}

	public override void SetTargetPosition()
	{
		float distance = agent.transform.position.x - master.transform.position.x;
		if (Mathf.Abs(distance) > 8)
		{
			agent.speed = catchUpSpeed;
			targetPos = master.transform.position;
			moveOnFixedUpdate = true;
		}
		else if (Mathf.Abs(distance) > 4)
		{
			agent.speed = followSpeed;
			targetPos = master.transform.position;
			moveOnFixedUpdate = true;
		}
		else
		{
			agent.transform.LookAt(master.transform);
		}







		//float distance = agent.transform.position.x - master.transform.position.x;
		//if (Mathf.Abs(distance) > 8)
		//{
		//	targetPos = master.transform.position;
		//	agent.speed = catchUpSpeed;
		//	moveOnFixedUpdate = true;
		//}
		//else
		//{
		//	targetPos = master.transform.position;
		//	agent.speed = followSpeed;
		//	moveOnFixedUpdate = true;
		//}

		//	if (Mathf.Abs(distance) > (2 * followDistance))
		//{
		//	targetPos = master.transform.position;
		//	agent.speed = catchUpSpeed;
		//	moveOnFixedUpdate = true;
		//}
		//else if (distance < -followDistance)
		//{
		//	if (!master.RBLeftMovementActive || distance < -(followDistance + 1))
		//	{
		//		targetPos = new Vector3(master.transform.position.x - followDistance, master.transform.position.y, master.transform.position.z);
		//		agent.speed = followSpeed;
		//		moveOnFixedUpdate = true;
		//	}
		//}
		//else if (distance > followDistance)
		//{
		//	if (!master.RBRightMovementActive || distance > (followDistance + 1))
		//	{
		//		targetPos = new Vector3(master.transform.position.x + followDistance, master.transform.position.y, master.transform.position.z);
		//		agent.speed = followSpeed;
		//		moveOnFixedUpdate = true;
		//	}
		//}
		//else
		//{
		//	AvoidObjects();
		//	agent.transform.LookAt(master.transform);
		//}
	}
}
