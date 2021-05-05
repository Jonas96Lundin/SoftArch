using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class OldContext
{

    //private State _state = null;
    private OldState _state = null;

    public OldContext(OldState state)
    {
        TransitionTo(state);
    }

    public void TransitionTo(OldState state)
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

    //?
    public void OnDrawGizmos()
    {
        _state.OnDrawGizmos();

    }
}
