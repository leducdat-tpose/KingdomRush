using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierIdleState: BaseState<Soldier>
{
    public SoldierIdleState(Soldier obj, StateManager<Soldier> objectStateManager) : base(obj, objectStateManager)
    {
    }
}
