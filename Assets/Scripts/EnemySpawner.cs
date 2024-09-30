using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Events;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private SpawnEnemiesInfo _enemiesSceneInfo;
    [Header("Attributes")]
    [SerializeField]
    private int[] _countEnemies;
    [SerializeField]
    float timeBetweenWaves = 5.0f;
    [SerializeField]
    float enemiesPerSec = 2f;
    [SerializeField]
    int baseEnemies = 3;
    [SerializeField]
    float diffScalingFactor = 0.75f;
    [SerializeField]
    bool isSpawning = false;
    [SerializeField]
    int maxNumsWave = 3;

    [SerializeField]
    int currentWave = 1;
    float timeSinceLastWave = 0;
    [SerializeField]
    int enemiesAlive = 0;
    [SerializeField]
    int enemiesLeftToSpawn = 0;

    [Header("Events")]
    public static UnityEvent OnEnemyDestroy = new UnityEvent();

    private void Awake()
    {
        
    }

    void Start()
    {
        OnEnemyDestroy.AddListener(EnemyBeingDestroy);
        
        StartWave();
    }

    void Update()
    {
        if (!isSpawning) return;
        timeSinceLastWave += Time.deltaTime;
        if (timeSinceLastWave >= (1f/ enemiesPerSec))
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            timeSinceLastWave = 0;
            if(enemiesLeftToSpawn == 0)
            {
                isSpawning = false;
                StartCoroutine(nameof(WaitBetweenWaves));
            }
        }
    }

    void StartWave()
    {
        if(currentWave > maxNumsWave)
        {
            isSpawning = false;
            currentWave = maxNumsWave;
            return;
        }
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, diffScalingFactor));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void SpawnEnemy()
    {
        var newEnemy = PoolingObject.Instance.GetObject(_enemiesSceneInfo.Enemies[1]);
        newEnemy.SetActive(true);
        enemiesAlive++;
    }

    void EnemyBeingDestroy()
    {
        enemiesAlive--;
    }

    private IEnumerator WaitBetweenWaves()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        currentWave++;
        StartWave();
    }
}
