using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public abstract class State
{
	//Johan//
	//Context
	protected Context _context;
	//Master
	protected CharController master;
	//NavMesh
	protected NavMeshAgent agent;
	protected Vector3 targetPos;

	//Static varialbles
	protected static bool followMaster;
	//Tweakable variables
	protected const float followDistance = 4.0f;
	protected float attentionSpan = 1.0f;
	protected float idleSpeed = 4.0f,
					catchUpSpeed = 10.0f;
	//Other Variables
	protected float timeToChange;

	protected bool isJumping;
	protected Vector3 startJumpPos, endJumpPos;
	protected float hightDifference;


	protected bool jumpOnFixedUpdate = false,
				   moveOnFixedUpdate = false;


	public void SetContext(Context context)
	{
		_context = context;
	}
	public abstract void UpdateState();

	public void FixedUpdateState()
	{
		if (moveOnFixedUpdate)
		{
			agent.SetDestination(targetPos);
			moveOnFixedUpdate = false;
		}
	}

	public abstract void SetTargetPosition();

	protected void TurnForward()
	{
		agent.transform.eulerAngles = new Vector3(0f, 0f, 0f);
	}
	//protected void TurnBack()
	//{
	//	agent.transform.eulerAngles = new Vector3(0f, 180f, 0f);
	//}
	//protected void TurnRight()
	//{
	//	agent.transform.eulerAngles = new Vector3(0f, 90f, 0f);
	//}
	//protected void TurnLeft()
	//{
	//	agent.transform.eulerAngles = new Vector3(0f, -90f, 0f);
	//}

	protected bool MasterInput()
	{
		if (Input.GetKeyDown("f"))
		{
			followMaster = !followMaster;
			//
			Debug.Log(followMaster.ToString());
			//
			if (followMaster)
			{
				_context.TransitionTo(new FollowState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
			}
			else if (Mathf.Abs(master.transform.position.x - agent.transform.position.x) < 10)
			{
				_context.TransitionTo(new IdleState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
			}
			else
			{
				_context.TransitionTo(new CatchUpState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
			}
			moveOnFixedUpdate = false;
			return true;
		}
		return false;
	}

	public void OnDrawGizmos()
	{
		if (agent.isStopped)
		{
			Gizmos.color = Color.green;
			foreach (var point in agent.path.corners)
			{
				Gizmos.DrawSphere(point, 0.25f);
			}
		}
	}
}
