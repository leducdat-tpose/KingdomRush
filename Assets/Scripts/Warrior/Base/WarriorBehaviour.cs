using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBehaviour : BaseBehaviour<Warrior>
{
    // private float _nextAttackTime;
    private Enemy _currentTargetEnemy;
    public override void Start()
    {
        // _nextAttackTime = 0;
        _currentTargetEnemy = null;
        base.Start();
        IdleState = new WarriorIdleState(Object, StateManager);
        WalkState = new WarriorWalkState(Object, StateManager);
        AttackState = new WarriorAttackState(Object, StateManager);
        DeathState = new WarriorDeathState(Object, StateManager);
        StateManager.Initialize(IdleState);
    }
    public override void Render()
    {
    }

    public override void Update()
    {
        StateManager.CurrentState.FrameUpdate();
        Render();
    }
    public override void FixedUpdate()
    {
        StateManager.CurrentState.PhysicsUpdate();
    }

    public override void CauseDamage()
    {
        base.CauseDamage();
        if(!_currentTargetEnemy) return;
        _currentTargetEnemy.TakenDamage(Object.BaseDamage);
    }

}
