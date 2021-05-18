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
	//Tweakable Const variables
	protected const float followDistance = 4.0f,
						  followSpeed = 4.0f,
						  avoidOffset = 4.0f,
						  rotationSpeed = 5.0f;
	protected float attentionSpan = 1.0f,
					idleSpeed = 3.0f,
					catchUpSpeed = 10.0f;
	//Static varialbles
	protected static Vector3 objectPos;
	protected static float timeToChange = 1.0f,
						   distanceToMaster;
	protected static bool followMaster,
						  isJumping,
						  isFalling,
						  invertedGravity,
						  objectFound;
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
		if (isFalling)
		{
			if (invertedGravity)
			{
				agent.GetComponent<Rigidbody>().AddForce(Vector3.up * (2.0f * 9.82f), ForceMode.Acceleration);
				agent.transform.rotation = (Quaternion.Slerp(agent.transform.rotation, Quaternion.Euler(new Vector3(agent.transform.rotation.x, agent.transform.rotation.y, -180)), rotationSpeed * Time.deltaTime));
			}
			else
			{
				agent.transform.rotation = (Quaternion.Slerp(agent.transform.rotation, Quaternion.Euler(new Vector3(agent.transform.rotation.x, agent.transform.rotation.y, 0)), rotationSpeed * Time.deltaTime));
			}
		}
		else
		{
			distanceToMaster = Mathf.Abs(agent.transform.position.x - master.transform.position.x);
			if (moveOnFixedUpdate)
			{
				if (agent.isOnNavMesh)
				{
					agent.SetDestination(targetPos);
					moveOnFixedUpdate = false;
				}
			}
			if (distanceToMaster < 4)
			{
				agent.transform.LookAt(master.transform);
			}
		}
	}

	protected void MasterInput()
	{
		if (Input.GetKeyDown("f"))
		{
			if (!objectFound)
			{
				followMaster = !followMaster;
				if (followMaster)
				{
					_context.TransitionTo(new FollowState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
				}
				else if (distanceToMaster < 10)
				{
					_context.TransitionTo(new IdleState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
				}
				else
				{
					_context.TransitionTo(new CatchUpState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
				}
				moveOnFixedUpdate = false;
			}
			else
			{
				followMaster = true;
				objectFound = false;
				_context.TransitionTo(new FollowState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
			}
		}
		else if (Input.GetKeyDown("g"))
		{
			moveOnFixedUpdate = false;
			agent.enabled = false;
			invertedGravity = !invertedGravity;
			agent.GetComponent<Rigidbody>().isKinematic = false;
			agent.GetComponent<AgentLinkMover>().invertedJump = !agent.GetComponent<AgentLinkMover>().invertedJump;
			isFalling = true;
			objectFound = false;
		}
	}

	public void HandleCollision(Collision collision)
	{
		if (!agent.isOnNavMesh)
		{
			if (!invertedGravity && collision.collider.CompareTag("WalkableObject"))
			{
				agent.GetComponent<Rigidbody>().isKinematic = true;
				agent.enabled = true;
				isFalling = false;
			}
			else if (invertedGravity && collision.collider.CompareTag("WalkableObject180"))
			{
				agent.GetComponent<Rigidbody>().isKinematic = true;
				agent.enabled = true;
				isFalling = false;
			}
		}
	}
	public void HandleProximityTrigger(Collider other)
	{
		if (isFalling)
		{
			if(!invertedGravity && other.tag != "WalkableObject")
			{
				if (agent.transform.position.y > other.transform.position.y)
				{
					if (agent.transform.position.x > other.transform.position.x)
					{
						agent.GetComponent<Rigidbody>().AddForce(Vector3.right * catchUpSpeed, ForceMode.Acceleration);
					}
					else if (agent.transform.position.x < other.transform.position.x)
					{
						agent.GetComponent<Rigidbody>().AddForce(Vector3.left * catchUpSpeed, ForceMode.Acceleration);
					}
				}
			}
			else if(invertedGravity && other.tag != "WalkableObject180")
			{
				if (agent.transform.position.y < other.transform.position.y)
				{
					if (agent.transform.position.x > other.transform.position.x)
					{
						agent.GetComponent<Rigidbody>().AddForce(Vector3.right * catchUpSpeed, ForceMode.Acceleration);
					}
					else if (agent.transform.position.x < other.transform.position.x)
					{
						agent.GetComponent<Rigidbody>().AddForce(Vector3.left * catchUpSpeed, ForceMode.Acceleration);
					}
				}
			}
		}
		else if (other.tag == "Player")
		{
			float distance = agent.transform.position.x - master.transform.position.x;
			if (distance > 0)
			{
				targetPos = new Vector3(master.transform.position.x + avoidOffset, agent.transform.position.y, master.transform.position.z + avoidOffset);
			}
			else
			{
				targetPos = new Vector3(master.transform.position.x - avoidOffset, agent.transform.position.y, master.transform.position.z + avoidOffset);
			}
			agent.speed = catchUpSpeed;
			moveOnFixedUpdate = true;
		}
		else if (other.tag == "InteractableObject" && !objectFound)
		{
			objectFound = true;
			objectPos = new Vector3(other.transform.position.x, agent.transform.position.y, other.transform.position.z);
			targetPos = objectPos;
			_context.TransitionTo(new HoldState(agent, master, attentionSpan, idleSpeed, catchUpSpeed));
		}
	}
}
