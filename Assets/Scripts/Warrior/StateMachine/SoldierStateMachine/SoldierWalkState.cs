using UnityEngine;
public class SoldierWalkState: BaseState<Warrior>
{
    public SoldierWalkState(Soldier obj, StateManager<Warrior> objectStateManager) : base(obj, objectStateManager)
    {
    }
}