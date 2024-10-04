using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MageTower : Tower
{
    [SerializeField]
    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        HaveAnimation = true;
    }

    protected override void Update()
    {
        base.Update();
        Render();
    }

    protected override void Render()
    {
        var newAnimation = Idle;
        if(InProgress) newAnimation = StartingProgress;
        if (newAnimation == _currentAnimation) return;
        _animator.CrossFade(newAnimation, 0.2f);
        _currentAnimation = newAnimation;
    }
}
