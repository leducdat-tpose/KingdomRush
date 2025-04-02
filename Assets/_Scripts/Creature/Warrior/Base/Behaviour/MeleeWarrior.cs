using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class MeleeWarrior : Warrior, IControllable
{
    protected float domainRange = 3f;
    protected float attackRange = 0.5f;
    private float _nextAttackTime;
    private Enemy _currentTargetEnemy;
    public Vector3 RallyPosition {get; private set;} = Vector3.zero;
    public Enemy CurrentTargetEnemy => _currentTargetEnemy;
    [SerializeField]
    private List<Enemy> _enemiesList;
    [SerializeField]
    private Tower _initTower;
    public Tower InitTower => _initTower;

    public Rigidbody2D Rigid{get; private set;}

    [SerializeField] 
    private CircleCollider2D _collider;
    private Vector3 _orderMovingPos = Vector3.zero;
    public Vector3 OrderMovingPos => _orderMovingPos;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = domainRange;
    }
    public override void Initialise()
    {
        base.Initialise();
        _nextAttackTime = 0;
        _currentTargetEnemy = null;
        CurrentAnimation = idleAnimation;
    }
    public override void FixedUpdate()
    {
        stateManager.FixedUpdate();
    }

    public override void Render()
    {
        int nextAnimation = idleAnimation;
        if(stateManager.CurrentState == attackState) 
            nextAnimation = attackAnimation;
        else if(stateManager.CurrentState == deathState)
            nextAnimation = deathAnimation;
        else if(Rigid.velocity != Vector2.zero)
        {
            Vector2 direction = Rigid.velocity / data.MoveSpeed;
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
                nextAnimation = direction.y < 0 ? walkDownAnimation : walkUpAnimation;
            else{
                nextAnimation = walkSideAnimation;
                spriteRenderer.flipX = direction.x < 0;
            }
        }
        if(nextAnimation == CurrentAnimation
        || this.gameObject.activeSelf == false) return;
        animator.CrossFade(nextAnimation, 0.1f, 0);
        CurrentAnimation = nextAnimation;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(Constant.EnemyTag)) return;
        var newEnemy = other.GetComponent<Enemy>();
        if(newEnemy.GetIsDead()) return;
        _enemiesList.Add(newEnemy);
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag(Constant.EnemyTag)) return;
        var existEnemy = other.GetComponent<Enemy>();
        if(!_enemiesList.Contains(existEnemy)) return;
        _enemiesList.Remove(existEnemy);
    }
    public override void Update()
    {
        UpdateCurrentTargetEnemy();
        ReadyToAttack();
        stateManager.Update();
        Render();
    }
    public override void ReadyToAttack()
    {
        if (!_currentTargetEnemy 
        || _currentTargetEnemy.GetIsDead() 
        || Time.time < _nextAttackTime
        || stateManager.CurrentState == attackState) return;
        float distance = Vector2.Distance(transform.position, _currentTargetEnemy.transform.position);
        if(distance > attackRange) return;
        stateManager.ChangeState(attackState);
        if(_currentTargetEnemy.BeingProvoke == false)
            _currentTargetEnemy.SetBeingProvoke(true, this);
        _nextAttackTime = Time.time + data.CoolDownAttack;
    }
    public override void StartAttacking()
    {
        if(!_currentTargetEnemy) return;
        _currentTargetEnemy.TakenDamage(data.Damage);
    }
    public override void StopAttacking()
    {
        base.StopAttacking();
        if(_currentTargetEnemy.CurrentHealth <= 0) _currentTargetEnemy = null;
    }

    public void UpdateCurrentTargetEnemy()
    {
        if(_currentTargetEnemy != null)
        {
            float distance = Vector2.Distance(transform.position, _currentTargetEnemy.transform.position);
            if(distance > attackRange) _currentTargetEnemy = null;
            return;
        }
        if(_enemiesList.Count == 0) {_currentTargetEnemy = null; return;}
        foreach(var enemy in _enemiesList)
        {
            if(enemy.BeingProvoke) continue;
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if(distance > attackRange) return;
            _currentTargetEnemy = enemy;
        }
    }

    public void MovingTo(Vector3 position)
    {
        position.z = 0;
        stateManager.ChangeState(walkState);
        SetRallyPosition(position);
    }
    
    public void SetRallyPosition(Vector3 position) => RallyPosition = position;
}