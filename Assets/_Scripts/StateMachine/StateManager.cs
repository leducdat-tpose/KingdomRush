using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager<T>
{
    public BaseState<T> CurrentState{get; private set;}
    private Dictionary<System.Type, BaseState<T>> _states = new Dictionary<System.Type, BaseState<T>>();
    
    public void AddState(BaseState<T> state)
    {
        _states[state.GetType()] = state;
    }
    
    public void Initialise(BaseState<T> startingState)
    {
        if(!_states.ContainsValue(startingState))
        {
            Debug.Log($"Not contain {startingState} in dict");
            return;
        }
        CurrentState = startingState;
        CurrentState.EnterState();
    }
    public void Initialise<TState>() where TState: BaseState<T>
    {
        CurrentState = _states[typeof(TState)];
        CurrentState.EnterState();
    }

    public void ChangeState(BaseState<T> newState)
    {
        if(!_states.ContainsValue(newState)) return;
        if(CurrentState == newState) return;
        CurrentState?.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }

    public void ChangeState<TState>() where TState: BaseState<T>
    {
        if(CurrentState == _states[typeof(TState)]) return;
        CurrentState?.ExitState();
        CurrentState = _states[typeof(TState)];
        CurrentState.EnterState();
    }
    public void Update()
    {
        CurrentState?.Update();
    }
    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}
