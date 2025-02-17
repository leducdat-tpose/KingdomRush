using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Soldier : Warrior, IDamageable
{
    public float DomainRange {get; private set;} = 2f;
    public float AttackRange {get; private set;} = 0.5f;
    public Rigidbody2D Rigidbody{get; private set;}
    public MeleeWarriorBehaviour Behaviour {get; private set;}
    [field: SerializeField]
    public DynamicHPBar DynamicHpBar { get; set; }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
    #endif
    protected override void Start()
    {
        base.Start();
        Rigidbody = GetComponent<Rigidbody2D>();
        DynamicHpBar = GetComponentInChildren<DynamicHPBar>();
        Behaviour = GetComponent<MeleeWarriorBehaviour>();
    }
    public override void TakenDamage(float damageAmount)
    {
        if(Behaviour.GetCurrentState() == Behaviour.DeathState)
        return;
        CurrentHealth -= damageAmount;
        DynamicHpBar?.UpdateHealthBar(CurrentHealth, MaxHealth);
        if(CurrentHealth > 0) return;
        CurrentHealth = 0;
        Behaviour.ChangeState(StateType.Death);
    }
    public void ReturnPoolObject()
    {
        PoolingObject.Instance.ReturnObject(this.gameObject);
    }
}
