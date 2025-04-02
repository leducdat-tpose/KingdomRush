using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer),
    typeof(Animator))]
public class Warrior : MonoBehaviour, ICreature
{
    [Header("References")]
    [SerializeField] private GameObject _prefabProjectile;
    public GameObject PrefabProjectTile => _prefabProjectile;
    [field: SerializeField, Range(1,3)]public int Level{get;private set;} = 1;

    [field:SerializeField]public float MaxHealth { get; set; }
    [field:SerializeField]public float CurrentHealth { get; set; }
    [field:SerializeField]public float BaseDamage { get; set; }
    [field:SerializeField]public float MoveSpeed { get; set;}
    private BaseBehaviour<Warrior> _behaviour;
    [SerializeField] protected float coolDownAttack;
    
    public float CoolDownAttack => coolDownAttack;

    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
    }
    public virtual void TakenDamage(float damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth > 0) return;
        CurrentHealth = 0;
        Destroy(this.gameObject);
    }
    public void SetDamageCause(float value) => this.BaseDamage = value;

}
