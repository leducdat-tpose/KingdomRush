using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorWalkState : BaseState<Warrior>
{
    IMoveable _moveable;
    public WarriorWalkState(Warrior obj, StateManager<Warrior> objectStateManager) : base(obj, objectStateManager)
    {
        _moveable = owner as IMoveable;
    }

    public override void EnterState()
    {
        if(_moveable == null)
        {
            Debug.LogError($"{owner.GetType()} can not moving");
            return;
        }
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        if(_moveable == null) return;
        _moveable.Moving();
    }
}
