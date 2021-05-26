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
	protected static Vector3 targetPos, objectPos, holdPos;
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
						  avoidOffset = 2.0f,
						  rayDistance = 2.0f,
						  longRayDistance = 4.0f,
						  fallRayDistance = 20.0f,
						  attentionSpan = 1.0f,
						  indicationTime = 3.0f;
	//Static variables
	protected static float timeToChange = 1.0f,
						   spotlightAngleChange = 2.8f,
						   distanceToMaster;
	protected static bool followMaster,
						  isFalling,
						  isHolding,
						  isFlyBack,
						  invertedGravity,
						  objectFound,
						  jumpOnFixedUpdate = false,
						  moveOnFixedUpdate = false;

	//RayCast
	protected const int playerMask = 1 << 6;
	protected const int groundMask = 1 << 10;
	protected const int ground180Mask = 1 << 11;
	protected const int obstacle = 1 << 12;
	protected static int InterestingObjects = 1 << 13;




	protected static int comboMask = groundMask | playerMask;


	protected float layerMaskHitDistance;
	protected RaycastHit hit;

	public void SetContext(Context context)
	{
		_context = context;
	}

	//Update
	public abstract void UpdateState();
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
	protected bool LookForPlayerAround()
	{
		//Forward
		if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.forward), out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * rayDistance, Color.red);
			return true;
		}
		//Forward/Right
		else if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized, out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * hit.distance, Color.green);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * rayDistance, Color.red);
			return true;
		}
		//Forwerd/Left
		else if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized, out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * hit.distance, Color.green);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * rayDistance, Color.red);
			return true;
		}
		//Right
		else if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.right), out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * hit.distance, Color.green);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * rayDistance, Color.red);
			return true;
		}
		//Left
		else if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.left), out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * hit.distance, Color.green);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * rayDistance, Color.red);
			return true;
		}
		//Back/Right
		else if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized, out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * hit.distance, Color.green);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * rayDistance, Color.red);
			return true;
		}
		//Back/Left
		else if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized, out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * hit.distance, Color.green);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * rayDistance, Color.red);
			return true;
		}
				//Back
		else if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.back), out hit, rayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.right) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.left) * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.back) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.back) * hit.distance, Color.green);
			return true;
		}
		return false;
	}
	protected bool LookForPlayerAhead()
	{
		//Forward
		if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.forward), out hit, longRayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			return true;
		}
		//Forward/Right
		else if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized, out hit, longRayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * longRayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * hit.distance, Color.green);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * rayDistance, Color.red);
			return true;
		}
		//Forwerd/Left
		else if (Physics.Raycast(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized, out hit, longRayDistance, playerMask))
		{
			Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * longRayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.right)).normalized * rayDistance, Color.red);
			Debug.DrawRay(agent.transform.position, (agent.transform.TransformDirection(Vector3.forward) + agent.transform.TransformDirection(Vector3.left)).normalized * hit.distance, Color.green);
			return true;
		}
		return false;
	}
	protected void AvoidPlayer()
	{
		float distance = agent.transform.position.x - master.transform.position.x;
		if (distance > 0)
		{
			targetPos = new Vector3(master.transform.position.x + avoidOffset, agent.transform.position.y, master.transform.position.z - avoidOffset);
			//agent.velocity += (Vector3.right + Vector3.back).normalized * 0.3f;
		}
		else
		{
			targetPos = new Vector3(master.transform.position.x - avoidOffset, agent.transform.position.y, master.transform.position.z - avoidOffset);
			//agent.velocity += (Vector3.left + Vector3.back).normalized * 0.3f;
		}
		agent.stoppingDistance = 1.0f;
		moveOnFixedUpdate = true;
	}

	protected void SetHoldPosition()
	{
		moveToIndicator.spotAngle = 1.0f;

		if (master.GetComponent<RotationManager>().rotateLeft)
			holdPos = new Vector3(master.transform.position.x - avoidOffset, master.transform.position.y, master.transform.position.z);
		else
			holdPos = new Vector3(master.transform.position.x + avoidOffset, master.transform.position.y, master.transform.position.z);

		targetPos = holdPos;
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
			if (Physics.Raycast(agent.transform.position, Vector3.down, out hit, fallRayDistance, groundMask))
			{
				Debug.DrawRay(agent.transform.position, Vector3.down * hit.distance, Color.green);
				Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.right).normalized * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.left).normalized * fallRayDistance, Color.red);
			}
			else if (Physics.Raycast(agent.transform.position, (Vector3.down + Vector3.right).normalized, out hit, fallRayDistance, groundMask))
			{
				Debug.DrawRay(agent.transform.position, Vector3.down * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.right).normalized * hit.distance, Color.green);
				Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.left).normalized * fallRayDistance, Color.red);

				agent.GetComponent<Rigidbody>().AddForce(Vector3.right, ForceMode.Force);
			}
			else if (Physics.Raycast(agent.transform.position, (Vector3.down + Vector3.left).normalized, out hit, fallRayDistance, groundMask))
			{
				Debug.DrawRay(agent.transform.position, Vector3.down * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.right).normalized * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.left).normalized * hit.distance, Color.green);

				agent.GetComponent<Rigidbody>().AddForce(Vector3.left, ForceMode.Force);
			}
			else
			{
				Debug.DrawRay(agent.transform.position, Vector3.down * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.right).normalized * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.down + Vector3.left).normalized * fallRayDistance, Color.red);
			}
		}
		else
		{
			if (Physics.Raycast(agent.transform.position, Vector3.up, out hit, fallRayDistance, groundMask))
			{
				Debug.DrawRay(agent.transform.position, Vector3.up * hit.distance, Color.green);
				Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.right).normalized * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.left).normalized * fallRayDistance, Color.red);
			}
			else if (Physics.Raycast(agent.transform.position, (Vector3.up + Vector3.right).normalized, out hit, fallRayDistance, groundMask))
			{
				Debug.DrawRay(agent.transform.position, Vector3.up * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.right).normalized * hit.distance, Color.green);
				Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.left).normalized * fallRayDistance, Color.red);

				agent.GetComponent<Rigidbody>().AddForce(Vector3.right * followSpeed, ForceMode.Force);
			}
			else if (Physics.Raycast(agent.transform.position, (Vector3.up + Vector3.left).normalized, out hit, fallRayDistance, groundMask))
			{
				Debug.DrawRay(agent.transform.position, Vector3.up * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.right).normalized * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.left).normalized * hit.distance, Color.green);

				agent.GetComponent<Rigidbody>().AddForce(Vector3.left * followSpeed, ForceMode.Force);
			}
			else
			{
				Debug.DrawRay(agent.transform.position, Vector3.up * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.right).normalized * fallRayDistance, Color.red);
				Debug.DrawRay(agent.transform.position, (Vector3.up + Vector3.left).normalized * fallRayDistance, Color.red);
			}
		}
	}
	//protected void LookForLand(Collider potentialLand)
	//{
	//	if (!invertedGravity && potentialLand.tag != "WalkableObject")
	//	{
	//		agent.GetComponent<Rigidbody>().AddForce(-(potentialLand.transform.position - agent.transform.position).normalized * catchUpSpeed, ForceMode.Force);
	//	}
	//	else if (invertedGravity && potentialLand.tag != "WalkableObject180")
	//	{
	//		agent.GetComponent<Rigidbody>().AddForce(-(potentialLand.transform.position - agent.transform.position).normalized * catchUpSpeed, ForceMode.Force);
	//	}
	//}
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
	protected bool GravityFlip()
	{
		if (Input.GetButtonDown("Fire2"))
		{
			moveOnFixedUpdate = false;
			agent.enabled = false;
			invertedGravity = !invertedGravity;
			agent.GetComponent<Rigidbody>().isKinematic = false;
			agent.GetComponent<Rigidbody>().useGravity = true;
			//agent.GetComponent<AgentLinkMover>().invertedJump = !agent.GetComponent<AgentLinkMover>().invertedJump;
			isFalling = true;
		}
		return isFalling;
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
				agent.GetComponent<AgentLinkMover>().invertedJump = false;
				agent.enabled = true;
				isFalling = false;
				isFlyBack = false;
			}
			else if (invertedGravity && collision.collider.CompareTag("WalkableObject180"))
			{
				agent.GetComponent<Rigidbody>().isKinematic = true;
				agent.GetComponent<Rigidbody>().useGravity = true;
				agent.GetComponent<AgentLinkMover>().invertedJump = true;
				agent.enabled = true;
				isFalling = false;
				isFlyBack = false;
			}
		}
		else
		{
			if (collision.collider.tag == "Player")
			{

			}
		}
	}
}
