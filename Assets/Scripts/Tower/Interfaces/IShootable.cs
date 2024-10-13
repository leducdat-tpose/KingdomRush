using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    Collider2D Collider { get; set; }
    float AttackRange { get; set; }

}
