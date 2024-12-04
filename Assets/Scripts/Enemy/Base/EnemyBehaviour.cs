using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour<T> : MonoBehaviour
{
    public StateManager<T> StateManager;
    public BaseState<T> IdleState;
    public BaseState<T> WalkState;
    public BaseState<T> AttackState;
    public BaseState<T> DeathState;

    public abstract void Start();

    public abstract void Update();

    public abstract void FixedUpdate();

}
