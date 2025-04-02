using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : BaseState<Enemy>
{
    public EnemyIdleState(Enemy enemy, StateManager<Enemy> enemyStateMachine, BaseBehaviour<Enemy> behaviour) : base(enemy, enemyStateMachine, behaviour)
    {
    }
    public override void EnterState()
    {
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
