using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Soldier : Warrior
{
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private CircleCollider2D _collider2D;
    [SerializeField] List<Enemy> _enemies;
    [SerializeField] Rigidbody2D _rigidbody2D;
    private void Reset()
    {
        _collider2D = GetComponent<CircleCollider2D>();
        _collider2D.radius = _attackRange;
        _collider2D.isTrigger = true;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.isKinematic = true;
    }

    protected override void Start()
    {
        base.Start();
        StandingPosition = transform.position;
        OwnerTower.ChangeStandingPosition += MoveToStandingPosition;
        IdleState = new SoldierIdleState(this, StateManager);
        WalkState = new SoldierWalkState(this, StateManager);
        AttackState = new SoldierAttackState(this, StateManager);
        DeathState = new SoldierDeathState(this, StateManager);
    }

    protected override void Update()
    {
        Render();
        MovingToStandingPosition();
        UpdateCurrentTarget();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Enemy newEnemy = other.GetComponent<Enemy>();
        _enemies.Add(newEnemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Enemy existEnemy = other.GetComponent<Enemy>();
        if(!_enemies.Contains(existEnemy)) return;
        _enemies.Remove(existEnemy);
    }

    private void MoveToStandingPosition(Vector3 standingPosition)
    {
        StandingPosition = standingPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        isMoving = true;
    }
    private void UpdateCurrentTarget()
    {
        if (_currentTarget) return;
        if (_enemies.Count == 0)
        {
            _currentTarget = null;
            return;
        }
        foreach (var enemy in _enemies)
        {
            if(enemy.GetBeingProvoked()) continue;
            _currentTarget = enemy;
            enemy.SetBeingProvoke(true, this);
            return;
        }
    }

    protected override void LoadingProjectile()
    {
    }

    protected override void StartAttacking()
    {
    }

    private void StopAttacking()
    {
        SetIsAttacking(false);
    }

    private void MovingToTargetPosition(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        _rigidbody2D.velocity = direction * Speed;
    }

    private void MovingToStandingPosition()
    {
        if (!isMoving) return;
        MovingToTargetPosition(StandingPosition);
        isMoving = true;
        if (!(Vector2.Distance(transform.position, StandingPosition) < 0.1f)) return;
        isMoving = false;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
