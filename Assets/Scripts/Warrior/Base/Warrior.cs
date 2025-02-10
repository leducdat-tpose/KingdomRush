using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer),
    typeof(Animator))]
public class Warrior : MonoBehaviour, IMoveable, ICreature
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
    [SerializeField] private GameObject _prefabProjectile;
    public GameObject PrefabProjectTile => _prefabProjectile;
    [SerializeField] private List<Sprite> _spritesSoliderUpgrade;


    [Header("Attributes")]
    [SerializeField] private float _upgradeDamageIndex;
    [field:SerializeField]public float MaxHealth { get; set; }
    [field:SerializeField]public float CurrentHealth { get; set; }
    [field:SerializeField]public float BaseDamage { get; set; }
    public int Level { get; set; }
    public float MoveSpeed { get; set;}
    private BaseBehaviour<Warrior> _behaviour;
    [SerializeField] protected float coolDownAttack;
    [field:SerializeField]public float Speed { get; set; }
    public Vector3 StandingPosition { get; set; }
    
    public float CoolDownAttack => coolDownAttack;

    protected virtual void Start()
    {
        // _behaviour = new RangerWarriorBehaviour();
        Level = 1;
        CurrentHealth = MaxHealth;
    }
    // protected virtual void UpgradeSolider(int towerLevel)
    // {
    //     if (Level == _spritesSoliderUpgrade.Count) return;
    //     Level = towerLevel;
    //     _damageCause = BaseDamage * _upgradeDamageIndex * Level;
    //     _spriteRenderer.sprite = _spritesSoliderUpgrade[Level - 1];
    //     string stringLevel = " Level_"+Level.ToString();
    //     Idle = Animator.StringToHash("Idle" + stringLevel);
    //     Walk = Animator.StringToHash("Walk" + stringLevel);
    //     Attack = Animator.StringToHash("Attack" + stringLevel);
    //     Death = Animator.StringToHash("Death" + stringLevel);
    //     IdleUp = Animator.StringToHash("Idle_Up" + stringLevel);
    //     AttackUp = Animator.StringToHash("Attack_Up" + stringLevel);
    // }
    public void TakenDamage(float damage)
    {}
    public void UpgradeDamage()=>this.BaseDamage *= _upgradeDamageIndex;

    public void SetDamageCause(float value) => this.BaseDamage = value;
}
