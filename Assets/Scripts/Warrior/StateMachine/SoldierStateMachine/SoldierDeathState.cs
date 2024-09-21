using UnityEngine;
public class SoldierDeathState: BaseState<Soldier>
{
    public SoldierDeathState(Soldier obj, StateManager<Soldier> objectStateManager) : base(obj, objectStateManager)
    {
    }
}