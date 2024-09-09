using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    public Rigidbody2D rb { get; set; }
    [SerializeField]
    private Animator animator;
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth {  get; set; }
    [field: SerializeField] public float MoveSpeed { get; set; } = 5f;
    public Vector3 TargetPosition {  get; set; }
    public int PathIndex { get; set; } = 0;
    private string _currentAnimation = "";

    #region Animation Trigger

    public enum AnimationTriggerType
    {
        Attack,
        Walk,
        Death
    }
    #endregion

    #region State Machine Variables
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyDeathState DeathState { get; set; }
    public EnemyWalkState WalkState { get; set; }
    #endregion

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        WalkState = new EnemyWalkState(this, StateMachine);
        DeathState = new EnemyDeathState(this, StateMachine);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        CurrentHealth = MaxHealth;
        TargetPosition = LevelManager.instance.GetStartingPoint();
        transform.position = TargetPosition;
        StateMachine.Initialize(WalkState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if(CurrentHealth <= 0 ) {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void MoveEnemy()
    {
        Vector2 direction = (TargetPosition - transform.position).normalized;
        rb.velocity = direction * MoveSpeed;
    }

    public void UpdateTargetPosition()
    {
        if (Vector2.Distance(TargetPosition, transform.position) > 0.1f || !this.gameObject.activeSelf) return;
        PathIndex++;
        if(PathIndex == LevelManager.instance.GetWaypointsLength())
        {
            this.gameObject.SetActive(false);
            PoolingObject.Instance.ReturnObject(this.gameObject);
            EnemySpawner.onEnemyDestroy?.Invoke();
            return;
        }
        TargetPosition = LevelManager.instance.GetPoint(PathIndex);
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public void ChangeAnimation(string nextAnimation)
    {
        if(_currentAnimation == nextAnimation) return;
        animator.Play(nextAnimation);
        _currentAnimation = nextAnimation;
    }
}
