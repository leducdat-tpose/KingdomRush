using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : BaseState<Enemy>
{
    public EnemyWalkState(Enemy obj, StateManager<Enemy> stateManager) : base(obj, stateManager)
    {
    }
    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        StopMoving();
    }

    public override void Update()
    {
        UpdateTargetPosition();
    }

    public override void FixedUpdate()
    {
        Moving();
    }
    private void Moving()
    {
        if(stateManager.CurrentState != this) return;
        Vector2 direction = (owner.TargetPosition - owner.transform.position).normalized;
        owner.Rigid.velocity = direction * owner.MoveSpeed;
    }

    private void UpdateTargetPosition()
    {
        if(Vector2.Distance(owner.TargetPosition, owner.transform.position) > 0.1f || !owner.gameObject.activeSelf) return;
        owner.PathIndex++;
        if(owner.PathIndex == GameController.Instance.LevelManager.GetWaypointsLength())
        {
            EventBus<GameController>.Raise(GameController.Instance);
            owner.ReturnPoolObject();
            return;
        }
        owner.TargetPosition = GameController.Instance.LevelManager.GetPoint(owner.PathIndex);
    }

    private void StopMoving()
    {
        owner.Rigid.velocity = Vector2.zero;
    }
}
