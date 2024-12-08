using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Soldier : Warrior
{
    [Header("Soldier Settings")]
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private CircleCollider2D _collider2D;
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] List<Enemy> _enemies;
    private bool _goToMarkPosition;
    #region State Machine Variables

    private StateManager<Soldier> StateManager;
    private SoldierIdleState IdleState { get; set; }
    private SoldierWalkState WalkState { get; set; }
    private SoldierAttackState AttackState { get; set; }
    private SoldierDeathState DeathState { get; set; }
    #endregion
    private void Reset()
    {
        _collider2D = GetComponent<CircleCollider2D>();
        _collider2D.radius = _attackRange;
        _collider2D.isTrigger = true;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.isKinematic = true;
        _goToMarkPosition = false;
    }

    protected override void Start()
    {
        base.Start();
        StandingPosition = transform.position;
        StateManager = new StateManager<Soldier>();
        OwnerTower.ChangeStandingPosition += MoveToStandingPosition;
        IdleState = new SoldierIdleState(this, StateManager);
        WalkState = new SoldierWalkState(this, StateManager);
        AttackState = new SoldierAttackState(this, StateManager);
        DeathState = new SoldierDeathState(this, StateManager);
        StateManager.Initialize(IdleState);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Enemy newEnemy = other.GetComponent<Enemy>();
        _enemies.Add(newEnemy);
    }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (!other.CompareTag("Enemy")) return;
    //     Enemy existEnemy = other.GetComponent<Enemy>();
    //     if(!_enemies.Contains(existEnemy)) return;
    //     _enemies.Remove(existEnemy);
    //     if(_currentTarget) _currentTarget.StopBeingProvoked();
    // }

    private void MoveToStandingPosition(Vector3 standingPosition)
    {
        StateManager.ChangeState(WalkState);
        StandingPosition = standingPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        isMoving = true;
        _goToMarkPosition = true;
    }
    private void UpdateCurrentTarget()
    {
        // if (_currentTarget) return;
        // if (_enemies.Count == 0)
        // {
        //     _currentTarget = null;
        //     return;
        // }
        // foreach (var enemy in _enemies)
        // {
        //     if(enemy.GetBeingProvoked()) continue;
        //     _currentTarget = enemy;
        //     enemy.SetBeingProvoke(true, this);
        //     StartMoving();
        //     return;
        // }
    }

    protected override void LoadingProjectile()
    {
    }

    private void StopAttacking()
    {
        SetIsAttacking(false);
    }

    public void StopAttackingState()
    {
        StateManager.ChangeState(IdleState);
        SetIsAttacking(false);
    }
    
    private void StartMoving()
    {
        isMoving = true;
        StateManager.ChangeState(WalkState);
    }
    public void StopMoving()
    {
        isMoving = false;
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void MovingToTargetPosition(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        _rigidbody2D.velocity = direction * Speed;
    }

    public void MovingToStandingPosition()
    {
        if (!isMoving || !_goToMarkPosition) return;
        MovingToTargetPosition(StandingPosition);
        isMoving = true;
        if (!(Vector2.Distance(transform.position, StandingPosition) < 0.1f)) return;
        StateManager.ChangeState(IdleState);
        _goToMarkPosition = false;
    }

    public void MovingToEnemyPosition()
    {
        if (!isMoving || !_currentTarget || _goToMarkPosition) return;
        var enemyPosition = _currentTarget.transform.position;
        MovingToTargetPosition(enemyPosition);
        isMoving = true;
        if (!(Vector2.Distance(transform.position, enemyPosition) < 0.3f)) return;
        if(!_currentTarget) StateManager.ChangeState(IdleState);
        else StateManager.ChangeState(AttackState);
    }
    
}
