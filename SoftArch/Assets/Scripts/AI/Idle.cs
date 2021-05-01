using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateController
{
	public Idle(GameObject ai, float attentionSpan, float idleSpeed, float catchUpSpeed, CharController master, bool followMaster)
	{
		this.ai = ai;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		//speed = catchUpSpeed;
		this.master = master;
		this.followMaster = followMaster;
		//velocityHorizontal = idleSpeed;
	}
	public Idle(GameObject ai)
	{
		this.ai = ai;
	}

	public override void UpdateState()
	{
		//if (MasterInput())
		//	return;
		//Rigidbody rigidbody = ai.GetComponent<Rigidbody>();
		////Jump
		//if (!jumpOnFixedUpdate)
		//{
		//	Physics.Raycast(ai.transform.position, Vector2.down, out RaycastHit info, groundDetectRayLength); // Raycast down
		//	bool isOnGround = info.collider != null;
		//	if (isOnGround) // Jump restrictions
		//	{
		//		//jumpOnFixedUpdate = Input.GetButtonDown("Jump"); // Jump on user input
		//	}
		//}
		//// Movement
		//float movement = Input.GetAxis("Horizontal");
		//if (!moveRightOnFixedUpdate && movement > 0)
		//{
		//	moveRightOnFixedUpdate = true;
		//}
		//else if (!moveLeftOnFixedUpdate && movement < 0)
		//{
		//	moveLeftOnFixedUpdate = true;
		//}
		//Brake
		//if (!brakeIsAllowed) // If not alowed to brake
		//{
		//	if (enableBreakWhenVelocityIsZero && rigidbody.velocity.x == 0) // If to turn on brake when character stops
		//	{
		//		enableBreakWhenVelocityIsZero = false;
		//		brakeIsAllowed = true;
		//	}
		//}

		//Change when timer
		if (timeToChange <= 0)
		{
			ChangeDirection();
		}
		timeToChange -= Time.deltaTime;

		//Move(speed);


		//if (Mathf.Abs(Camera.main.transform.position.x - ai.transform.position.x) > 20)
		//{
		//	//_context.TransitionTo(new FollowState(ai));
		//	_context.TransitionTo(new CatchUpState(ai, attentionSpan, idleSpeed, catchUpSpeed, master, false));
		//}
	}

	public override void ChangeDirection()
	{
		RandomDirection();
		timeToChange = attentionSpan;
	}

	protected void RandomDirection()
	{
		while (true)
		{
			int newDir = Random.Range(0, 3);
			if (newDir == 0)
			{
				TurnForward();
				//moveRightOnFixedUpdate = false;
				//moveLeftOnFixedUpdate = false;
				//brakeIsAllowed = true;
				velocityHorizontal = 0f;
				break;
			}
			else if (newDir == 1)
			{
				TurnRight();
				//moveRightOnFixedUpdate = true;
				//moveLeftOnFixedUpdate = false;
				//brakeIsAllowed = false;
				velocityHorizontal = idleSpeed;
				break;
			}
			else if (newDir == 2)
			{
				TurnLeft();
				//moveRightOnFixedUpdate = false;
				//moveLeftOnFixedUpdate = true;
				//brakeIsAllowed = false;
				velocityHorizontal = idleSpeed;
				break;
			}
		}
	}

	//public override void HandleCollision(Collider other)
	//{
	//    if (nextPos == Vector3.zero && other.gameObject.tag == "CrossRoad")
	//    {
	//        //RandomDirection(other.gameObject.GetComponent<TurnPoint>().canTurnForward, other.gameObject.GetComponent<TurnPoint>().canTurnBack,
	//        //    other.gameObject.GetComponent<TurnPoint>().canTurnRight, other.gameObject.GetComponent<TurnPoint>().canTurnLeft);

	//        RandomDirection(other.gameObject.GetComponent<CrossRoad>().canTurnForward, other.gameObject.GetComponent<CrossRoad>().canTurnBack,
	//            other.gameObject.GetComponent<CrossRoad>().canTurnRight, other.gameObject.GetComponent<CrossRoad>().canTurnLeft);

	//        if (Physics.Raycast(ai.transform.position, ai.transform.TransformDirection(Vector3.forward), out hit, 100, turnPointMask))
	//        {
	//            nextPos = hit.collider.transform.position;
	//        }
	//        //Enter Patrol
	//        ai.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f);
	//        _context.TransitionTo(new PatrolState(ai));
	//    }
	//}
}









//OLD!!!!!!!!!!!!!!!!!!!!!!!!!!
//	public IdleState(GameObject ai, float attentionSpan, float idleSpeed, float catchUpSpeed, CharController master, bool followMaster)
//	{
//		this.ai = ai;
//		this.attentionSpan = attentionSpan;
//		this.idleSpeed = idleSpeed;
//		this.catchUpSpeed = catchUpSpeed;
//		speed = catchUpSpeed;
//		this.master = master;
//		this.followMaster = followMaster;
//	}
//	public IdleState(GameObject ai)
//    {
//        this.ai = ai;
//    }

//    public override void UpdateState()
//    {
//		if (MasterInput())
//			return;

//        //Change when timer
//		if(timeToChange <= 0)
//		{
//			ChangeDirection();
//		}
//		timeToChange -= Time.deltaTime;

//        Move(speed);


//		if (Mathf.Abs(Camera.main.transform.position.x - ai.transform.position.x) > 20)
//		{
//			//_context.TransitionTo(new FollowState(ai));
//			_context.TransitionTo(new CatchUpState(ai, attentionSpan,idleSpeed, catchUpSpeed, master, false));
//		}
//	}

//    public override void ChangeDirection()
//    {
//		RandomDirection();
//		timeToChange = attentionSpan;
//	}

//	protected void RandomDirection()
//	{
//		while (true)
//		{
//			int newDir = Random.Range(0, 3);
//			if(newDir == 0)
//			{
//				TurnForward();
//				speed = 0f;
//				break;
//			}
//			else if (newDir == 1)
//			{
//				TurnRight();
//				speed = idleSpeed;
//				break;
//			}
//			else if (newDir == 2)
//			{
//				TurnLeft();
//				speed = idleSpeed;
//				break;
//			}
//		}
//	}

//	//public override void HandleCollision(Collider other)
//	//{
//	//    if (nextPos == Vector3.zero && other.gameObject.tag == "CrossRoad")
//	//    {
//	//        //RandomDirection(other.gameObject.GetComponent<TurnPoint>().canTurnForward, other.gameObject.GetComponent<TurnPoint>().canTurnBack,
//	//        //    other.gameObject.GetComponent<TurnPoint>().canTurnRight, other.gameObject.GetComponent<TurnPoint>().canTurnLeft);

//	//        RandomDirection(other.gameObject.GetComponent<CrossRoad>().canTurnForward, other.gameObject.GetComponent<CrossRoad>().canTurnBack,
//	//            other.gameObject.GetComponent<CrossRoad>().canTurnRight, other.gameObject.GetComponent<CrossRoad>().canTurnLeft);

//	//        if (Physics.Raycast(ai.transform.position, ai.transform.TransformDirection(Vector3.forward), out hit, 100, turnPointMask))
//	//        {
//	//            nextPos = hit.collider.transform.position;
//	//        }
//	//        //Enter Patrol
//	//        ai.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f);
//	//        _context.TransitionTo(new PatrolState(ai));
//	//    }
//	//}
