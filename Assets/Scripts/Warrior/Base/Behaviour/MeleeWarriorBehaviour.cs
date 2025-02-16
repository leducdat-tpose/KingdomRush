using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MeleeWarriorBehaviour : BaseBehaviour<Soldier>
{
    private float _nextAttackTime;
    private Enemy _currentTargetEnemy;
    [SerializeField]
    private List<Enemy> _enemiesList;
    [SerializeField]
    private bool _haveTower = true;
    public Tower MainTower{get; private set;}
    [SerializeField] 
    private CircleCollider2D _collider;
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
        _nextAttackTime = Time.time + Object.CoolDownAttack;
    }
    public override void StartAttacking()
    {
        _currentTargetEnemy.TakenDamage(Object.BaseDamage);
    }
    
    public void UpdateCurrentTargetEnemy()
    {
        if (_enemiesList.Count == 0)
        {
            _currentTargetEnemy = null;
            return;
        }
        float minDistance = Mathf.Infinity;
        foreach (Enemy enemy in _enemiesList)
        {
            float realDistance = Vector2.Distance(transform.position, enemy.transform.position);
            if(minDistance <= realDistance) continue;
            if(enemy.GetIsDead()) continue;
            minDistance = realDistance;
            _currentTargetEnemy = enemy;
        }
    }
}
