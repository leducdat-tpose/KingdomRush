using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreature
{
    float MaxHealth{ get; set; }
    float CurrentHealth{ get; set; }
    float BaseDamage{ get; set; }
    float MoveSpeed{ get; set; }
}
