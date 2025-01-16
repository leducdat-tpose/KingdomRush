using UnityEngine;
public class SoldierWalkState: BaseState<Soldier>
{
    public SoldierWalkState(Soldier obj, StateManager<Soldier> objectStateManager) : base(obj, objectStateManager)
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