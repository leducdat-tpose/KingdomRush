using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
public abstract class Enemy : BaseBehaviour, IDamageable
{
    #region References
    [Header("References")]
    protected IDamageable currentTarget;
    public IDamageable CurrentTarget => currentTarget;
    [SerializeField]
    protected EnemyData data;
    [SerializeField]
    private Rigidbody2D _rigid;
    public Rigidbody2D Rigid => _rigid;
    #endregion
    
    #region Variables
    protected float currentHealth;
    public float CurrentHealth => currentHealth;
    public float MoveSpeed => data.MoveSpeed;
    public Vector3 TargetPosition {  get; set; }
    public int PathIndex { get; set; } = 0;
    #endregion
    public Action<float, float> HealthChanged { get; set; }

    protected StateManager<Enemy> stateManager;
    protected EnemyIdleState idleState;
    protected EnemyAttackState attackState;
    protected EnemyDeathState deathState;
    protected EnemyWalkState walkState;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.isKinematic = true;
        LoadStateManager();
    }
    public override void Initialise()
    {
        base.Initialise();
        currentAnimation = idleAnimation;
    }

    protected void LoadStateManager()
    {
        stateManager = new StateManager<Enemy>();
        idleState = new EnemyIdleState(this, stateManager);
        walkState = new EnemyWalkState(this, stateManager);
        attackState = new EnemyAttackState(this, stateManager);
        deathState = new EnemyDeathState(this, stateManager);
        stateManager.AddState(idleState);
        stateManager.AddState(walkState);
        stateManager.AddState(attackState);
        stateManager.AddState(deathState);
        stateManager.Initialise(idleState);
    }

    private void Start()
    {
        currentHealth = data.MaxHealth;
        TargetPosition = GameController.Instance.LevelManager.GetStartingPoint();
        currentTarget = null;
        transform.position = TargetPosition;
    }

    public void TakenDamage(float damageAmount)
    {
        if(stateManager.CurrentState == deathState) return;
        currentHealth -= damageAmount;
        HealthChanged?.Invoke(currentHealth, data.MaxHealth);
        if (currentHealth > 0) return;
        ResourceManagement.CollectResource?.Invoke(data.MoneyEarned);
        currentHealth = 0;
        stateManager.ChangeState(deathState);
        _rigid.velocity = Vector2.zero;
    }
    IEnumerator StartRemainDeath(){
        yield return new WaitForSeconds(3f);
        ReturnPoolObject();
    }
    public void ReturnPoolObject()
    {
        this.gameObject.SetActive(false);
        PoolingObject.Instance.ReturnObject(this.gameObject);
        EnemySpawner.OnEnemyDestroy?.Invoke();
        ResetValue();
    }
    protected virtual void ResetValue()
    {
        currentHealth = data.MaxHealth;
        transform.position = Vector3.zero;
        currentTarget = null;
    }
    public override bool GetIsDead()
    {
        return stateManager.CurrentState == deathState;
    }
    
    private float _nextAttackTime;
    public bool BeingProvoke {get; private set;}
    private void OnEnable() {
        BeingProvoke = false;
    }
    public override void Update()
    {
        if(!(stateManager.CurrentState == deathState))
        {
            if(!BeingProvoke) stateManager.ChangeState(walkState);
            else ReadyToAttack();
        }
        stateManager.CurrentState.Update();
        Render();
    }
    public override void ReadyToAttack()
    {
        if(Time.time < _nextAttackTime
        || currentTarget == null) return;
        stateManager.ChangeState(attackState);
        _nextAttackTime = Time.time + data.CoolDownAttack;
    }
    public override void FixedUpdate()
    {
        stateManager.CurrentState.FixedUpdate();
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
        if(nextAnimation == currentAnimation
        || this.gameObject.activeSelf == false) return;
        animator.CrossFade(nextAnimation, 0.1f, 0);
        currentAnimation = nextAnimation;
    }
    public override void StartAttacking()
    {
        if(currentTarget == null) return;
        currentTarget.TakenDamage(data.Damage);
    }
    public override void StopAttacking()
    {
        base.StopAttacking();
        if(currentTarget == null) BeingProvoke = false;
        // || currentTarget <= 0) 
    }
    public override void CauseDamage()
    {
        base.CauseDamage();
    }
    public void SetBeingProvoke(bool value, Warrior creature)
    {
        if(!value){
            BeingProvoke = false;
            currentTarget = null;
            return;
        }
        BeingProvoke = true;
        // currentTarget = creature;
        currentTarget = null;
    }
}
