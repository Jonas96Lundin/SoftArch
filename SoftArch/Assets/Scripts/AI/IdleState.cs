using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class IdleState : State
{
	public IdleState(GameObject ai, float attentionSpan, float idleSpeed, float followSpeed)
	{
		this.ai = ai;
		this.attentionSpan = attentionSpan;
		this.idleSpeed = idleSpeed;
		this.followSpeed = followSpeed;
		speed = idleSpeed;
	}
	public IdleState(GameObject ai)
    {
        this.ai = ai;
    }

    public override void UpdateState()
    {
		//ai.Move(3);
		//ai.transform.position = new Vector3(ai.transform.position.x + horizontal, transform.position.y + vertical, transform.position.z);
		Debug.Log(attentionSpan);
        //Change when timer
		if(timeToChange <= 0)
		{
			Debug.Log("hallo000000");
			ChangeDirection();
		}
			

		timeToChange -= Time.deltaTime;
        //Move
        Move(speed);

		if (Mathf.Abs(Camera.main.transform.position.x - ai.transform.position.x) > 20)
		{
			//_context.TransitionTo(new FollowState(ai));
			_context.TransitionTo(new FollowState(ai, attentionSpan,idleSpeed, followSpeed));
		}
	}

    public override void ChangeDirection()
    {
		Debug.Log("hgunnar");
		//Random direction
		//ai.transform.forward = new Vector3(0, 0, 0);
		RandomDirection();
		timeToChange = attentionSpan;
        //ai.transform.rotation.Set(0, 90, 0, 0);
	}

	protected void RandomDirection()
	{
		while (true)
		{
			int newDir = Random.Range(0, 3);
			if(newDir == 0)
			{
				TurnForward();
				speed = 0f;

				break;
			}
			else if (newDir == 1)
			{
				TurnRight();
				speed = idleSpeed;
				break;
			}
			else if (newDir == 2)
			{
				TurnLeft();
				speed = idleSpeed;
				break;
			}
		}
	}

	//protected void TurnForward()
	//{
	//	ai.transform.eulerAngles = new Vector3(0f, 0f, 0f);
	//}
	//protected void TurnBack()
	//{
	//	ai.transform.eulerAngles = new Vector3(0f, 180f, 0f);
	//}
	//protected void TurnRight()
	//{
	//	ai.transform.eulerAngles = new Vector3(0f, 90f, 0f);
	//}
	//protected void TurnLeft()
	//{
	//	ai.transform.eulerAngles = new Vector3(0f, -90f, 0f);
	//}

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
