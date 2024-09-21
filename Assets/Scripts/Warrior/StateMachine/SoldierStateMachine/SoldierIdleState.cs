using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierIdleState: BaseState<Warrior>
{
    public SoldierIdleState(Soldier obj, StateManager<Warrior> objectStateManager) : base(obj, objectStateManager)
    {
    }
}
