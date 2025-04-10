using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Warrior : BaseBehaviour
{
    [SerializeField]
    protected WarriorData data;
    protected float currentHealth;
    public float CurrentHealth => currentHealth;
    protected Enemy currentTargetEnemy;
    protected StateManager<Warrior> stateManager;
    protected WarriorIdleState idleState;
    protected WarriorAttackState attackState;
    protected WarriorDeathState deathState;
    protected WarriorWalkState walkState;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        currentTargetEnemy = null;
        LoadStateManager();
    }
    public override void Initialise()
    {
        base.Initialise();
        currentHealth = data.MaxHealth;
    }
    protected virtual void LoadStateManager(){
        stateManager = new StateManager<Warrior>();
        idleState = new WarriorIdleState(this, stateManager);
        walkState = new WarriorWalkState(this, stateManager);
        attackState = new WarriorAttackState(this, stateManager);
        deathState = new WarriorDeathState(this, stateManager);
        stateManager.AddState(idleState);
        stateManager.AddState(walkState);
        stateManager.AddState(attackState);
        stateManager.AddState(deathState);
        stateManager.Initialise(idleState);
    }

    public override void Render()
    {
    }

    public override void Update()
    {
        stateManager.Update();
        Render();
    }
    public override void FixedUpdate()
    {
        stateManager.FixedUpdate();
    }

    public override void CauseDamage()
    {
        base.CauseDamage();
        if(!currentTargetEnemy) return;
        currentTargetEnemy.TakenDamage(data.Damage);
    }
}
