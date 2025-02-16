using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangerWarriorBehaviour : BaseBehaviour<Warrior>
{
    private float _nextAttackTime;
    private Enemy _currentTargetEnemy;
    [SerializeField]
    public Tower MainTower{get; private set;}
    private Vector2 PositionReleaseProjectTile;
    private ProjectTiles _currentProjectTile;
    public override void Start()
    {
        base.Start();
        _nextAttackTime = 0;
        _currentTargetEnemy = null;
        MainTower = transform.root.GetComponentInChildren<Tower>();
        PositionReleaseProjectTile = transform.position;
        CurrentAnimation = idleAnimation;
        IdleState = new WarriorIdleState(Object, StateManager, this);
        WalkState = new WarriorWalkState(Object, StateManager, this);
        AttackState = new WarriorAttackState(Object, StateManager, this);
        DeathState = new WarriorDeathState(Object, StateManager, this);
        StateManager.Initialise(IdleState);
    }

    public override void Render()
    {
        int newAnimation = idleAnimation;
        if (StateManager.CurrentState != IdleState)
        {
            if (StateManager.CurrentState == AttackState)
                newAnimation = attackAnimation;
            else if (StateManager.CurrentState == DeathState)
            {
                newAnimation = deathAnimation;
            }
            else if (StateManager.CurrentState == WalkState)
            {
                newAnimation = walkSideAnimation;
            }
            if(MainTower.CurrentTarget)
            {
                Vector2 direction = MainTower.CurrentTarget.transform.position - MainTower.transform.position;
                spriteRenderer.flipX = direction.x < -0.8f;
                if(direction.y > 1.0f && StateManager.CurrentState == AttackState) newAnimation = attackUpAnimation;
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

    public override void ReadyToAttack()
    {
        if (!MainTower.CurrentTarget 
        || MainTower.CurrentTarget.GetIsDead() 
        || Time.time < _nextAttackTime
        || StateManager.CurrentState == AttackState) return;
        StateManager.ChangeState(AttackState);
        _nextAttackTime = Time.time + Object.CoolDownAttack;
    }

    public override void StartAttacking()
    {
        //Execute the tower's animate
        MainTower.StartProgress();
        _currentProjectTile.SetCauseDamage(Object.BaseDamage);
    }

    public void ShootProjectTile(){
        _currentProjectTile.gameObject.SetActive(true);
    }

    public override void ReadyProjectile()
    {
        var newObject = PoolingObject.Instance.GetObject(Object.PrefabProjectTile);
        _currentProjectTile = newObject.GetComponent<ProjectTiles>();
        _currentProjectTile.transform.position = PositionReleaseProjectTile;
        _currentProjectTile.gameObject.SetActive(false);
        _currentProjectTile.SetCurrentEnemy(_currentTargetEnemy);
    }
    public void UpdateCurrentTargetEnemy()
    {
        if(_currentTargetEnemy == MainTower.CurrentTarget) return;
        _currentTargetEnemy = MainTower.CurrentTarget;
    }
}
