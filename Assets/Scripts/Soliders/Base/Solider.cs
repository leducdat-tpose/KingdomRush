using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Solider : MonoBehaviour, IMoveable, IDamageable
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int IdleUp = Animator.StringToHash("Idle_Up");
    private static readonly int AttackUp = Animator.StringToHash("Attack_Up");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Death");
    public float Speed { get; set; }
    public Vector3 Position { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    [SerializeField] private GameObject tower;
    [SerializeField] private Tower _tower;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _spritesUpgrade;
    [SerializeField] private int _level;
    [SerializeField] private GameObject _prefabProjectile;
    ProjectTiles _currentProjectTiles;
    [SerializeField] protected float coolDownAttack;
    private bool _isAttacking;
    private int _currentAnimation;
    private float _nextAttackTime;
    protected virtual void Start()
    {
        tower = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        _tower = tower.GetComponentInChildren<Tower>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _level = 1;
        _currentProjectTiles = null;
        _isAttacking = false;
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

    void LoadingProjectile()
    {
        GameObject newObject = PoolingObject.Instance.GetObject(_prefabProjectile);
        _currentProjectTiles = newObject.GetComponent<ProjectTiles>();
        _currentProjectTiles.transform.position = transform.position;
        _currentProjectTiles.gameObject.SetActive(false);
    }

    protected virtual void StartAttacking()
    {
        _currentProjectTiles.gameObject.SetActive(true);
        _currentProjectTiles.SetCurrentEnemy(_tower.CurrentTarget);
        _isAttacking = false;
    }
    
    public void TakenDamage(float damageAmount)
    {
        
    }

    public void Die()
    {
        
    }
}
