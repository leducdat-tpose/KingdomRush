using UnityEngine;
public class SoldierWalkState: BaseState<Soldier>
{
    public SoldierWalkState(Soldier obj, StateManager<Soldier> objectStateManager, BaseBehaviour<Soldier> behaviour) : base(obj, objectStateManager, behaviour)
    {
    }

    public override void FrameUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
    }

    public override void ExitState()
    {
    }
    public void TestingFunction(){}

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }
}