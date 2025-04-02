using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : BaseState<T> where T: Enemy
{
    public EnemyIdleState(T obj, StateManager<T> stateManager) : base(obj, stateManager)
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
