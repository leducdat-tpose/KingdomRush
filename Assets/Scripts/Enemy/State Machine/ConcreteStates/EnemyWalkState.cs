using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyState
{
    public EnemyWalkState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType type)
    {
        base.AnimationTriggerEvent(type);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("EnterState Walk");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("ExitState Walk");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.UpdateTargetPosition();
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
        enemy.MoveEnemy();
    }
    public override void TestingDebug()
    {
        Debug.Log("Move State");
    }
}
