using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class FollowState : State
{
	public FollowState(GameObject ai, float attentionSpan, float idleSpeed, float catchUpSpeed, CharController master, bool followMaster)
	{
		this.ai = ai;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.catchUpSpeed = catchUpSpeed;
		speed = catchUpSpeed;
		this.master = master;
		this.followMaster = followMaster;
		masterPosition = master.transform.position;
		masterVelocityHorizontal = master.GetComponent<Rigidbody>().velocity.x;
		timeToChange = attentionSpan;
	}
	public FollowState(GameObject ai)
	{
		this.ai = ai;
	}
	public override void UpdateState()
	{
		if (MasterInput())
			return;

		//Rigidbody rigidbody = ai.GetComponent<Rigidbody>();

		//velocityHorizontal = master.GetComponent<Rigidbody>().velocity.x;


		if (timeToChange <= 0)
		{
			ChangeDirection();
		}
		timeToChange -= Time.deltaTime;

			//ai.GetComponent<Rigidbody>().velocity.x = master.GetComponent<Rigidbody>().velocity.x;


		//Match player speed???
		//masterSpeed = master.transform.position.x - masterPosition.x;
		//masterPosition = master.transform.position;



		//masterSpeed = 20;
		//Move(masterSpeed);

		//Vector3 deltaMovement = masterPosition - master.transform.position;
		//ai.transform.position += new Vector3(deltaMovement.x * parallaxEffect.x, deltaMovement.y * parallaxEffect.y, 0);
		//lastCameraPos = cameraTransform.position;
	}

	public override void ChangeDirection()
	{
		if(lastHorizontalposPos > master.transform.position.x)
		{
			float distance = (master.transform.position.x - ai.transform.position.x) / 10;
			velocityHorizontal = 10 * distance;
			Debug.Log(velocityHorizontal);
			if (!moveLeftOnFixedUpdate)
			{

			}
		}
		else if (lastHorizontalposPos < master.transform.position.x)
		{

		}
		lastHorizontalposPos = master.transform.position.x;
		//ai.transform.position = Vector3.MoveTowards(ai.transform.position, new Vector3(master.transform.position.x + 4, ai.transform.position.y, ai.transform.position.z), 1);
		//float masterVelocity = ((master.transform.position - masterPosition).magnitude) / Time.deltaTime;
		//masterPosition = master.transform.position;
		if (master.RBLeftMovementActive)
		{
			float distance = (master.transform.position.x - ai.transform.position.x) / 10;
			velocityHorizontal = 10 * distance;
			Debug.Log(velocityHorizontal);
			//ai.transform.position = Vector3.MoveTowards(ai.transform.position, new Vector3(master.transform.position.x + 4.0f, ai.transform.position.y, ai.transform.position.z), 0.01f);
			//if (ai.transform.position.x > master.transform.position.x + 4)
			//{
			//	Debug.Log("Faster");
			//	velocityHorizontal = 5.2f;
			//}
			//else if (ai.transform.position.x < master.transform.position.x + 4)
			//{
			//	Debug.Log("Slower");
			//	velocityHorizontal = 4.5f;
			//}
			if (!moveLeftOnFixedUpdate)
			{
				//int rndSpeed = Random.Range(0, 3);
				//if (rndSpeed == 0)
				//{
				//	velocityHorizontal = 4.8f;
				//}
				//else if (rndSpeed == 1)
				//{
				//	velocityHorizontal = 5.0f;
				//}
				//else if (rndSpeed == 2)
				//{
				//	velocityHorizontal = 5.2f;
				//}
				TurnLeft();
				//velocityHorizontal = 5.0f;
				//timeToChange = attentionSpan / 2;
			}
		}
		else if (master.RBRightMovementActive)
		{

			float distance = (master.transform.position.x - ai.transform.position.x) / 10;
			velocityHorizontal = 10 * distance;
			Debug.Log(velocityHorizontal);

			//if (ai.transform.position.x < master.transform.position.x - 4)
			//{
			//	Debug.Log("Faster");
			//	velocityHorizontal = 5.2f;
			//}
			//else if (ai.transform.position.x > master.transform.position.x - 4)
			//{
			//	Debug.Log("Slower");
			//	velocityHorizontal = 4.8f;
			//}

			//int rndSpeed = Random.Range(0, 3);
			//if (rndSpeed == 0)
			//{
			//	velocityHorizontal = 4.8f;
			//}
			//else if (rndSpeed == 1)
			//{
			//	velocityHorizontal = 5.0f;
			//}
			//else if (rndSpeed == 2)
			//{
			//	velocityHorizontal = 5.2f;
			//}
			if (!moveRightOnFixedUpdate)
			{
				//int rndSpeed = Random.Range(0, 3);
				//if (rndSpeed == 0)
				//{
				//	velocityHorizontal = 4.8f;
				//}
				//else if (rndSpeed == 1)
				//{
				//	velocityHorizontal = 5.0f;
				//}
				//else if (rndSpeed == 2)
				//{
				//	velocityHorizontal = 5.2f;
				//}
				TurnRight();
				//velocityHorizontal = 5.0f;
				////timeToChange = attentionSpan / 2;
			}
			else
			{
				
			}
		}
		else
		{
			Debug.Log("Follow Forward");
			//TurnForward();
		}


		

		//if (Mathf.Abs(master.transform.position.x - ai.transform.position.x) < 4)
		//{
		//	//_context.TransitionTo(new IdleState(ai));
		//	_context.TransitionTo(new IdleState(ai, attentionSpan, idleSpeed, catchUpSpeed, master));
		//}

		//Vector2 direction = ((Vector2)Camera.main.transform.position - (Vector2)ai.transform.position).normalized;
		//ai.transform.eulerAngles = new Vector3(0f, -90f, 0f);
		//ai.transform.eulerAngles = (ai.transform.position - Camera.main.transform.position).normalized * 360f;
	}


}
