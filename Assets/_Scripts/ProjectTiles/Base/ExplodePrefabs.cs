using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class ExplodePrefabs : MonoBehaviour
{
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [SerializeField] private float _damage;

    // Start is called before the first frame update
    void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        if(_circleCollider2D == null) {
            _circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
        }
        _circleCollider2D.isTrigger = true;
        _damage = transform.parent.GetComponent<ProjectTiles>().DamageCause;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().TakenDamage(_damage);
        }
    }
    void Update()
    {
        
    }

}
