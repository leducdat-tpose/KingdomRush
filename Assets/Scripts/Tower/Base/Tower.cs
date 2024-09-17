using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tower : MonoBehaviour, IShootable
{
    public event Action<int> UpgradeAction;
    [field: SerializeField] public float AttackRange { get; set; } = 5f;
    [SerializeField]
    private List<Enemy> _enemies;
    [SerializeField]
    private Enemy _currentTarget = null;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private List<Sprite> _spritesTowerUpgrade;
    public Enemy CurrentTarget => _currentTarget;
    [SerializeField]
    private int _towerLevel = 1;
    public Collider2D Collider { get; set; }
    private void Reset()
    {
        Collider = GetComponent<Collider2D>();
        Collider.isTrigger = true;
        Collider.GetComponent<CircleCollider2D>().radius = AttackRange;
    }

    protected virtual void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateCurrentTarget();
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpgradeTower();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Enemy newEnemy = other.GetComponent<Enemy>();
        _enemies.Add(newEnemy);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Enemy existEnemy = other.GetComponent<Enemy>();
        if(!_enemies.Contains(existEnemy)) return;
        _enemies.Remove(existEnemy);
    }
    public void Shoot()
    {
    }

    protected virtual void UpdateCurrentTarget()
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

    private void UpgradeTower()
    {
        if (_towerLevel == _spritesTowerUpgrade.Count) return;
        _towerLevel++;
        _spriteRenderer.sprite = _spritesTowerUpgrade[_towerLevel - 1];
        UpgradeAction?.Invoke(_towerLevel);
    }
}
