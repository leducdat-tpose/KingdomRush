using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierIdleState: BaseState<Soldier>
{
    public SoldierIdleState(Soldier obj, StateManager<Soldier> objectStateManager, BaseBehaviour<Soldier> behaviour) : base(obj, objectStateManager, behaviour)
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
