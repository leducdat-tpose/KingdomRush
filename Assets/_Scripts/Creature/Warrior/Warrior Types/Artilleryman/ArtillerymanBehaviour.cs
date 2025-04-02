using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtillerymanBehaviour : RangerWarrior
{
    public override void Render()
    {
        int newAnimation = idleAnimation;
        if (stateManager.CurrentState != idleState)
        {
            if (stateManager.CurrentState == attackState)
                newAnimation = attackAnimation;
        }
        if(CurrentAnimation == newAnimation) return;
        animator.CrossFade(newAnimation, 0.1f, 0);
        CurrentAnimation = newAnimation;
    }
}
