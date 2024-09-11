using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void AnimationTriggerEvent(Enemy.AnimationTriggerType type)
    {
    }

    public virtual void EnterState()
    {
    }

    public virtual void ExitState()
    {
    }

    public virtual void FrameUpdate()
    {
    }

    public virtual void PhysicsUpdate() { }
    public virtual void GetNextState()
    {
    }

    public virtual void OnTriggerEnter(Collider2D collision)
    {
    }

    public virtual void OnTriggerExit(Collider2D collision)
    {
    }

    public virtual void OnTriggerStay(Collider2D collision)
    {
    }

    public virtual void TestingDebug()
    {
        Debug.Log("Unknown state");
    }
}
