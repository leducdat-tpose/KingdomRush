using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    #region ID_Animations
    protected static readonly int Idle = Animator.StringToHash("Idle");
    protected static readonly int WalkUp = Animator.StringToHash("WalkUp");
    protected static readonly int WalkDown = Animator.StringToHash("WalkDown");
    protected static readonly int WalkSide = Animator.StringToHash("WalkSide");
    protected static readonly int Death = Animator.StringToHash("Death");
    protected static readonly int Attack = Animator.StringToHash("Attack");
    #endregion
    
    #region References
    [Header("References")]
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    #endregion
    
    #region Variables
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float MoveSpeed { get; set; } = 5f;
    public Vector3 TargetPosition {  get; set; }
    public int PathIndex { get; set; } = 0;
    private bool _isAttacking = false;
    private bool _isDead = false;
    private int _currentAnimation = Idle;
    [SerializeField]
    private bool _beingProvoke = false;
    #endregion

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        CurrentHealth = MaxHealth;
        TargetPosition = LevelManager.instance.GetStartingPoint();
        transform.position = TargetPosition;
        StateMachine.Initialize(WalkState);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartAttacking();
        }
        StateMachine.CurrentEnemyState.FrameUpdate();
        Render();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    public void TakenDamage(float damageAmount)
    {
        if (_isDead) return;
        CurrentHealth -= damageAmount;
        if (!(CurrentHealth <= 0)) return;
        CurrentHealth = 0;
        _isDead = true;
        StateMachine.ChangeState(DeathState);
        rb.velocity = Vector2.zero;
    }

    public void Die()
    {
        PoolingObject.Instance.ReturnObject(this.gameObject);
        ResetValue();
    }

    public void MoveEnemy()
    {
        if (StateMachine.CurrentEnemyState != WalkState) return;
        Vector2 direction = (TargetPosition - transform.position).normalized;
        rb.velocity = direction * MoveSpeed;
    }

    public void UpdateTargetPosition()
    {
        if (Vector2.Distance(TargetPosition, transform.position) > 0.1f || !this.gameObject.activeSelf || _isDead) return;
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

    protected virtual void Render()
    {
        var nextAnimation = Idle;
        if (_isAttacking) nextAnimation = Attack;
        else if (_isDead) nextAnimation = Death;
        else if (rb.velocity != Vector2.zero)
        {
            Vector2 direction = rb.velocity / MoveSpeed;
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) nextAnimation = direction.y < 0 ? WalkDown : WalkUp;
            else
            {
                nextAnimation = WalkSide;
                spriteRenderer.flipX = direction.x < 0;
            }
        }
        if(nextAnimation == _currentAnimation || gameObject.activeSelf == false) return;
        animator.CrossFade(nextAnimation, 0.1f, 0);
        _currentAnimation = nextAnimation;
    }

    protected virtual void ResetValue()
    {
        CurrentHealth = MaxHealth;
        _isAttacking = false;
        _isDead = false;
        transform.position = Vector3.zero;
        _currentAnimation = Idle;
    }
    public bool GetIsDead(){return _isDead;}

    public void StartAttacking()
    {
        _isAttacking = true;
        StateMachine.ChangeState(AttackState);
    }

    public void StopAttacking()
    {
        _isAttacking = false;
        StateMachine.ChangeState(WalkState);
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    public bool GetBeingProvoked()
    {
        return _beingProvoke;
    }

    public void SetBeingProvoke(bool value)
    {
        _beingProvoke = value;
    }
}
