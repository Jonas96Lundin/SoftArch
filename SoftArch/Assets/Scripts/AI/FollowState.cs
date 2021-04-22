using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class FollowState : State
{
	public FollowState(GameObject ai, float attentionSpan, float idleSpeed ,float followSpeed)
	{
		this.ai = ai;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.followSpeed = followSpeed;
		speed = followSpeed;

	}
	public FollowState(GameObject ai)
	{
		this.ai = ai;
		speed = 10;
	}
	public override void UpdateState()
	{


		ChangeDirection();
		//Move
		Move(speed);
	}

	public override void ChangeDirection()
	{
		Debug.Log("hej");
		if (moveRight)
		{
			if (ai.transform.position.x > Camera.main.transform.position.x)
			{
				Debug.Log("Turn Left");
				TurnLeft();
			}
		}
		else if (moveLeft)
		{
			if (ai.transform.position.x < Camera.main.transform.position.x)
			{
				Debug.Log("Turn Right");
				TurnRight();
			}
		}
		else
		{
			if (ai.transform.position.x > Camera.main.transform.position.x)
			{
				Debug.Log("Turn Left");
				TurnLeft();
			}
			else if (ai.transform.position.x < Camera.main.transform.position.x)
			{
				Debug.Log("Turn Right");
				TurnRight();
			}
			else
			{
				Debug.Log("Turn Back");
				//TurnBack();
			}
		}

		if (Mathf.Abs(Camera.main.transform.position.x - ai.transform.position.x) < 2)
		{
			//_context.TransitionTo(new IdleState(ai));
			_context.TransitionTo(new IdleState(ai, attentionSpan, idleSpeed, followSpeed));
		}

		//Vector2 direction = ((Vector2)Camera.main.transform.position - (Vector2)ai.transform.position).normalized;
		//ai.transform.eulerAngles = new Vector3(0f, -90f, 0f);
		//ai.transform.eulerAngles = (ai.transform.position - Camera.main.transform.position).normalized * 360f;
	}

	//protected void RandomDirection()
	//{
	//	while (true)
	//	{
	//		int newDir = Random.Range(0, 3);
	//		if (newDir == 0)
	//		{
	//			TurnForward();
	//			speed = 0f;
	//			break;
	//		}
	//		else if (newDir == 1)
	//		{
	//			TurnRight();
	//			speed = 10f;
	//			break;
	//		}
	//		else if (newDir == 2)
	//		{
	//			TurnLeft();
	//			speed = 10f;
	//			break;
	//		}
	//	}
	//}
}
