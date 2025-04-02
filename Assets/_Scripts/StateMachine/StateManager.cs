using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager<T>
{
    public BaseState<T> CurrentState{get; private set;}
    public void Initialise(BaseState<T> startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public void ChangeState(BaseState<T> newState)
    {
        if(CurrentState == newState) return;
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
