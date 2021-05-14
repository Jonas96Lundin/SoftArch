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
	//RayCast
	protected RaycastHit hit;
	protected const int playerMask = 1 << 6;
	//Tweakable Const variables
	protected const float followDistance = 4.0f;
	protected float attentionSpan = 1.0f,
					idleSpeed = 3.0f,
					catchUpSpeed = 10.0f,
					followSpeed = 4.0f;
	//Static varialbles
	protected static float timeToChange = 1.0f;
	protected static bool followMaster,
						  isJumping,
						  isFalling,
						  invertedGravity;
	//Other Variables
	protected bool jumpOnFixedUpdate = false,
				   moveOnFixedUpdate = false;


	public void SetContext(Context context)
	{
		_context = context;
	}
	public abstract void UpdateState();
	public abstract void SetTargetPosition();

	public void FixedUpdateState()
	{
		Debug.Log("Speed: " + agent.speed);

		if (isFalling && invertedGravity)
		{
			agent.GetComponent<Rigidbody>().AddForce(Vector3.up * (2.0f * 9.82f), ForceMode.Acceleration);
		}
		else if (AvoidObjects() || moveOnFixedUpdate)
		{
			if (agent.isOnNavMesh)
			{
				agent.SetDestination(targetPos);
				moveOnFixedUpdate = false;
			}
		}
	}

	protected bool AvoidObjects()
	{
		if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.forward), out hit, 1.0f, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * 1.0f, Color.green);
			agent.speed = catchUpSpeed;
			targetPos = new Vector3(agent.transform.position.x, agent.transform.position.y, master.transform.position.z + 5.5f) - agent.transform.TransformDirection(Vector3.forward) * 5.5f;
			moveOnFixedUpdate = true;
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * 1.0f, Color.red);
		}

		if (Physics.Raycast(agent.transform.position, - agent.transform.TransformDirection(Vector3.forward), out hit, 1.0f, playerMask))
		{
			Debug.DrawRay(agent.transform.position, - agent.transform.TransformDirection(Vector3.forward) * 1.0f, Color.green);
			agent.speed = catchUpSpeed;
			targetPos = new Vector3(agent.transform.position.x, agent.transform.position.y, master.transform.position.z + 5.5f) + agent.transform.TransformDirection(Vector3.forward) * 5.5f;
			moveOnFixedUpdate = true;
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, -agent.transform.TransformDirection(Vector3.forward) * 1.0f, Color.red);
		}

		if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.right), out hit, 1.0f, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * 1.0f, Color.green);
			agent.speed = catchUpSpeed;
			targetPos = new Vector3(agent.transform.position.x, agent.transform.position.y, master.transform.position.z + 5.5f) - agent.transform.TransformDirection(Vector3.right) * 5.5f;
			moveOnFixedUpdate = true;
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * 1.0f, Color.red);
		}

		if (Physics.Raycast(agent.transform.position, -agent.transform.TransformDirection(Vector3.right), out hit, 1.0f, playerMask))
		{
			Debug.DrawRay(agent.transform.position, -agent.transform.TransformDirection(Vector3.right) * 1.0f, Color.green);
			agent.speed = catchUpSpeed;
			targetPos = new Vector3(agent.transform.position.x, agent.transform.position.y, master.transform.position.z + 5.5f) + agent.transform.TransformDirection(Vector3.right) * 5.5f;
			moveOnFixedUpdate = true;
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, -agent.transform.TransformDirection(Vector3.right) * 1.0f, Color.red);
		}
		return false;
	}

	protected bool MasterInput()
	{
		if (Input.GetKeyDown("f"))
		{
			followMaster = !followMaster;
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
		else if (Input.GetKeyDown("g"))
		{
			moveOnFixedUpdate = false;
			agent.enabled = false;
			invertedGravity = !invertedGravity;
			agent.GetComponent<Rigidbody>().isKinematic = false;
			isFalling = true;
		}
		return false;
	}

	public void HandleCollision(Collision collision)
	{
		if (!agent.isOnNavMesh && collision.collider.CompareTag("WalkableObject"))
		{
			agent.GetComponent<Rigidbody>().isKinematic = true;
			agent.GetComponent<AgentLinkMover>().invertedJump = !agent.GetComponent<AgentLinkMover>().invertedJump;
			agent.enabled = true;
			isFalling = false;
		}
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
