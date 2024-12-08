using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour, IDamageable, ICreature
{
    
    #region References
    [Header("References")]
    [SerializeField]
    private Rigidbody2D _rigidbody;
    public Rigidbody2D Rigidbody => _rigidbody;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [FormerlySerializedAs("_currentTargetSolider")] [SerializeField]
    private Warrior _currentTargetWarrior;
    public Warrior CurrentTargetWarrior => _currentTargetWarrior;
    [SerializeField] private DynamicHPBar _dynamicHpBar;
    #endregion
    
    #region Variables
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    public float BaseDamage { get;set;}
    public int Level { get;set;}
    [field: SerializeField] public float MoveSpeed { get; set; } = 5f;
    public Vector3 TargetPosition {  get; set; }
    public int PathIndex { get; set; } = 0;
    [SerializeField]
    private float coolDownAttack;
    public float CoolDownAttack => coolDownAttack;
    [SerializeField]
    private bool _beingProvoke = false;
    #endregion

    private EnemyBehaviour _behaviour;

    private void Awake()
    {
        this.tag = "Enemy";
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _behaviour = GetComponent<EnemyBehaviour>();
        _rigidbody.isKinematic = true;
        CurrentHealth = MaxHealth;
        TargetPosition = LevelManager.instance.GetStartingPoint();
        _currentTargetWarrior = null;
        transform.position = TargetPosition;
    }

    public void TakenDamage(float damageAmount)
    {
        if(_behaviour.GetCurrentState() == _behaviour.GetState(StateType.Death)) return;
        CurrentHealth -= damageAmount;
        _dynamicHpBar?.UpdateHealthBar(CurrentHealth, MaxHealth);
        if (CurrentHealth > 0) return;
        CurrentHealth = 0;
        _behaviour.ChangeState(StateType.Death);
        _rigidbody.velocity = Vector2.zero;
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
        _beingProvoke = false;
        _currentTargetWarrior = null;
    }
    public bool GetIsDead() 
    => _behaviour.GetCurrentState() == _behaviour.GetState(StateType.Death);

    public bool GetBeingProvoked()
    {
        return _beingProvoke;
    }
}
