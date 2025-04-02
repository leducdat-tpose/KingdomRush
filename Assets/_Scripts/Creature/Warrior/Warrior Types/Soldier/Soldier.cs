using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Soldier : MeleeWarrior, IDamageable
{
    public Action<float, float> HealthChanged {get; set;}

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public virtual void TakenDamage(float damageAmount)
    {
        if(currentHealth <= 0) return;
        currentHealth -= damageAmount;
        HealthChanged?.Invoke(currentHealth, data.MaxHealth);
        if(currentHealth > 0) return;
        currentHealth = 0;
        // Behaviour.ChangeState(StateType.Death);
    }
    public void ReturnPoolObject()
    {
        PoolingObject.Instance.ReturnObject(this.gameObject);
    }

    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
