using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorDeathState<T> : BaseState<T> where T: Warrior
{
    public WarriorDeathState(T obj, StateManager<T> objectStateManager) : base(obj, objectStateManager)
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
