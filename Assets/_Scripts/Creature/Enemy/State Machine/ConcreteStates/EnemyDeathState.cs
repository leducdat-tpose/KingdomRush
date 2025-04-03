using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState: BaseState<Enemy>
{
    public EnemyDeathState(Enemy enemy, StateManager<Enemy> enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {

        GameObject effect = PoolingObject.Instance.GetEffectObject("BleedingBlood", EffectType.BleedingSmallRed);
        effect.transform.position = owner.transform.position;
        effect.SetActive(true);
        owner.StartCoroutine("StartRemainDeath");
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }
}
