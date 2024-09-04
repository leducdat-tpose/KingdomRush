using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    protected bool IsTransitionState = false;

    private void Start()
    {
        CurrentState.EnterState();
    }

    private void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();
        if(nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if(!IsTransitionState)
        {
            TransitionToState(nextStateKey);
        }
        
    }

    public void TransitionToState(EState state)
    {
        IsTransitionState = true;
        CurrentState.ExitState();
        CurrentState = States[state];
        CurrentState.EnterState();
        IsTransitionState = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CurrentState.OnTriggerEnter(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CurrentState.OnTriggerStay(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CurrentState.OnTriggerExit(collision);
    }
}
