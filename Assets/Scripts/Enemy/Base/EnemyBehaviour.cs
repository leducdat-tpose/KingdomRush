using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : BaseBehaviour<Enemy>
{
    private float _nextAttackTime;
    private Warrior _currentTargetWarrior;
    public override void Start()
    {
        base.Start();
        CurrentAnimation = idleAnimation;
        IdleState = new EnemyIdleState(Object, StateManager, this);
        WalkState = new EnemyWalkState(Object, StateManager, this);
        AttackState = new EnemyAttackState(Object, StateManager, this);
        DeathState = new EnemyDeathState(Object, StateManager, this);
        StateManager.Initialise(WalkState);
    }

    public override void Update()
    {
        if(Time.time > _nextAttackTime && _currentTargetWarrior)
        {
            StateManager.ChangeState(AttackState);
            _nextAttackTime = Time.time + Object.CoolDownAttack;
        }
        StateManager.CurrentState.FrameUpdate();
        Render();
    }
    public override void FixedUpdate()
    {
        StateManager.CurrentState.PhysicsUpdate();
    }
    public override void Render()
    {
        int nextAnimation = idleAnimation;
        if(StateManager.CurrentState == AttackState) 
            nextAnimation = attackAnimation;
        else if(StateManager.CurrentState == DeathState)
            nextAnimation = deathAnimation;
        else if(Object.Rigidbody.velocity != Vector2.zero)
        {
            Vector2 direction = Object.Rigidbody.velocity / Object.MoveSpeed;
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
                nextAnimation = direction.y < 0 ? walkDownAnimation : walkUpAnimation;
            else{
                nextAnimation = walkSideAnimation;
                spriteRenderer.flipX = direction.x < 0;
            }
        }
        if(nextAnimation == CurrentAnimation
        || Object.gameObject.activeSelf == false) return;
        animator.CrossFade(nextAnimation, 0.1f, 0);
        CurrentAnimation = nextAnimation;
    }
    public override void CauseDamage()
    {
        base.CauseDamage();
        if(!_currentTargetWarrior) return;
        _currentTargetWarrior.TakenDamage(Object.BaseDamage);
    }
}
