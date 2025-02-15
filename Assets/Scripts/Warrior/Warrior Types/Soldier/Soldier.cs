using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Soldier : Warrior
{
    [Header("Soldier Settings")]
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private CircleCollider2D _collider2D;
    [SerializeField] Rigidbody2D _rigidbody2D;
}
