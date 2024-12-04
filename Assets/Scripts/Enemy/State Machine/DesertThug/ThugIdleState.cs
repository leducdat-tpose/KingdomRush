using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThugIdleState : BaseState<DesertThug>
{
    public ThugIdleState(DesertThug obj, StateManager<DesertThug> objectStateManager) : base(obj, objectStateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Object.StopMoving();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void GetNextState()
    {
        base.GetNextState();
    }

    public override void OnTriggerEnter(Collider2D collision)
    {
        base.OnTriggerEnter(collision);
    }

    public override void OnTriggerExit(Collider2D collision)
    {
        base.OnTriggerExit(collision);
    }

    public override void OnTriggerStay(Collider2D collision)
    {
        base.OnTriggerStay(collision);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
