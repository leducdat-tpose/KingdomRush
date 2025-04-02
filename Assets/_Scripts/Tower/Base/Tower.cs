using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tower : MonoBehaviour
{
    protected int StartingProgress = Animator.StringToHash("StartProgress");
    protected int Idle = Animator.StringToHash("Idle");
    protected bool HaveAnimation = false;
    [SerializeField]
    private List<Enemy> _enemies;
    [SerializeField]
    private Enemy _currentTarget = null;
    public Enemy CurrentTarget => _currentTarget;
    protected bool InProgress;
    protected int _currentAnimation;
    private Collider2D _collider;
    [SerializeField]
    private float _colliderRange = 5f;
    private void Reset()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        _collider.GetComponent<CircleCollider2D>().radius = _colliderRange;
    }

    public virtual void Initialise(){}

    protected virtual void Start()
    {
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
        //CompareTag is way better than  other.gameObject.tag == tag
        //Instead of comparing strings, Unity converts the tag into an integer ID and compares the IDs associated with those tags
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
        StartCoroutine(nameof(WaitUpdateCurrentTarget));
    }

    private IEnumerator WaitUpdateCurrentTarget()
    {
        yield return new WaitForSecondsRealtime(1.5f);
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

    public float GetColliderRange() => _colliderRange;

}
