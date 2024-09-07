using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjectTiles.Base
{
    public class ProjectTiles: MonoBehaviour
    {
        public static Action<Enemy, float> OnEnemyHit;
        public Tower Tower{get; set;}

        private Enemy _enemyTarget;
        [Header("Attributes")]
        [SerializeField]
        private float _damageCause;
        public float DamageCause => _damageCause;
        [SerializeField]
        private float _speed;

        [SerializeField] private float minDistanceToDealDamage = 0.1f;
        private void Reset()
        {
        }

        private void Start()
        {
            _enemyTarget = null;
        }

        private void Update()
        {
            if (!_enemyTarget) return;
            MoveProjectTiles();
            RotateProjectTiles();
        }
        public void SetCurrentEnemy(Enemy thisEnemy)
        {
            _enemyTarget = thisEnemy;
        }

        private void MoveProjectTiles()
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyTarget.gameObject.transform.position, _speed * Time.deltaTime);
            float distanceToTarget = (_enemyTarget.gameObject.transform.position - transform.position).magnitude;
            if (distanceToTarget > minDistanceToDealDamage) return;
            _enemyTarget.Damage(_damageCause);
            
        }
        private void RotateProjectTiles()
        {
            var enemyPos = _enemyTarget.transform.position - transform.position;
            float angle = Vector3.SignedAngle(transform.up, enemyPos, transform.forward);
            transform.Rotate(0f, 0f, angle);
        }
        
    }
}