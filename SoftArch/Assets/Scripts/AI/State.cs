using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>

public abstract class State
{
	//Context
	protected Context _context;
	//Player
	protected CharController master;
	//NavMesh
	protected NavMeshAgent agent;
	protected Vector3 targetPos;
	//Tweakable Const variables
	protected const float idleSpeed = 2.0f,
						  followSpeed = 4.0f,
						  catchUpSpeed = 8.0f,
						  flyBackSpeed = 5.0f,
						  flipRotationSpeed = 5.0f,
						  antiGravity = 2.0f * 9.82f,
						  followDistance = 4.0f,
						  flyBackDistance = 20.0f,
						  avoidOffset = 4.0f,
						  attentionSpan = 1.0f;
	//Static variables
	protected static Vector3 objectPos;
	protected static float timeToChange = 1.0f,
						   distanceToMaster;
	protected static bool followMaster,
						  isJumping,
						  isFalling,
						  isFlyBack,
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
		distanceToMaster = Vector3.Distance(agent.transform.position, master.transform.position);

		if (isFalling)
		{
			//Move
			if (!isFlyBack)
				Fall();
			else
				FlyBack();

			//Rotation
			if (distanceToMaster < followDistance)
			{
				LookAt(master.transform.position);
			}
			else if (!invertedGravity)
			{
				agent.transform.rotation = (Quaternion.Slerp(agent.transform.rotation, Quaternion.Euler(new Vector3(agent.transform.rotation.x, agent.transform.rotation.y, 0)), flipRotationSpeed * Time.deltaTime));
			}
			else
			{
				agent.transform.rotation = (Quaternion.Slerp(agent.transform.rotation, Quaternion.Euler(new Vector3(agent.transform.rotation.x, agent.transform.rotation.y, -180)), flipRotationSpeed * Time.deltaTime));
			}

		}
		else
		{
			//Move
			if (moveOnFixedUpdate && agent.isOnNavMesh)
			{
				agent.SetDestination(targetPos);
				moveOnFixedUpdate = false;
			}

			//Rotation
			if (distanceToMaster < 4)
			{
				LookAt(master.transform.position);
			}
			else if (objectFound && distanceToMaster > 6.0f)
			{
				LookAt(objectPos);
			}
		}
	}

	private void Fall()
	{
		float verticalDistanceToMaster = master.transform.position.y - agent.transform.position.y;

		if (!invertedGravity)
		{
			if (verticalDistanceToMaster > flyBackDistance)
			{
				agent.GetComponent<Rigidbody>().useGravity = false;
				agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
				isFlyBack = true;
			}
			else
			{
				agent.transform.rotation = (Quaternion.Slerp(agent.transform.rotation, Quaternion.Euler(new Vector3(agent.transform.rotation.x, agent.transform.rotation.y, 0)), flipRotationSpeed * Time.deltaTime));
			}
		}
		else
		{
			agent.GetComponent<Rigidbody>().useGravity = false;

			if (verticalDistanceToMaster < -flyBackDistance)
			{
				agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
				isFlyBack = true;
			}
			else
			{
				agent.GetComponent<Rigidbody>().AddForce(Vector3.up * antiGravity, ForceMode.Acceleration);
				agent.transform.rotation = (Quaternion.Slerp(agent.transform.rotation, Quaternion.Euler(new Vector3(agent.transform.rotation.x, agent.transform.rotation.y, -180)), flipRotationSpeed * Time.deltaTime));
			}
		}
	}
	protected void FlyBack()
	{
		if (distanceToMaster > followDistance)
		{
			agent.GetComponent<Rigidbody>().AddForce((master.transform.position - agent.transform.position).normalized * flyBackSpeed, ForceMode.Acceleration);
		}
	}

	private void LookAt(Vector3 target)
	{
		if (invertedGravity)
		{
			agent.transform.LookAt(target, -Vector3.up);
		}
		else
		{
			agent.transform.LookAt(target, Vector3.up);
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
					_context.TransitionTo(new FollowState(agent, master));
				}
				else if (distanceToMaster < 10)
				{
					_context.TransitionTo(new IdleState(agent, master));
				}
				else
				{
					_context.TransitionTo(new CatchUpState(agent, master));
				}
				moveOnFixedUpdate = false;
			}
			else
			{
				followMaster = true;
				objectFound = false;
				_context.TransitionTo(new FollowState(agent, master));
			}
		}
		else if (Input.GetButtonDown("Fire2") || Input.GetKeyDown("g"))
		{
			moveOnFixedUpdate = false;
			agent.enabled = false;
			invertedGravity = !invertedGravity;
			agent.GetComponent<Rigidbody>().isKinematic = false;
			agent.GetComponent<Rigidbody>().useGravity = true;
			agent.GetComponent<AgentLinkMover>().invertedJump = !agent.GetComponent<AgentLinkMover>().invertedJump;
			isFalling = true;
			objectFound = false;
		}
		else if (Input.GetKeyDown("h"))
		{
			targetPos = agent.transform.position;
			objectFound = false;
			followMaster = false;
			_context.TransitionTo(new HoldState(agent, master));

		}
	}

	public void HandleCollision(Collision collision)
	{
		if (!agent.isOnNavMesh)
		{
			if (!invertedGravity && collision.collider.CompareTag("WalkableObject"))
			{
				agent.GetComponent<Rigidbody>().isKinematic = true;
				agent.GetComponent<Rigidbody>().useGravity = true;
				agent.enabled = true;
				isFalling = false;
				isFlyBack = false;
			}
			else if (invertedGravity && collision.collider.CompareTag("WalkableObject180"))
			{
				agent.GetComponent<Rigidbody>().isKinematic = true;
				agent.GetComponent<Rigidbody>().useGravity = true;
				agent.enabled = true;
				isFalling = false;
				isFlyBack = false;
			}
		}
	}
	public void HandleProximityTrigger(Collider other)
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
			_context.TransitionTo(new HoldState(agent, master));
		}
	}
}
