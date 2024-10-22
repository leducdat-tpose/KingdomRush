using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    #region ID_Animations
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int WalkUp = Animator.StringToHash("WalkUp");
    private static readonly int WalkDown = Animator.StringToHash("WalkDown");
    private static readonly int WalkSide = Animator.StringToHash("WalkSide");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Attack = Animator.StringToHash("Attack");
    #endregion
    
    #region References
    [Header("References")]
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [FormerlySerializedAs("_currentTargetSolider")] [SerializeField]
    private Warrior currentTargetWarrior;
    [SerializeField] private DynamicHPBar _dynamicHpBar;
    #endregion
    
    #region Variables
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float MoveSpeed { get; set; } = 5f;
    public Vector3 TargetPosition {  get; set; }
    public int PathIndex { get; set; } = 0;
    private bool _isAttacking = false;
    private bool _isDead = false;
    private int _currentAnimation;
    [SerializeField]
    private float coolDownAttack;
    public float CoolDownAttack => coolDownAttack;
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
    public StateManager<Enemy> StateManager { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyDeathState DeathState { get; set; }
    public EnemyWalkState WalkState { get; set; }
    #endregion

    private void Awake()
    {
        this.tag = "Enemy";
        StateManager = new StateManager<Enemy>();
        IdleState = new EnemyIdleState(this, StateManager);
        AttackState = new EnemyAttackState(this, StateManager);
        WalkState = new EnemyWalkState(this, StateManager);
        DeathState = new EnemyDeathState(this, StateManager);
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        CurrentHealth = MaxHealth;
        TargetPosition = LevelManager.instance.GetStartingPoint();
        currentTargetWarrior = null;
        _currentAnimation = Idle;
        transform.position = TargetPosition;
        StateManager.Initialize(WalkState);
    }

    protected virtual void Update()
    {
        if (currentTargetWarrior)
        {
            StateManager.ChangeState(AttackState);
        }
        StateManager.CurrentState.FrameUpdate();
        Render();
    }

    private void FixedUpdate()
    {
        StateManager.CurrentState.PhysicsUpdate();
    }
    public void TakenDamage(float damageAmount)
    {
        if (_isDead) return;
        CurrentHealth -= damageAmount;
        _dynamicHpBar?.UpdateHealthBar(CurrentHealth, MaxHealth);
        if (CurrentHealth > 0) return;
        CurrentHealth = 0;
        _isDead = true;
        StateManager.ChangeState(DeathState);
        rb.velocity = Vector2.zero;
    }

    public void ReturnPoolObject()
    {
        PoolingObject.Instance.ReturnObject(this.gameObject);
        ResetValue();
    }

    public void MoveEnemy()
    {
        if (StateManager.CurrentState != WalkState) return;
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
            EnemySpawner.OnEnemyDestroy?.Invoke();
            return;
        }
        TargetPosition = LevelManager.instance.GetPoint(PathIndex);
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
        _beingProvoke = false;
        currentTargetWarrior = null;
    }
    public bool GetIsDead(){return _isDead;}

    public void StartAttacking()
    {
        _isAttacking = true;
    }

    protected virtual void CauseDamage()
    {
        if(_isDead || !currentTargetWarrior) return;
        currentTargetWarrior.TakenDamage(2);
    }
        

    public void StopAttacking()
    {
        _isAttacking = false;
        if (_beingProvoke) return;
        StateManager.ChangeState(WalkState);
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    public bool GetBeingProvoked()
    {
        return _beingProvoke;
    }

    public void SetBeingProvoke(bool value, Warrior sourceWarrior)
    {
        _beingProvoke = value;
        currentTargetWarrior = sourceWarrior;
        if(value) StateManager.ChangeState(AttackState);
    }

    public void StopBeingProvoked()
    {
        _beingProvoke = false;
        currentTargetWarrior = null;
        StateManager.ChangeState(WalkState);
    }
}
