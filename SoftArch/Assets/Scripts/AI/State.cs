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
	protected Vector3 startPos = new Vector3(-2.0f, 1.5f, 0.0f);
	protected static Vector3 targetPos;
	protected Light moveToIndicator;
	//Tweakable Const variables
	protected const float idleSpeed = 2.0f,
						  followSpeed = 4.0f,
						  catchUpSpeed = 5.0f,
						  flyBackSpeed = 5.0f,
						  flipRotationSpeed = 5.0f,
						  antiGravity = 2.0f * 9.82f,
						  followDistance = 4.0f,
						  flyBackDistance = 20.0f,
						  avoidOffset = 4.0f,
						  rayDistance = 3.0f,
						  attentionSpan = 1.0f,
						  indicationTime = 3.0f;
	//Static variables
	protected static Vector3 objectPos;
	protected static float timeToChange = 1.0f,
						   spotlightAngleChange = 2.8f,
						   distanceToMaster;
	protected static bool followMaster,
						  isJumping,
						  isFalling,
						  isHolding,
						  isFlyBack,
						  invertedGravity,
						  objectFound;
	//Other Variables
	protected static bool jumpOnFixedUpdate = false,
				   moveOnFixedUpdate = false;

	//RayCast
	protected static int groundMask = 1 << 3;
	protected static int playerMask = 1 << 6;
	protected static int ground180Mask = 1 << 8;

	protected static int comboMask = groundMask | playerMask;


	protected float layerMaskHitDistance;
	protected RaycastHit hit;

	public void SetContext(Context context)
	{
		_context = context;
	}

	public abstract void UpdateState();
	protected abstract void SetTargetPosition();
	public void FixedUpdateState()
	{
		distanceToMaster = Vector3.Distance(agent.transform.position, master.transform.position);

		if (isFalling)
		{
			//Move
			if (isFlyBack || !FreeFall())
				FlyBack(startPos);


			//Rotation
			if (distanceToMaster < avoidOffset)
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
			if (distanceToMaster < avoidOffset)
			{
				LookAt(master.transform.position);
			}
			else if (objectFound && distanceToMaster > 6.0f)
			{
				LookAt(objectPos);
			}
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

	//On Ground
	protected bool CheckProximity()
	{
		//Forward
		if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.forward), out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
		}

		//Back
		if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.back), out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * hit.distance, Color.green);
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * rayDistance, Color.red);
		}

		//Right
		if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.right), out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * hit.distance, Color.green);
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * rayDistance, Color.red);
		}

		//Left
		if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.left), out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * hit.distance, Color.green);
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * rayDistance, Color.red);
		}

		
		//Forward/Right
		if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized, out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * hit.distance, Color.green);
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
		}

		//Forwerd/Left
		if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized, out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * hit.distance, Color.green);
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
		}

		//Back/Right
		if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized, out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * hit.distance, Color.green);
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
		}

		//Back/Left
		if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized, out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * hit.distance, Color.green);
			return true;
		}
		else
		{
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
		}

		return false;
	}
	protected void AvoidPlayer()
	{
		float distance = agent.transform.position.x - master.transform.position.x;
		if (distance > 0)
		{
			targetPos = new Vector3(master.transform.position.x + avoidOffset, agent.transform.position.y, master.transform.position.z - avoidOffset);
		}
		else
		{
			targetPos = new Vector3(master.transform.position.x - avoidOffset, agent.transform.position.y, master.transform.position.z - avoidOffset);
		}
		//agent.speed = catchUpSpeed;
		moveOnFixedUpdate = true;
	}
	protected void FoundObject(Collider interestingObject)
	{
		objectPos = new Vector3(interestingObject.transform.position.x, agent.transform.position.y, interestingObject.transform.position.z);
		targetPos = objectPos;
		_context.TransitionTo(new FoundObjectState(agent, master, moveToIndicator));
	}
	protected void MoveToHoldPosition()
	{
		moveToIndicator.spotAngle = 1.0f;

		if (master.GetComponent<RotationManager>().rotateLeft)
			targetPos = new Vector3(master.transform.position.x - avoidOffset, master.transform.position.y, master.transform.position.z);
		else
			targetPos = new Vector3(master.transform.position.x + avoidOffset, master.transform.position.y, master.transform.position.z);

		moveToIndicator.transform.position = targetPos + Vector3.up * 3.5f;
		moveToIndicator.enabled = true;
	}

	//In Air
	private bool FreeFall()
	{
		float verticalDistanceToMaster = master.transform.position.y - agent.transform.position.y;

		if (!invertedGravity)
		{
			if (verticalDistanceToMaster > flyBackDistance)
			{
				agent.GetComponent<Rigidbody>().useGravity = false;
				agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
				isFlyBack = true;
				return false;
			}
		}
		else
		{
			if (verticalDistanceToMaster < -flyBackDistance)
			{
				agent.GetComponent<Rigidbody>().useGravity = false;
				agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
				isFlyBack = true;
				return false;
			}
			else
			{
				agent.GetComponent<Rigidbody>().AddForce(Vector3.up * antiGravity, ForceMode.Acceleration);
			}
		}

		LookForLand();

		return true;
	}
	private void LookForLand()
	{
		if (!invertedGravity)
		{
			if (Physics.Raycast(agent.transform.position, Vector3.down, out hit, rayDistance, groundMask))
			{
				Debug.DrawRay(agent.transform.position, Vector3.down * hit.distance, Color.green);
			}
			else
			{
				Debug.DrawRay(agent.transform.position, Vector3.down * rayDistance, Color.red);

				if (Physics.Raycast(agent.transform.position, (Vector3.down + Vector3.right).normalized, out hit, rayDistance, groundMask))
				{
					Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.right).normalized * hit.distance, Color.green);
					agent.GetComponent<Rigidbody>().AddForce(Vector3.right, ForceMode.Force);
				}
				else
				{
					Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.right).normalized * rayDistance, Color.red);

					if (Physics.Raycast(agent.transform.position, (Vector3.down + Vector3.left).normalized, out hit, rayDistance, groundMask))
					{
						Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.left).normalized * hit.distance, Color.green);
						agent.GetComponent<Rigidbody>().AddForce(Vector3.left, ForceMode.Force);
					}
					else
					{
						Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.left).normalized * rayDistance, Color.red);
					}
				}
			}
		}
		else
		{
			if (Physics.Raycast(agent.transform.position, Vector3.up, out hit, rayDistance, ground180Mask))
			{
				Debug.DrawRay(agent.transform.position, Vector3.up * hit.distance, Color.green);
			}
			else
			{
				Debug.DrawRay(agent.transform.position, Vector3.up * rayDistance, Color.red);

				if (Physics.Raycast(agent.transform.position, (Vector3.up + Vector3.right).normalized, out hit, rayDistance, ground180Mask))
				{
					Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.right).normalized * hit.distance, Color.green);
					agent.GetComponent<Rigidbody>().AddForce(Vector3.right, ForceMode.Force);

				}
				else
				{
					Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.right).normalized * rayDistance, Color.red);

					if (Physics.Raycast(agent.transform.position, (Vector3.up + Vector3.left).normalized, out hit, rayDistance, ground180Mask))
					{
						Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.left).normalized * hit.distance, Color.green);
						agent.GetComponent<Rigidbody>().AddForce(Vector3.left, ForceMode.Force);
					}
					else
					{
						Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.left).normalized * rayDistance, Color.red);
					}
				}
			}
		}
	}
	protected void LookForLand(Collider potentialLand)
	{
		if (!invertedGravity && potentialLand.tag != "WalkableObject")
		{
			agent.GetComponent<Rigidbody>().AddForce(-(potentialLand.transform.position - agent.transform.position).normalized * catchUpSpeed, ForceMode.Force);
		}
		else if (invertedGravity && potentialLand.tag != "WalkableObject180")
		{
			agent.GetComponent<Rigidbody>().AddForce(-(potentialLand.transform.position - agent.transform.position).normalized * catchUpSpeed, ForceMode.Force);
		}
	}
	protected void FlyBack(Vector3 flyBackPos)
	{
		if (Vector3.Distance(agent.transform.position, flyBackPos) > followDistance)
		{
			agent.GetComponent<Rigidbody>().AddForce((flyBackPos - agent.transform.position).normalized * flyBackSpeed, ForceMode.Force);
		}
		else
		{
			isFlyBack = false;
		}
	}

	//Player Input
	protected abstract void MasterInput();
	protected void GravityFlip()
	{
		moveOnFixedUpdate = false;
		agent.enabled = false;
		invertedGravity = !invertedGravity;
		agent.GetComponent<Rigidbody>().isKinematic = false;
		agent.GetComponent<Rigidbody>().useGravity = true;
		agent.GetComponent<AgentLinkMover>().invertedJump = !agent.GetComponent<AgentLinkMover>().invertedJump;
		isFalling = true;
	}
	
	//Colliders
	public abstract void HandleProximityTrigger(Collider other);
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
		else
		{
			if(collision.collider.tag == "Player")
			{

			}
		}
	}
}
