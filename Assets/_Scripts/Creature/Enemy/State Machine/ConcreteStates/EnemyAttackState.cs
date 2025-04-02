using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState<T> : BaseState<T> where T: Enemy
{
    public EnemyAttackState(T enemy, StateManager<T> stateManager) : base(enemy, stateManager)
    {
    }

    public override void EnterState()
    {
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
