using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider : MonoBehaviour, IMoveable, IDamageable
{
    public float Speed { get; set; }
    public Vector3 Position { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    [SerializeField] private Tower _tower;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _spritesUpgrade;
    [SerializeField] private int _level;
    [SerializeField] private GameObject _prefabProjectile;
    ProjectTiles _currentProjectTiles;
    [SerializeField] private float _coolDownAttack;
    protected virtual void Start()
    {
        _tower = GetComponent<Tower>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _level = 1;
        _currentProjectTiles = null;
    }
    
    protected virtual void Update()
    {
        
    }
    
    protected virtual void Render()
    {}

    void LoadingProjectile()
    {
        GameObject newObject = PoolingObject.Instance.GetObject(_prefabProjectile);
        _currentProjectTiles = newObject.GetComponent<ProjectTiles>();
        _currentProjectTiles.transform.position = transform.position;
        _currentProjectTiles.gameObject.SetActive(false);
    }
    
    public void TakenDamage(float damageAmount)
    {
        
    }

    public void Die()
    {
        
    }
}
