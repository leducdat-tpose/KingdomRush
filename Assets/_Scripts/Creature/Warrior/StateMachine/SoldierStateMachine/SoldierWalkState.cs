using UnityEngine;
public class SoldierWalkState<T> : WarriorIdleState<Soldier>
{
    private Vector3 _targetPosition = Vector3.zero;
    public SoldierWalkState(Soldier obj, StateManager<Soldier> objectStateManager) : base(obj, objectStateManager)
    {
    }

    public override void Update()
    {
        UpdateTargetPosition();
    }

    public override void FixedUpdate()
    {
        Moving();
    }

    public override void ExitState()
    {
        StopMoving();
    }

    public override void EnterState()
    {
        // if(!owner.Data.CanMove) stateManager.ChangeState(behaviour.IdleState);
    }
    private void UpdateTargetPosition()
    {
        // _targetPosition = owner.RallyPosition;
        // if(Vector2.Distance(owner.RallyPosition, owner.transform.position) > 0.1f || !owner.gameObject.activeSelf) return;
        // stateManager.ChangeState(behaviour.IdleState);
    }
    private void Moving()
    {
        if(stateManager.CurrentState != this) return;
        // Vector2 direction = (_targetPosition - owner.transform.position).normalized;
        // owner.Rigidbody.velocity = direction * owner.Data.MoveSpeed;
    }
    private void StopMoving() 
    {
        // owner.Rigidbody.velocity = Vector2.zero;
    }
}