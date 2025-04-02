using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MeleeWarriorBehaviour : BaseBehaviour<Soldier>, IMoveable
{
    private float _nextAttackTime;
    private Enemy _currentTargetEnemy;
    public Enemy CurrentTargetEnemy => _currentTargetEnemy;
    [SerializeField]
    private List<Enemy> _enemiesList;
    [SerializeField]
    private bool _haveTower = true;
    public Tower MainTower{get; private set;}

    [SerializeField] 
    private CircleCollider2D _collider;
    private Vector3 _orderMovingPos = Vector3.zero;
    public Vector3 OrderMovingPos => _orderMovingPos;
    public override void Start()
    {
        base.Start();
        _nextAttackTime = 0;
        _currentTargetEnemy = null;
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = Object.DomainRange;
        if(_haveTower)
            MainTower = transform.root.GetComponentInChildren<Tower>();
        CurrentAnimation = idleAnimation;
        IdleState = new SoldierIdleState(Object, StateManager, this);
        WalkState = new SoldierWalkState(Object, StateManager, this);
        AttackState = new SoldierAttackState(Object, StateManager, this);
        DeathState = new SoldierDeathState(Object, StateManager, this);
        StateManager.Initialise(IdleState);
    }
    public override void FixedUpdate()
    {
        StateManager.CurrentState.PhysicsUpdate();
    }

    public override void Render()
    {
        int nextAnimation = idleAnimation;
        if(StateManager.CurrentState == AttackState) 
            nextAnimation = attackAnimation;
        else if(StateManager.CurrentState == DeathState)
            nextAnimation = deathAnimation;
        else if(Object.Rigidbody.velocity != Vector2.zero)
        {
            Vector2 direction = Object.Rigidbody.velocity / Object.MoveSpeed;
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
                nextAnimation = direction.y < 0 ? walkDownAnimation : walkUpAnimation;
            else{
                nextAnimation = walkSideAnimation;
                spriteRenderer.flipX = direction.x < 0;
            }
        }
        if(nextAnimation == CurrentAnimation
        || Object.gameObject.activeSelf == false) return;
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
        StateManager.CurrentState.FrameUpdate();
        Render();
    }
    public override void ReadyToAttack()
    {
        if (!_currentTargetEnemy 
        || _currentTargetEnemy.GetIsDead() 
        || Time.time < _nextAttackTime
        || StateManager.CurrentState == AttackState) return;
        float distance = Vector2.Distance(transform.position, _currentTargetEnemy.transform.position);
        if(distance > Object.AttackRange) return;
        StateManager.ChangeState(AttackState);
        if(_currentTargetEnemy.Behaviour.BeingProvoke == false)
            _currentTargetEnemy.Behaviour.SetBeingProvoke(true, Object);
        _nextAttackTime = Time.time + Object.CoolDownAttack;
    }
    public override void StartAttacking()
    {
        if(!_currentTargetEnemy) return;
        _currentTargetEnemy.TakenDamage(Object.BaseDamage);
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
            if(distance > Object.AttackRange) _currentTargetEnemy = null;
            return;
        }
        if(_enemiesList.Count == 0) {_currentTargetEnemy = null; return;}
        foreach(var enemy in _enemiesList)
        {
            if(enemy.Behaviour.BeingProvoke) continue;
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if(distance > Object.AttackRange) return;
            _currentTargetEnemy = enemy;
        }
    }

    public void MovingTo(Vector3 position)
    {
        position.z = 0;
        StateManager.ChangeState(WalkState);
        Object.SetRallyPosition(position);
    }
}
