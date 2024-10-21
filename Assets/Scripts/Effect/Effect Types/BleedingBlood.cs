using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BleedingBlood : Effect
{
    public static string Name = "BleedingBlood";
    private readonly int _greenBig = Animator.StringToHash("Bleeding_Green_Big");
    private  readonly int _redBig = Animator.StringToHash("Bleeding_Red_Big");
    private  readonly int _violetBig = Animator.StringToHash("Bleeding_Violet_Big");
    private  readonly int _greenSmall = Animator.StringToHash("Bleeding_Green_Small");
    private  readonly int _redSmall = Animator.StringToHash("Bleeding_Red_Small");
    private  readonly int _violetSmall = Animator.StringToHash("Bleeding_Violet_Small");
    protected override void Render()
    {
        base.Render();
        var idAnimation = _redSmall;
        switch(effectType)
        {
            case EffectType.BleedingSmallGreen:
                idAnimation = _greenSmall;
                break;
            case EffectType.BleeingSmallViolet:
                idAnimation = _violetSmall;
                break;
            case EffectType.BleedingBigRed:
                idAnimation = _redBig;
                break;
            case EffectType.BleedingBigGreen:
                idAnimation = _greenBig;
                break;
            case EffectType.BleedingBigViolet:
                idAnimation = _violetBig;
                break;
        }
        if(currentAnimation == idAnimation) return;
        animator.CrossFade(idAnimation, 0, 0);
        currentAnimation = idAnimation;
    }
}
