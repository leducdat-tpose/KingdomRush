using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class ProjectTiles: MonoBehaviour
    {
        public Tower Tower{get; set;}
        [FormerlySerializedAs("EnemyTarget")] [SerializeField]
        private Enemy _enemyTarget;
        protected Vector3 targetPosition = Vector3.zero;
        [Header("Attributes")]
        [SerializeField]
        private float _damageCause;
        public float DamageCause => _damageCause;
        [SerializeField]
        private float _speed;
        public float Speed => _speed;

        [SerializeField] private float minDistanceToDealDamage = 0.1f;

        protected virtual void Start()
        {
        }
        
        protected virtual void Update()
        {
            if(_enemyTarget) targetPosition = _enemyTarget.gameObject.transform.position;
            MoveProjectTiles();
            RotateProjectTiles();
        }
        public void SetCurrentEnemy(Enemy enemy)
        {
            _enemyTarget = enemy;
        }
        
        
        
        protected virtual void MoveProjectTiles()
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            float distanceToTarget = (targetPosition - transform.position).magnitude;
            if (!(distanceToTarget < minDistanceToDealDamage)) return;
            if(_enemyTarget) _enemyTarget.TakenDamage(_damageCause);
            ReturnToPool();
        }
        protected virtual void RotateProjectTiles()
        {
            var enemyPos = targetPosition - transform.position;
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
            targetPosition = Vector3.zero;
        }

        public void SetDamageCause(float damage)
        {
            _damageCause = damage;
        }
        protected virtual IEnumerator CurvePath(Vector2 start, Vector2 target, float duration, float heightY, AnimationCurve curve)
        {
            float timePassed = 0f;
            Vector2 end = target;
            while (timePassed <= duration)
            {
                timePassed += Time.deltaTime;
                float linearT = timePassed / duration;
                float heightT = curve.Evaluate(linearT);
                float height = Mathf.Lerp(0, heightY, heightT);
                transform.position = Vector2.Lerp(start, end, linearT) + Vector2.up * height;
                yield return null;
            }
        }
    }