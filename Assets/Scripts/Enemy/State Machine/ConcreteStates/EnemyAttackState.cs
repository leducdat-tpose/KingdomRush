using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : BaseState<Enemy>
{
    private float _nextAttackTime;
    public EnemyAttackState(Enemy enemy, StateManager<Enemy> enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (Time.time < _nextAttackTime) return;
        Object.StartAttacking();
        _nextAttackTime = Time.time + Object.CoolDownAttack;
    }

    public override void GetNextState()
    {
        base.GetNextState();
    }

    public override void OnTriggerEnter(Collider2D collision)
    {
        base.OnTriggerEnter(collision);
    }

    public override void OnTriggerExit(Collider2D collision)
    {
        base.OnTriggerExit(collision);
    }

    public override void OnTriggerStay(Collider2D collision)
    {
        base.OnTriggerStay(collision);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
