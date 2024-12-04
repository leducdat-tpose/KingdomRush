using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBehaviour : WarriorBehaviour<Soldier>
{
    public override void Start()
    {
        Soldier obj = this.gameObject.GetComponent<Soldier>();
        StateManager = new StateManager<Soldier>();
        IdleState = new SoldierIdleState(obj, StateManager);
        WalkState = new SoldierWalkState(obj, StateManager);
        AttackState = new SoldierAttackState(obj, StateManager);
        DeathState = new SoldierDeathState(obj, StateManager);
        StateManager.Initialize(IdleState);
    }

    public override void Update()
    {
        StateManager.CurrentState.FrameUpdate();
    }

    public override void FixedUpdate()
    {
        StateManager.CurrentState.PhysicsUpdate();
    }
}
