using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
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

    public EnemyBehaviour Behaviour{get; private set;}
    [field: SerializeField]
    public DynamicHPBar DynamicHpBar { get; set; }

    private void Awake()
    {
        this.tag = Constant.EnemyTag;
    }

    private void Start()
    {
        DynamicHpBar = GetComponentInChildren<DynamicHPBar>();
        _rigidbody = GetComponent<Rigidbody2D>();
        Behaviour = GetComponent<EnemyBehaviour>();
        _rigidbody.isKinematic = true;
        CurrentHealth = MaxHealth;
        TargetPosition = GameController.Instance.LevelManager.GetStartingPoint();
        _currentTargetWarrior = null;
        transform.position = TargetPosition;
    }

    public void TakenDamage(float damageAmount)
    {
        if(Behaviour.GetCurrentState() == Behaviour.GetState(StateType.Death)) return;
        CurrentHealth -= damageAmount;
        DynamicHpBar?.UpdateHealthBar(CurrentHealth, MaxHealth);
        if (CurrentHealth > 0) return;
        ResourceManagement.CollectResource?.Invoke(_moneyEarned);
        CurrentHealth = 0;
        Behaviour.ChangeState(StateType.Death);
        _rigidbody.velocity = Vector2.zero;
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
        _currentTargetWarrior = null;
    }
    public bool GetIsDead() 
    => Behaviour.GetCurrentState() == Behaviour.GetState(StateType.Death);
}
