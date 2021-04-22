using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class AIState : MonoBehaviour
{
    [SerializeField]
    private float attentionSpan;
    [SerializeField]
    private float idleSpeed, followSpeed;

    private Context context;
    

    void Start()
    {
        //context = new Context(new IdleState(gameObject));
        context = new Context(new IdleState(gameObject, attentionSpan, idleSpeed, followSpeed));
    }

    void Update()
    {
        context.UpdateContext();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    context.HandleCollision(other);
    //}
}