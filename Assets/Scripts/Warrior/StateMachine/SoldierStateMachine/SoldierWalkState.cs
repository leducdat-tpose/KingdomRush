using UnityEngine;
public class SoldierWalkState: BaseState<Soldier>
{
    private bool _isOrder = false;
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
    }
    private void Moving()
    {
        if(ObjectStateManager.CurrentState != this) return;
        if(!_isOrder){
            _targetPosition = Object.Behaviour.CurrentTargetEnemy.transform.position;
        }
        Vector2 direction = (_targetPosition - Object.transform.position).normalized;
        Object.Rigidbody.velocity = direction * Object.MoveSpeed;
    }
    private void StopMoving() => Object.Rigidbody.velocity = Vector2.zero;
    public void OrderMoving(Vector3 position)
    {
        _isOrder = true;
        _targetPosition = position;
    }
}