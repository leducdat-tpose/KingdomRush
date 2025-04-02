using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicHPBar : DetMonobehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Camera _camera;
    [SerializeField]
    private IDamageable _damageableObj;
    [SerializeField]
    private GameObject _target;
    [SerializeField] private Vector3 _offset;
    protected override void LoadComponents()
    {
        _slider = GetComponent<Slider>();
        _camera = Camera.main;
        _slider.value = 1;
        _damageableObj = transform.parent.parent.GetComponent<IDamageable>();
        _damageableObj.HealthChanged += UpdateHealthBar;
        _target = transform.root.gameObject;
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth) {
        _slider.value = currentHealth / maxHealth;
    }
    private void Update() {
        if(_damageableObj == null) return;
        transform.rotation = _camera.transform.rotation;
        transform.position = _target.transform.position + _offset;
    }
}
