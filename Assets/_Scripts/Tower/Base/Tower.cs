using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tower : DetMonobehaviour
{
    protected int StartingProgress = Animator.StringToHash("StartProgress");
    protected int Idle = Animator.StringToHash("Idle");
    protected bool HaveAnimation = false;
    [SerializeField]
    private List<Enemy> _enemies;
    [SerializeField]
    private Enemy _currentTarget = null;
    public Enemy CurrentTarget => _currentTarget;
    protected bool inProgress;
    protected int currentAnimation;
    public Action<Enemy> TargetChanged;
    private Collider2D _collider;
    [SerializeField]
    protected float towerRange = 5f;
    [SerializeField]
    protected float timeUpdateTarget = 0.5f;
    [SerializeField]
    protected float rateUpdateTarget = 1f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        _collider.GetComponent<CircleCollider2D>().radius = towerRange;
    }

    private void Start() {
        Initialise();
        InvokeRepeating(nameof(UpdateCurrentTarget),timeUpdateTarget, rateUpdateTarget);
    }

    public override void Initialise()
    {
        base.Initialise();
        currentAnimation = Idle;
        inProgress = false;
    }

    protected virtual void Update()
    {
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
            Debug.DrawRay(transform.position, enemy.transform.position);
            TargetChanged?.Invoke(enemy);
            minDistance = realDistance;
            _currentTarget = enemy;
        }
    }
    public void StartProgress()
    {
        if (HaveAnimation == false) return;
        inProgress = true;
    }
    public void StopProgress()
    {
        inProgress = false;
    }
}
