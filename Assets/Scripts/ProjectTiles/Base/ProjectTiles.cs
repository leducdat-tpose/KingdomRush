using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class ProjectTiles: MonoBehaviour
    {
        public static Action<Enemy, float> OnEnemyHit;
        public Tower Tower{get; set;}
        [FormerlySerializedAs("_enemyTarget")] [SerializeField]
        public Enemy EnemyTarget;
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
            EnemyTarget = Tower.CurrentTarget;
            if (!EnemyTarget) return;
            MoveProjectTiles();
            RotateProjectTiles();
        }
        public void SetCurrentEnemy(Enemy enemy)
        {
            Debug.Log("From SetCurrentEnemy", enemy);
            if (enemy == EnemyTarget) return;
            this.EnemyTarget = enemy;
        }
        
        
        
        private void MoveProjectTiles()
        {
            transform.position = Vector2.MoveTowards(transform.position, EnemyTarget.gameObject.transform.position, _speed * Time.deltaTime);
            float distanceToTarget = (EnemyTarget.gameObject.transform.position - transform.position).magnitude;
            if (distanceToTarget > minDistanceToDealDamage) return;
            EnemyTarget.Damage(_damageCause);
            PoolingObject.Instance.ReturnObject(gameObject);
        }
        private void RotateProjectTiles()
        {
            var enemyPos = EnemyTarget.transform.position - transform.position;
            float angle = Vector3.SignedAngle(transform.up, enemyPos, transform.forward);
            transform.Rotate(0f, 0f, angle);
        }
        
    }