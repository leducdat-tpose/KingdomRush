using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingBlood : Effect
{
    private int GreenBig = Animator.StringToHash("Bleeding_Big_Green");
    private int RedBig = Animator.StringToHash("Bleeding_Big_Red");
    private int VioletBig = Animator.StringToHash("Bleeding_Big_Violet");
    private int GreenSmall = Animator.StringToHash("Bleeding_Small_Green");
    private int RedSmall = Animator.StringToHash("Bleeding_Small_Red");
    private int VioletSmall = Animator.StringToHash("Bleeding_Small_Violet");
    protected override void Render()
    {
        base.Render();
        var idAnimation = RedSmall;
        switch(effectType)
        {
            case EffectType.BleedingSmallGreen:
                idAnimation = GreenSmall;
                break;
            case EffectType.BleeingSmallViolet:
                idAnimation = VioletSmall;
                break;
            case EffectType.BleedingBigRed:
                idAnimation = RedBig;
                break;
            case EffectType.BleedingBigGreen:
                idAnimation = GreenBig;
                break;
            case EffectType.BleedingBigViolet:
                idAnimation = VioletBig;
                break;
        }
        if(currentAnimation == idAnimation) return;
        animator.CrossFade(idAnimation, 0);
        currentAnimation = idAnimation;
    }
}
