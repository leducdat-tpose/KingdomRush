using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    DynamicHPBar DynamicHpBar { get; set; }

    void TakenDamage(float damageAmount);
}
