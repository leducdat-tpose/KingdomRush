using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MageTower : Tower
{
    [SerializeField]
    private Animator _animator;
    [SerializeField, Range(1,3)]
    private int _level = 1;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        _animator = GetComponent<Animator>();
        HaveAnimation = true;
        for(int i = 0; i < _animator.layerCount; i++)
        {
            if(i == (_level - 1)) _animator.SetLayerWeight(i, 1);
            else _animator.SetLayerWeight(i, 0);
        }
    }

    protected override void Update()
    {
        base.Update();
        Render();
    }

    protected override void Render()
    {
        var newAnimation = Idle;
        if(inProgress) newAnimation = StartingProgress;
        if (newAnimation == currentAnimation) return;
        _animator.CrossFade(newAnimation, 0.2f, _level - 1);
        currentAnimation = newAnimation;
    }
}
