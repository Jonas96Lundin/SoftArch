using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class Context
{
    private State _state = null;

    public Context(State state)
    {
        TransitionTo(state);
    }

    public void TransitionTo(State state)
    {
        _state = state;
        _state.SetContext(this);
    }


    public void UpdateContext()
    {
        _state.UpdateState();
    }
    public void FixedUpdateContext()
	{
        _state.FixedUpdateState();

    }


    public void HandleProximityTrigger(Collider other)
    {
        _state.HandleProximityTrigger(other);
    }
    public void HandleCollision(Collision collision)
	{
        _state.HandleCollision(collision);
    }

}
