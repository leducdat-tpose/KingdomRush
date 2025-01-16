using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangerWarriorBehaviour : BaseBehaviour<Warrior>
{
    #region ID_Animations
    private int _idleAnimation = Animator.StringToHash("Idle Level_1");
    private int _idleUpAnimation = Animator.StringToHash("Idle_Up Level_1");
    // private int _walkUpAnimation = Animator.StringToHash("WalkUp");
    // private int _walkDownAnimation = Animator.StringToHash("WalkDown");
    private int _walkSideAnimation = Animator.StringToHash("Walk Level_1");
    private int _deathAnimation = Animator.StringToHash("Death Level_1");
    private int _attackAnimation = Animator.StringToHash("Attack Level_1");
    private int _attackUpAnimation = Animator.StringToHash("Attack_Up Level_1");
    #endregion
    private float _nextAttackTime;
    private Enemy _currentTargetEnemy;
    [SerializeField]
    private Tower _tower;
    private Vector2 PositionReleaseProjectTile;
    private ProjectTiles _currentProjectTile;
    public override void Start()
    {
        base.Start();
        _nextAttackTime = 0;
        _currentTargetEnemy = null;
        _tower = transform.root.GetComponentInChildren<Tower>();
        // _tower = Object.OwnerTower;
        PositionReleaseProjectTile = transform.position;
        CurrentAnimation = _idleAnimation;
        IdleState = new WarriorIdleState(Object, StateManager);
        WalkState = new WarriorWalkState(Object, StateManager);
        AttackState = new WarriorAttackState(Object, StateManager);
        DeathState = new WarriorDeathState(Object, StateManager);
        StateManager.Initialize(IdleState);
    }

    public override void Render()
    {
        int newAnimation = _idleAnimation;
        if (StateManager.CurrentState != IdleState)
        {
            if (StateManager.CurrentState == AttackState)
                newAnimation = _attackAnimation;
            else if (StateManager.CurrentState == DeathState)
            {
                newAnimation = _deathAnimation;
            }
            else if (StateManager.CurrentState == WalkState)
            {
                newAnimation = _walkSideAnimation;
            }
            if(_tower.CurrentTarget)
            {
                Vector2 direction = _tower.CurrentTarget.transform.position - _tower.transform.position;
                spriteRenderer.flipX = direction.x < -0.8f;
                if(direction.y > 1.0f && StateManager.CurrentState == AttackState) newAnimation = _attackUpAnimation;
            }
        }
        if(CurrentAnimation == newAnimation) return;
        animator.CrossFade(newAnimation, 0.1f, 0);
        CurrentAnimation = newAnimation;
    }

    public override void Update()
    {
        UpdateCurrentTargetEnemy();
        ReadyToAttack();
        StateManager.CurrentState.FrameUpdate();
        Render();
    }

    public override void FixedUpdate()
    {
        StateManager.CurrentState.PhysicsUpdate();
    }

    private void ReadyToAttack()
    {
        if (!_tower.CurrentTarget 
        || _tower.CurrentTarget.GetIsDead() 
        || Time.time < _nextAttackTime
        || StateManager.CurrentState == AttackState) return;
        StateManager.ChangeState(AttackState);
        ReadyProjectile();
        StartAttacking();
        _nextAttackTime = Time.time + Object.CoolDownAttack;
    }

    public void StartAttacking()
    {
        _tower.StartProgress();
        _currentProjectTile.SetCauseDamage(Object.BaseDamage);
    }

    public void ShootProjectTile(){
        _currentProjectTile.gameObject.SetActive(true);
    }
    public void StopAttacking()
    {
        StateManager.ChangeState(IdleState);
    }

    private void ReadyProjectile()
    {
        var newObject = PoolingObject.Instance.GetObject(Object.PrefabProjectTile);
        _currentProjectTile = newObject.GetComponent<ProjectTiles>();
        _currentProjectTile.transform.position = PositionReleaseProjectTile;
        _currentProjectTile.gameObject.SetActive(false);
        _currentProjectTile.SetCurrentEnemy(_currentTargetEnemy);
    }

    public override void CauseDamage()
    {
        base.CauseDamage();
        if(!_currentTargetEnemy) return;
        _currentTargetEnemy.TakenDamage(Object.BaseDamage);
    }
    public void UpdateCurrentTargetEnemy()
    {
        if(_currentTargetEnemy == _tower.CurrentTarget) return;
        _currentTargetEnemy = _tower.CurrentTarget;
    }
}
