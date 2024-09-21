using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Warrior : MonoBehaviour, IMoveable, IDamageable
{
    private int Idle = Animator.StringToHash("Idle Level_1");
    private int Walk = Animator.StringToHash("Walk Level_1");
    private int IdleUp = Animator.StringToHash("Idle_Up Level_1");
    private int AttackUp = Animator.StringToHash("Attack_Up Level_1");
    private int Attack = Animator.StringToHash("Attack Level_1");
    private int Death = Animator.StringToHash("Death Level_1");
    [field:SerializeField]public float Speed { get; set; }
    public Vector3 StandingPosition { get; set; }
    [field:SerializeField]public float MaxHealth { get; set; }
    [field:SerializeField]public float CurrentHealth { get; set; }
    private GameObject tower;
    [SerializeField] private Tower _tower;
    protected Tower OwnerTower => _tower;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _spritesSoliderUpgrade;
    [SerializeField] private int _level;
    [SerializeField] private GameObject _prefabProjectile;
    ProjectTiles _currentProjectTiles;
    [SerializeField] private float _baseDamage;
    [SerializeField] private float _upgradeDamageIndex;
    [SerializeField] private float _damageCause;
    [SerializeField] protected float coolDownAttack;
    public float CoolDownAttack => coolDownAttack;
    [SerializeField] protected Enemy _currentTarget;
    private bool _isAttacking;
    protected bool isMoving;
    private bool _isDead;
    private int _currentAnimation;
    private float _nextAttackTime;

    protected virtual void Start()
    {
        GetTower();
        _tower.UpgradeAction += UpgradeSolider;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _level = 1;
        _currentProjectTiles = null;
        _isAttacking = false;
        isMoving = false;
        _isDead = false;
        _damageCause = _baseDamage;
        _currentTarget = null;
        CurrentHealth = MaxHealth;
    }

    protected virtual void GetTower()
    {
        tower = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        _tower = tower.GetComponentInChildren<Tower>();
    }
    protected virtual void Update()
    {
        Render();
        if (!_tower.CurrentTarget || _tower.CurrentTarget.GetIsDead() || Time.time < _nextAttackTime) return;
        LoadingProjectile();
        _isAttacking = true;
        _nextAttackTime = Time.time + coolDownAttack;
    }

    protected virtual void FixedUpdate()
    {
        
    }
    protected virtual void Render()
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
        if (_tower.CurrentTarget)
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
        GameObject newObject = PoolingObject.Instance.GetObject(_prefabProjectile);
        _currentProjectTiles = newObject.GetComponent<ProjectTiles>();
        _currentProjectTiles.transform.position = transform.position;
        _currentProjectTiles.gameObject.SetActive(false);
        _currentProjectTiles.SetCurrentEnemy(_tower.CurrentTarget);
    }
    public virtual void StartAttacking()
    {
        _currentProjectTiles.gameObject.SetActive(true);
        _currentProjectTiles.SetDamageCause(_damageCause);
        _isAttacking = false;
    }
    public virtual void TakenDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth > 0) return;
        _isDead = true;
        _currentTarget.StopBeingProvoked();
    }
    private void UpgradeSolider(int towerLevel)
    {
        if (_level == _spritesSoliderUpgrade.Count) return;
        _level = towerLevel;
        _damageCause = _baseDamage * _upgradeDamageIndex * _level;
        _spriteRenderer.sprite = _spritesSoliderUpgrade[_level - 1];
        string stringLevel = " Level_"+_level.ToString();
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

    protected virtual void DealDamage()
    {
        if (!_currentTarget) return;
        _currentTarget.TakenDamage(_damageCause);
        if(_currentTarget.GetIsDead()) _currentTarget = null;
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
