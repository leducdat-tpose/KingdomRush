using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IDamageable
{
    void TakenDamage(float damageAmount);
    Action<float, float> HealthChanged{get;set;}
}
