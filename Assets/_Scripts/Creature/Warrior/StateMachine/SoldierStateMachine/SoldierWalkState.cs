using UnityEngine;
public class SoldierWalkState: BaseState<Soldier>
{
    private Vector3 _targetPosition = Vector3.zero;
    public SoldierWalkState(Soldier obj, StateManager<Soldier> objectStateManager, BaseBehaviour<Soldier> behaviour) : base(obj, objectStateManager, behaviour)
    {
    }

    public override void FrameUpdate()
    {
        UpdateTargetPosition();
    }

    public override void PhysicsUpdate()
    {
        Moving();
    }

    public override void ExitState()
    {
        StopMoving();
    }

    public override void EnterState()
    {
    }
    private void UpdateTargetPosition()
    {
        _targetPosition = Object.RallyPosition;
        if(Vector2.Distance(Object.RallyPosition, Object.transform.position) > 0.1f || !Object.gameObject.activeSelf) return;
        ObjectStateManager.ChangeState(Behaviour.IdleState);
    }
    private void Moving()
    {
        if(ObjectStateManager.CurrentState != this) return;
        Vector2 direction = (_targetPosition - Object.transform.position).normalized;
        Object.Rigidbody.velocity = direction * Object.MoveSpeed;
    }
    private void StopMoving() => Object.Rigidbody.velocity = Vector2.zero;
}