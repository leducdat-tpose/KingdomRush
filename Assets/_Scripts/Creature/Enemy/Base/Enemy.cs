using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
public class Enemy : BaseBehaviour, IDamageable
{
    
    #region References
    [Header("References")]
    protected IDamageable currentTarget;
    public IDamageable CurrentTarget => currentTarget;
    [SerializeField]
    private Rigidbody2D _rigid;
    public Rigidbody2D Rigid => _rigid;
    #endregion
    
    #region Variables
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField]
    public float BaseDamage { get;set;} = 5;
    public int Level { get;set;}
    [field: SerializeField] public float MoveSpeed { get; set; } = 5f;
    public Vector3 TargetPosition {  get; set; }
    public int PathIndex { get; set; } = 0;
    [SerializeField]
    private float coolDownAttack;
    public float CoolDownAttack => coolDownAttack;
    [SerializeField]
    //After killing an enemy, it will give player money to upgrade tower, champs, etc.
    private int _moneyEarned = 5;
    #endregion
    public Action<float, float> HealthChanged { get; set; }

    protected StateManager<Enemy> stateManager;
    protected EnemyIdleState<Enemy> idleState;
    protected EnemyAttackState<Enemy> attackState;
    protected EnemyDeathState<Enemy> deathState;
    protected EnemyWalkState<Enemy> walkState;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.isKinematic = true;
    }
    public override void Initialise()
    {
        base.Initialise();
        CurrentAnimation = idleAnimation;
        stateManager = new StateManager<Enemy>();
        idleState = new EnemyIdleState<Enemy>(this, stateManager);
        walkState = new EnemyWalkState<Enemy>(this, stateManager);
        attackState = new EnemyAttackState<Enemy>(this, stateManager);
        deathState = new EnemyDeathState<Enemy>(this, stateManager);
        stateManager.AddState(idleState);
        stateManager.AddState(walkState);
        stateManager.AddState(attackState);
        stateManager.AddState(deathState);
        stateManager.Initialise(idleState);
    }

    private void Start()
    {
        
        CurrentHealth = MaxHealth;
        TargetPosition = GameController.Instance.LevelManager.GetStartingPoint();
        currentTarget = null;
        transform.position = TargetPosition;
    }

    public void TakenDamage(float damageAmount)
    {
        if(stateManager.CurrentState == deathState) return;
        CurrentHealth -= damageAmount;
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth > 0) return;
        ResourceManagement.CollectResource?.Invoke(_moneyEarned);
        CurrentHealth = 0;
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
        CurrentHealth = MaxHealth;
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
        _nextAttackTime = Time.time + CoolDownAttack;
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
            Vector2 direction = Rigid.velocity / MoveSpeed;
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
    public override void StartAttacking()
    {
        if(currentTarget == null) return;
        currentTarget.TakenDamage(BaseDamage);
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
