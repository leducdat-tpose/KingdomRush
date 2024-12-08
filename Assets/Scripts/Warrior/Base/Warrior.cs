using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer),
    typeof(Animator))]
public class Warrior : MonoBehaviour, IMoveable, IDamageable, ICreature
{
    #region ID_Animations
    private int Idle = Animator.StringToHash("Idle Level_1");
    private int Walk = Animator.StringToHash("Walk Level_1");
    private int IdleUp = Animator.StringToHash("Idle_Up Level_1");
    private int AttackUp = Animator.StringToHash("Attack_Up Level_1");
    private int Attack = Animator.StringToHash("Attack Level_1");
    private int Death = Animator.StringToHash("Death Level_1");
    #endregion
    [Header("References")]
    [SerializeField]protected GameObject tower;
    protected Tower _tower;
    public Tower OwnerTower => _tower;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _prefabProjectile;
    public GameObject PrefabProjectTile => _prefabProjectile;
    ProjectTiles _currentProjectTiles;
    [SerializeField] protected Enemy _currentTarget;
    [SerializeField] private List<Sprite> _spritesSoliderUpgrade;


    [Header("Attributes")]
    [SerializeField] private float _upgradeDamageIndex;
    [field:SerializeField]public float MaxHealth { get; set; }
    [field:SerializeField]public float CurrentHealth { get; set; }
    public float BaseDamage { get; set; }
    public int Level { get; set; }
    public float MoveSpeed { get; set;}
    private BaseBehaviour<Warrior> _behaviour;
    

    [SerializeField] private float _damageCause;
    [SerializeField] protected float coolDownAttack;
    [field:SerializeField]public float Speed { get; set; }
    public Vector3 StandingPosition { get; set; }
    
    public float CoolDownAttack => coolDownAttack;
    private bool _isAttacking;
    protected bool isMoving;
    private bool _isDead;
    private float _nextAttackTime;
    protected Vector3 LoadingProjectilePosition;
    private int _currentAnimation;

    protected virtual void Start()
    {
        GetTower();
        _behaviour = new RangerWarriorBehaviour();
        LoadingProjectilePosition = transform.position;
        _tower.UpgradeAction += UpgradeSolider;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Level = 1;
        _currentProjectTiles = null;
        _isAttacking = false;
        isMoving = false;
        _isDead = false;
        _damageCause = BaseDamage;
        _currentTarget = null;
        CurrentHealth = MaxHealth;
    }

    protected virtual void GetTower()
    {
        tower = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        _tower = tower.GetComponentInChildren<Tower>();
    }

    protected virtual void ShootingProjectTile()
    {
        if (!_tower.CurrentTarget || _tower.CurrentTarget.GetIsDead() || Time.time < _nextAttackTime) return;
        LoadingProjectile();
        _isAttacking = true;
        _nextAttackTime = Time.time + coolDownAttack;
    }
    protected virtual void Render(bool canTurnUp = true)
    {
        var newAnimation = Idle;
        if (_isAttacking) newAnimation = Attack;
        else if(isMoving)
        {
            newAnimation = Walk;
            Vector2 direction = StandingPosition - transform.position;
            _spriteRenderer.flipX = direction.x < -0.1f;
        }
        else if (_isDead) newAnimation = Death;
        if (_tower.CurrentTarget && canTurnUp)
        {
            Vector2 direction = _tower.CurrentTarget.transform.position - tower.transform.position;
            _spriteRenderer.flipX = direction.x < -0.8f;
            if (direction.y > 1.0f)
            {
                newAnimation = IdleUp;
                if (_isAttacking) newAnimation = AttackUp;
            }
        }
        if (_currentAnimation == newAnimation) return;
        _animator.CrossFade(newAnimation, 0.1f, 0);
        _currentAnimation = newAnimation;
    }

    protected virtual void LoadingProjectile()
    {
        var newObject = PoolingObject.Instance.GetObject(_prefabProjectile);
        _currentProjectTiles = newObject.GetComponent<ProjectTiles>();
        _currentProjectTiles.transform.position = LoadingProjectilePosition;
        _currentProjectTiles.gameObject.SetActive(false);
        _currentProjectTiles.SetCurrentEnemy(_tower.CurrentTarget);
    }
    public virtual void TakenDamage(float damageAmount)
    {
    }
    protected virtual void UpgradeSolider(int towerLevel)
    {
        if (Level == _spritesSoliderUpgrade.Count) return;
        Level = towerLevel;
        _damageCause = BaseDamage * _upgradeDamageIndex * Level;
        _spriteRenderer.sprite = _spritesSoliderUpgrade[Level - 1];
        string stringLevel = " Level_"+Level.ToString();
        Idle = Animator.StringToHash("Idle" + stringLevel);
        Walk = Animator.StringToHash("Walk" + stringLevel);
        Attack = Animator.StringToHash("Attack" + stringLevel);
        Death = Animator.StringToHash("Death" + stringLevel);
        IdleUp = Animator.StringToHash("Idle_Up" + stringLevel);
        AttackUp = Animator.StringToHash("Attack_Up" + stringLevel);
    }
    protected void SetIsAttacking(bool _isAttacking)
    {
        this._isAttacking = _isAttacking;
    }
    private void ReturnToTower()
    {
        _currentTarget = null;
        gameObject.SetActive(false);
        transform.position = _tower.transform.position;
    }

    public Enemy GetCurrentTarget()
    {
        return _currentTarget;
    }
}
