using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryTower : Tower
{
    [SerializeField]
    private Animator _animator;
    [SerializeField, Range(1,3)]
    private int _level = 1;
    protected override void Start()
    {
        base.Start();
        HaveAnimation = true;
        _animator = GetComponent<Animator>();
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
        if(InProgress) newAnimation = StartingProgress;
        if (newAnimation == _currentAnimation) return;
        _animator.CrossFade(newAnimation, 0.2f, _level - 1);
        _currentAnimation = newAnimation;
    }
}
