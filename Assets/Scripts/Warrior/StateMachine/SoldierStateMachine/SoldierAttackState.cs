using UnityEngine;
public class SoldierAttackState: BaseState<Soldier>
{
    private float _nextAttackTime;
    public SoldierAttackState(Soldier obj, StateManager<Soldier> objectStateManager) : base(obj, objectStateManager)
    {
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if(!Object.GetCurrentTarget()) Object.StopAttackingState();
        if (Time.time < _nextAttackTime) return;
        Object.StartAttacking();
        _nextAttackTime = Time.time + Object.CoolDownAttack;
    }
    
}