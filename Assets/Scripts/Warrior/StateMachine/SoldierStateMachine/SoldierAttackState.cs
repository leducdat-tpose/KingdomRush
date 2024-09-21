using UnityEngine;
public class SoldierAttackState: BaseState<Warrior>
{
    public SoldierAttackState(Soldier obj, StateManager<Warrior> objectStateManager) : base(obj, objectStateManager)
    {
    }
}