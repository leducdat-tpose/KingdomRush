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
    Transform target;

    private void Reset()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }

    void Start()
    {
        target = LevelManager.instance.paths[pathIndex];
    }

    void Update()
    {
        if(Vector2.Distance(target.position, transform.position) < 0.1f)
        {
            pathIndex++;
            if(pathIndex == LevelManager.instance.paths.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                this.gameObject.SetActive(false);
                return;
            }
            target = LevelManager.instance.paths[pathIndex];
        }
    }
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rigid.velocity = direction * moveSpeed;
    }
}
