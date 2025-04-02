using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum StateType{
    None,
    Idle,
    Attack,
    Walk,
    Death
}
[RequireComponent(typeof(Animator),typeof(SpriteRenderer))]
public abstract class BaseBehaviour : DetMonobehaviour
{
    #region Animator
    protected Animator animator;
    protected readonly int idleAnimation = Animator.StringToHash("Idle");
    protected readonly int idleUpAnimation = Animator.StringToHash("IdleUp");
    protected readonly int walkSideAnimation = Animator.StringToHash("WalkSide");
    protected readonly int walkUpAnimation = Animator.StringToHash("WalkUp");
    protected readonly int walkDownAnimation = Animator.StringToHash("WalkDown");
    protected readonly int deathAnimation = Animator.StringToHash("Death");
    protected readonly int attackAnimation = Animator.StringToHash("Attack");
    protected readonly int attackUpAnimation = Animator.StringToHash("AttackUp");
    public int CurrentAnimation{get; protected set;}
    #endregion
    protected SpriteRenderer spriteRenderer;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        animator = this.gameObject.GetComponent<Animator>();
    }
    public override void Initialise()
    {
        base.Initialise();
    }
    public abstract void Update();

    public abstract void FixedUpdate();
    public abstract void Render();

    public virtual void ChangeState(StateType type){}

    public virtual void CauseDamage(){}

    public virtual void ReadyToAttack(){}
    public virtual void ReadyProjectile(){}
    public virtual void StartAttacking(){}
    public virtual void StopAttacking(){}
    public virtual bool GetIsDead() => false;
}
