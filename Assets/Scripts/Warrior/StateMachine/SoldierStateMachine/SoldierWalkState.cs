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
        Object.MovingToStandingPosition();
        Object.MovingToEnemyPosition();
    }

    public override void ExitState()
    {
        Object.StopMoving();
    }
    public void TestingFunction(){}
}