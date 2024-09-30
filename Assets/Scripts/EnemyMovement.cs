using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    Rigidbody2D rigid;

    [Header("Attributes")]
    [SerializeField]
    float moveSpeed = 10f;

    int pathIndex = 0;
    Vector3 target;

    private void Reset()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }

    void Start()
    {
        target = LevelManager.instance.GetStartingPoint();
        transform.position = target;
    }

    void Update()
    {
        if (Vector2.Distance(target, transform.position) > 0.1f) return;
        pathIndex++;
        if (pathIndex == LevelManager.instance.GetWaypointsLength())
        {
            EnemySpawner.OnEnemyDestroy.Invoke();
            this.gameObject.SetActive(false);
            return;
        }
        target = LevelManager.instance.GetPoint(pathIndex);
    }
    private void FixedUpdate()
    {
        Vector2 direction = (target - transform.position).normalized;
        rigid.velocity = direction * moveSpeed;
    }
}
