using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorIdleState<T> : BaseState<T> where T: Warrior
{
    public WarriorIdleState(T obj, StateManager<T> objectStateManager) : base(obj, objectStateManager)
    {
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }
}