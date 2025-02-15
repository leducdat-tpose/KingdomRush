using UnityEngine;
public class SoldierAttackState: BaseState<Soldier>
{
    private float _nextAttackTime;
    public SoldierAttackState(Soldier obj, StateManager<Soldier> objectStateManager, BaseBehaviour<Soldier> behaviour) : base(obj, objectStateManager, behaviour)
    {
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void FrameUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
        throw new System.NotImplementedException();
    }
}