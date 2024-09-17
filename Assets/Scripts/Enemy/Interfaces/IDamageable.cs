using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakenDamage(float damageAmount);
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
}
