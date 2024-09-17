using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Solider : MonoBehaviour, IMoveable, IDamageable
{
    private static int Idle = Animator.StringToHash("Idle Level_1");
    private static int Walk = Animator.StringToHash("Walk Level_1");
    private static int IdleUp = Animator.StringToHash("Idle_Up Level_1");
    private static int AttackUp = Animator.StringToHash("Attack_Up Level_1");
    private static int Attack = Animator.StringToHash("Attack Level_1");
    private static int Death = Animator.StringToHash("Death Level_1");
    public float Speed { get; set; }
    public Vector3 Position { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    private GameObject tower;
    [SerializeField] private Tower _tower;
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
    [SerializeField] protected Enemy _currentTarget;
    private bool _isAttacking;
    private int _currentAnimation;
    private float _nextAttackTime;
    protected virtual void Start()
    {
        tower = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        _tower = tower.GetComponentInChildren<Tower>();
        _tower.UpgradeAction += UpgradeSolider;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _level = 1;
        _currentProjectTiles = null;
        _isAttacking = false;
        _damageCause = _baseDamage;
        _currentTarget = null;
    }
    
    protected virtual void Update()
    {
        Render();
        if (!_tower.CurrentTarget || _tower.CurrentTarget.GetIsDead() || Time.time < _nextAttackTime) return;
        LoadingProjectile();
        _isAttacking = true;
        _nextAttackTime = Time.time + coolDownAttack;
    }

    protected virtual void Render()
    {
        var newAnimation = Idle;
        if (_isAttacking) newAnimation = Attack;
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
    protected virtual void StartAttacking()
    {
        _currentProjectTiles.gameObject.SetActive(true);
        _currentProjectTiles.SetDamageCause(_damageCause);
        _isAttacking = false;
    }
    public virtual void TakenDamage(float damageAmount)
    {
        
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
}
