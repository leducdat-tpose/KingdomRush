using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : BaseBehaviour<Enemy>
{
    #region ID_Animations
    private readonly int _idleAnimation = Animator.StringToHash("Idle");
    private readonly int _walkUpAnimation = Animator.StringToHash("WalkUp");
    private readonly int _walkDownAnimation = Animator.StringToHash("WalkDown");
    private readonly int _walkSideAnimation = Animator.StringToHash("WalkSide");
    private readonly int _deathAnimation = Animator.StringToHash("Death");
    private readonly int _attackAnimation = Animator.StringToHash("Attack");
    #endregion
    private float _nextAttackTime;
    private Warrior _currentTargetWarrior;
    public override void Start()
    {
        base.Start();
        CurrentAnimation = _idleAnimation;
        IdleState = new EnemyIdleState(Object, StateManager);
        WalkState = new EnemyWalkState(Object, StateManager);
        AttackState = new EnemyAttackState(Object, StateManager);
        DeathState = new EnemyDeathState(Object, StateManager);
        StateManager.Initialize(WalkState);
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
        int nextAnimation = _idleAnimation;
        if(StateManager.CurrentState == AttackState) 
            nextAnimation = _attackAnimation;
        else if(StateManager.CurrentState == DeathState)
            nextAnimation = _deathAnimation;
        else if(Object.Rigidbody.velocity != Vector2.zero)
        {
            Vector2 direction = Object.Rigidbody.velocity / Object.MoveSpeed;
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
                nextAnimation = direction.y < 0 ? _walkDownAnimation : _walkUpAnimation;
            else{
                nextAnimation = _walkSideAnimation;
                spriteRenderer.flipX = direction.x < 0;
            }
            if(nextAnimation == CurrentAnimation
            || Object.gameObject.activeSelf == false) return;
            animator.CrossFade(nextAnimation, 0.1f, 0);
            CurrentAnimation = nextAnimation;
        }
    }
    public override void CauseDamage()
    {
        base.CauseDamage();
        if(!_currentTargetWarrior) return;
        _currentTargetWarrior.TakenDamage(Object.BaseDamage);
    }
}
