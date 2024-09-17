using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class Soldier : Solider
{
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private CircleCollider2D _collider2D;
    [SerializeField] List<Enemy> _enemies;

    private void Reset()
    {
        _collider2D = GetComponent<CircleCollider2D>();
        _collider2D.radius = _attackRange;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        UpdateCurrentTarget();
    }

    protected override void Render()
    {
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

    private void UpdateCurrentTarget()
    {
        if (_enemies.Count == 0)
        {
            _currentTarget = null;
            return;
        }
        float minDistance = Mathf.Infinity;
        foreach (var enemy in _enemies)
        {
            float realDistance = Vector2.Distance(transform.position, enemy.transform.position);
            if(minDistance <= realDistance) continue;
            minDistance = realDistance;
            _currentTarget = enemy;
        }
    }
    
    protected override void LoadingProjectile()
    {
    }

    protected override void StartAttacking()
    {
    }
}
