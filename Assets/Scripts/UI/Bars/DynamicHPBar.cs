using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicHPBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    private void Start() {
        _slider = GetComponent<Slider>();
        _camera = Camera.main;
        _target = transform.parent.parent.transform;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth) {
        _slider.value = currentHealth / maxHealth;
    }
    private void Update() {
        if(_target == null) return;
        transform.rotation = _camera.transform.rotation;
        transform.position = _target.gameObject.transform.position + _offset;
    }
}
