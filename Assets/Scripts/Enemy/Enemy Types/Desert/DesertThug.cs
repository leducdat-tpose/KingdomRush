using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertThug : Enemy
{
    protected override void Update()
    {
        base.Update();
        Render();
    }

    protected override void Render()
    {
        var newAnimation = "Idle";
        if (IsAttacking) newAnimation = "Attack";
        if (IsDead) newAnimation = "Dead";
        ChangeAnimation(newAnimation);
    }
}
