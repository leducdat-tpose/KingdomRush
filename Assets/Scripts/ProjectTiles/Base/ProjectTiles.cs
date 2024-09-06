using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjectTiles.Base
{
    [RequireComponent(typeof(Collider2D),typeof(Rigidbody2D))]
    public class ProjectTiles: MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Collider2D _collider;
        [SerializeField]
        private Rigidbody2D _rigid;

        private Enemy _currentEnemy;
        [Header("Attributes")]
        [SerializeField]
        private float _damageCause;
        public float DamageCause => _damageCause;
        [SerializeField]
        private float _speed;

        private void Reset()
        {
            _collider = GetComponent<Collider2D>();
            _rigid = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _currentEnemy = null;
        }

        private void Update()
        {
            if (!_currentEnemy) return;
        }

        private void FixedUpdate()
        {
            if (!_currentEnemy) return;
            ChasingEnemy();
        }

        public void SetCurrentEnemy(Enemy thisEnemy)
        {
            _currentEnemy = thisEnemy;
        }

        private void ChasingEnemy()
        {
            var directionToEnemy = transform.position - _currentEnemy.transform.position;
            var resultDirect = Vector3.RotateTowards(transform.forward, directionToEnemy, _speed * Time.deltaTime, 1);
            _rigid.velocity = resultDirect.normalized * _speed;
            if (Vector2.Distance(_currentEnemy.transform.position, transform.position) > 0.1f) return;
            _currentEnemy.Damage(_damageCause);
            //Build bool object later
            this.gameObject.SetActive(false);
        }
    }
}