using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tower : MonoBehaviour, IShootable
{
    protected int StartingProgress = Animator.StringToHash("StartProgress");
    protected int Idle = Animator.StringToHash("Idle");
    protected bool HaveAnimation = false;
    [field: SerializeField] public float AttackRange { get; set; } = 5f;
    [SerializeField]
    private List<Enemy> _enemies;
    [SerializeField]
    private Enemy _currentTarget = null;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    public Enemy CurrentTarget => _currentTarget;
    protected bool InProgress;
    protected int _currentAnimation;
    public Collider2D Collider { get; set; }
    private void Reset()
    {
        Collider = GetComponent<Collider2D>();
        Collider.isTrigger = true;
        Collider.GetComponent<CircleCollider2D>().radius = AttackRange;
    }

    public virtual void Initialise(){}

    protected virtual void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentAnimation = Idle;
        InProgress = false;
    }

    protected virtual void Update()
    {
        UpdateCurrentTarget();
    }

    protected virtual void Render()
    {
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(Constant.EnemyTag)) return;
        Enemy newEnemy = other.GetComponent<Enemy>();
        if(newEnemy.GetIsDead()) return;
        _enemies.Add(newEnemy);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(Constant.EnemyTag)) return;
        Enemy existEnemy = other.GetComponent<Enemy>();
        if(!_enemies.Contains(existEnemy)) return;
        _enemies.Remove(existEnemy);
    }
    protected virtual void Shoot()
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
        foreach (Enemy enemy in _enemies)
        {
            float realDistance = Vector2.Distance(transform.position, enemy.transform.position);
            if(minDistance <= realDistance) continue;
            if(enemy.GetIsDead()) continue;
            minDistance = realDistance;
            _currentTarget = enemy;
        }
    }
    public void StartProgress()
    {
        if (HaveAnimation == false) return;
        InProgress = true;
    }
    public void StopProgress()
    {
        InProgress = false;
    }
}
