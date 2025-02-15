using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtillerymanBehaviour : RangerWarriorBehaviour
{
    public override void Render()
    {
        int newAnimation = idleAnimation;
        if (StateManager.CurrentState != IdleState)
        {
            if (StateManager.CurrentState == AttackState)
                newAnimation = attackAnimation;
        }
        if(CurrentAnimation == newAnimation) return;
        animator.CrossFade(newAnimation, 0.1f, 0);
        CurrentAnimation = newAnimation;
    }
}
