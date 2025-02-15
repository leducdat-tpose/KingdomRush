using UnityEngine;
public class SoldierDeathState: BaseState<Soldier>
{
    public SoldierDeathState(Soldier obj, StateManager<Soldier> objectStateManager, BaseBehaviour<Soldier> behaviour) : base(obj, objectStateManager, behaviour)
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
        throw new System.NotImplementedException();
    }

    public override void PhysicsUpdate()
    {
        throw new System.NotImplementedException();
    }
}