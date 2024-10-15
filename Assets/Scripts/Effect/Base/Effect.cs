using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType{
    None,
    BleedingSmallRed,
    BleedingSmallGreen,
    BleeingSmallViolet,
    BleedingBigRed,
    BleedingBigGreen,
    BleedingBigViolet,
}

public class Effect : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private SpriteRenderer _spriteRender;
    [SerializeField]private Animator _animator;

    [Header("Attributes")]
    private EffectType _effectType;

    protected virtual void Start() {
        _spriteRender = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _effectType = EffectType.None;
    }
    protected virtual void Update() {
        Render();
    }
    protected virtual void ReturnToPool()
    {
        PoolingObject.Instance.ReturnObject(this.gameObject);
    }

    public void SetEffectType(EffectType type)
    {
        this._effectType = type;
    }
    protected virtual void Render()
    {
        if(_effectType == EffectType.None) return;
    }
}
