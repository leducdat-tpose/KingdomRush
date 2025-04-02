using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttackState<T> : BaseState<T> where T: Warrior
{
    public WarriorAttackState(T obj, StateManager<T> objectStateManager) : base(obj, objectStateManager)
    {
    }

    public override void EnterState()
    {
        // behaviour.ReadyProjectile();
        // behaviour.StartAttacking();
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
