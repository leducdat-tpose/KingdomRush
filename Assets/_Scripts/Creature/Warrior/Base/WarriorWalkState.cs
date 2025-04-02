using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorWalkState<T> : BaseState<T> where T: Warrior
{
    public WarriorWalkState(T obj, StateManager<T> objectStateManager) : base(obj, objectStateManager)
    {
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        StopMoving();
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
        Moving();
    }
    private void StopMoving()
    {

    }
    private void Moving()
    {

    }
}
