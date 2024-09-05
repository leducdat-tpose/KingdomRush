using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Tower : MonoBehaviour, IShootable
{
    [field: SerializeField] public float AttackRange { get; set; }

    public Collider2D Collider { get; set; }
    [SerializeField]
    private List<Enemy> _enemies;
    public Enemy EnemyCurrentTarget = null;

    private void Start()
    {
        Collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Enemy enemy = GetComponent<Enemy>();
        _enemies.Add(enemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Enemy enemy = GetComponent<Enemy>();
        if (!_enemies.Contains(enemy)) return;
        _enemies.Remove(enemy);
    }

    public void Shoot()
    {
    }
    //Some towers can shoot the farthest or shoot loads of enemies
    public virtual void UpdateCurrentTarget()
    {
        if (_enemies.Count <= 0)
        {
            EnemyCurrentTarget = null;
            return;
        }
        EnemyCurrentTarget = _enemies[0];
    }
}
