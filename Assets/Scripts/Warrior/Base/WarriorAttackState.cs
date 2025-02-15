using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttackState : BaseState<Warrior>
{
    public WarriorAttackState(Warrior obj, StateManager<Warrior> objectStateManager, BaseBehaviour<Warrior> behaviour) : base(obj, objectStateManager, behaviour)
    {
    }

    public override void EnterState()
    {
        Behaviour.ReadyProjectile();
        Behaviour.StartAttacking();
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
    }
}
