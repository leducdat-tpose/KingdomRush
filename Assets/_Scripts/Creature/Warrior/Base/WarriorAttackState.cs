using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttackState : BaseState<Warrior>
{
    public WarriorAttackState(Warrior obj, StateManager<Warrior> objectStateManager) : base(obj, objectStateManager)
    {
    }

    public override void EnterState()
    {
        owner.StartAttacking();
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
