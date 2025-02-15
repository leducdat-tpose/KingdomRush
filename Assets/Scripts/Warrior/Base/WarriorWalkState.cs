using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorWalkState : BaseState<Warrior>
{
    public WarriorWalkState(Warrior obj, StateManager<Warrior> objectStateManager, BaseBehaviour<Warrior> behaviour) : base(obj, objectStateManager, behaviour)
    {
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        StopMoving();
    }

    public override void FrameUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
        Moving();
    }
    private void StopMoving()
    {

    }
    private void Moving()
    {

    }
}
