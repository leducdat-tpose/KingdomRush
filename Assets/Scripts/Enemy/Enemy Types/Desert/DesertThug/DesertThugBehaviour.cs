using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertThugBehaviour : EnemyBehaviour<DesertThug>
{
    public override void Start()
    {
        DesertThug obj = this.gameObject.GetComponent<DesertThug>();
        StateManager = new StateManager<DesertThug>();
        IdleState = new ThugIdleState(obj, StateManager);
        WalkState = new ThugWalkState(obj, StateManager);
        AttackState = new ThugAttackState(obj, StateManager);
        DeathState = new ThugDeathState(obj, StateManager);
        StateManager.Initialize(IdleState);
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }
}
