using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    public Rigidbody2D rb { get; set; }
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth {  get; set; }
    [field: SerializeField] public float MoveSpeed { get; set; } = 5f;
    public Vector3 TargetPosition {  get; set; }
    public int PathIndex { get; set; } = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        CurrentHealth = MaxHealth;
        TargetPosition = LevelManager.instance.GetStartingPoint();
        transform.position = TargetPosition;
    }

    private void Update()
    {
        UpdateTargetPosition();
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if(CurrentHealth <= 0 ) {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void MoveEnemy()
    {
        Vector2 direction = (TargetPosition - transform.position).normalized;
        rb.velocity = direction * MoveSpeed;
    }

    public void UpdateTargetPosition()
    {
        if (Vector2.Distance(TargetPosition, transform.position) > 0.1f || !this.gameObject.activeSelf) return;
        PathIndex++;
        if(PathIndex == LevelManager.instance.GetWaypointsLength())
        {
            this.gameObject.SetActive(false);
            return;
        }
        TargetPosition = LevelManager.instance.GetPoint(PathIndex);
    }
}
