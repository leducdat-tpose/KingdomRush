using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType{
    None,
    #region Bleeding
    BleedingSmallRed,
    BleedingSmallGreen,
    BleeingSmallViolet,
    BleedingBigRed,
    BleedingBigGreen,
    BleedingBigViolet,
    #endregion
}
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Effect : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private SpriteRenderer _spriteRender;
    [SerializeField]protected Animator animator;

    [Header("Attributes")]
    [SerializeField]protected EffectType effectType = EffectType.None;
    protected int currentAnimation = 0;

    protected virtual void Start() {
        _spriteRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    protected virtual void Update() {
        Render();
        EffectMovement();
    }
    //Some effects need to be set a specific position
    protected virtual void SetPosition(Vector3 thisObj)
    {

    }
    //Movement of the effect
    protected virtual void EffectMovement()
    {

    }

    protected virtual void ReturnToPool()
    {
        effectType = EffectType.None;
        PoolingObject.Instance.ReturnObject(this.gameObject);
    }

    public void SetEffectType(EffectType type)
    {
        this.effectType = type;
    }
    protected virtual void Render()
    {
        if(effectType == EffectType.None) return;
    }
}
