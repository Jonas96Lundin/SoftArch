using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class AI : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private CharController master;
    [SerializeField]
    private Light moveToIndicator;

    private Context context;


    void Start()
    {
        context = new Context(new IdleState(agent, master, moveToIndicator/*, attentionSpan, idleSpeed, catchUpSpeed*/));
    }


    void Update()
    {
        context.UpdateContext();
    }
    private void FixedUpdate()
    {
        context.FixedUpdateContext();
    }
	

	private void OnCollisionEnter(Collision collision)
	{
		context.HandleCollision(collision);
	}

	private void OnTriggerEnter(Collider other)
	{
        context.HandleProximityTrigger(other);
    }
}
