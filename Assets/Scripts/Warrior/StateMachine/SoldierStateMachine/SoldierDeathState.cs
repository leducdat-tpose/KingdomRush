using UnityEngine;
public class SoldierDeathState: BaseState<Warrior>
{
    public SoldierDeathState(Soldier obj, StateManager<Warrior> objectStateManager) : base(obj, objectStateManager)
    {
    }
}