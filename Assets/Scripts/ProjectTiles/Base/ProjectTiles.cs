using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class ProjectTiles: MonoBehaviour, IPoolObject
    {
        public Tower Tower{get; set;}
        [FormerlySerializedAs("EnemyTarget")] [SerializeField]
        private Enemy _enemyTarget;
        private Vector3 _targetPosition = Vector3.zero;
        [Header("Attributes")]
        [SerializeField]
        private float _damageCause;
        public float DamageCause => _damageCause;
        [SerializeField]
        private float _speed;

        [SerializeField] private float minDistanceToDealDamage = 0.1f;

        private void Start()
        {
        }
        
        private void Update()
        {
            if(_enemyTarget) _targetPosition = _enemyTarget.gameObject.transform.position;
            MoveProjectTiles();
            RotateProjectTiles();
        }
        public void SetCurrentEnemy(Enemy enemy)
        {
            _enemyTarget = enemy;
        }
        
        
        
        private void MoveProjectTiles()
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
            float distanceToTarget = (_targetPosition - transform.position).magnitude;
            if (!(distanceToTarget < minDistanceToDealDamage)) return;
            if(_enemyTarget) _enemyTarget.TakenDamage(_damageCause);
            ReturnToPool();
        }
        private void RotateProjectTiles()
        {
            var enemyPos = _targetPosition - transform.position;
            float angle = Vector3.SignedAngle(transform.up, enemyPos, transform.forward);
            transform.Rotate(0f, 0f, angle);
        }

        public void ReturnToPool()
        {
            ResetValue();
            PoolingObject.Instance.ReturnObject(this.gameObject);
        }

        public void ResetValue()
        {
            Tower = null;
            _enemyTarget = null;
            _targetPosition = Vector3.zero;
        }
    }