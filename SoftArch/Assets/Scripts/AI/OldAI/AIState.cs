using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>

[RequireComponent(typeof(NavMeshAgent), typeof(MeshRenderer))]
public class AIState : MonoBehaviour
{
    [SerializeField]
    private CharController master;
    [SerializeField]
    private float attentionSpan;
    [SerializeField]
    private float idleSpeed, catchUpSpeed;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private MeshRenderer mesh;


    private OldContext context;
    

    void Start()
    {
        //context = new Context(new IdleState(gameObject));
        context = new OldContext(new OldIdleState(gameObject, master, false, attentionSpan, idleSpeed, catchUpSpeed, agent, mesh));
    }

    void Update()
    {
        context.UpdateContext();
    }

	private void FixedUpdate()
	{
        context.FixedUpdateContext();
	}

	//private void OnDrawGizmos()
	//{
 //       context.OnDrawGizmos();
	//}
	//private void OnTriggerEnter(Collider other)
	//{
	//    context.HandleCollision(other);
	//}

}