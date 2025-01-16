using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : BaseState<Enemy>
{
    public EnemyWalkState(Enemy enemy, StateManager<Enemy> enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }
    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        StopMoving();
    }

    public override void FrameUpdate()
    {
        UpdateTargetPosition();
    }

    public override void PhysicsUpdate()
    {
        Moving();
    }
    private void Moving()
    {
        if(ObjectStateManager.CurrentState != this) return;
        Vector2 direction = (Object.TargetPosition - Object.transform.position).normalized;
        Object.Rigidbody.velocity = direction * Object.MoveSpeed;
    }

    private void UpdateTargetPosition()
    {
        if(Vector2.Distance(Object.TargetPosition, Object.transform.position) > 0.1f || !Object.gameObject.activeSelf) return;
        Object.PathIndex++;
        if(Object.PathIndex == GameController.Instance.LevelManager.GetWaypointsLength())
        {
            Object.ReturnPoolObject();
            return;
        }
        Object.TargetPosition = GameController.Instance.LevelManager.GetPoint(Object.PathIndex);
    }

    private void StopMoving()
    {
        Object.Rigidbody.velocity = Vector2.zero;
    }
}
