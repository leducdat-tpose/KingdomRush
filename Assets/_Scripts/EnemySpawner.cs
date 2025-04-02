using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    private SpawnEnemiesInfo _enemiesSceneInfo;

    [Header("Attributes")] 
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float enemiesPerSec;
    [SerializeField] private int baseEnemies;
    [SerializeField] private float diffScalingFactor;
    [SerializeField] private bool isSpawning = false;

    public int TotalWaves{get; private set;}
    public int CurrentWave{get; private set;}
    private float timeSinceLastWave = 0;
    [SerializeField] private int enemiesAlive = 0;
    [SerializeField] private int enemyIndex;
    [SerializeField] private int amountEnemyNeedToSpawn;
    [SerializeField] private int enemiesLeftToSpawn = 0;

    [Header("Events")]
    public static UnityEvent OnEnemyDestroy = new UnityEvent();

    private void Awake()
    {
        OnEnemyDestroy.AddListener(EnemyBeingDestroyed);
    }

    void Start()
    {
        CurrentWave = 1;
        _enemiesSceneInfo = GameController.Instance.EnemiesSceneInfo;
        if(_enemiesSceneInfo == null)
        {
            Debug.LogError("Forgot to assign enemies scene info in gamecontroller");
            return;
        }
        TotalWaves = _enemiesSceneInfo.waves.Count;
        timeBetweenWaves = _enemiesSceneInfo.TimeBetweenWaves;
        enemiesPerSec = _enemiesSceneInfo.EnemiesPerSec;
        baseEnemies = _enemiesSceneInfo.BaseEnemies;
        diffScalingFactor = _enemiesSceneInfo.DiffScalingFactor;
        enemyIndex = 0;
        GetAmountEnemyNeedToSpawn();
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
            if (enemiesLeftToSpawn == 0)
            {
                isSpawning = false;
                StartCoroutine(nameof(WaitBetweenWaves));
            }
        }
    }

    void StartWave()
    {
        if (CurrentWave > TotalWaves)
        {
            isSpawning = false;
            CurrentWave = TotalWaves;

            return;
        }

        isSpawning = true;
        ResetWave();
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private int EnemiesPerWave()
    {
        return _enemiesSceneInfo.waves[CurrentWave - 1].GetTotalAmount();
    }

    private void GetAmountEnemyNeedToSpawn()
    {
        amountEnemyNeedToSpawn = _enemiesSceneInfo.waves[CurrentWave - 1].enemiesInfo[enemyIndex].amount;
    }

    private void ResetWave()
    {
        enemyIndex = 0;
        GetAmountEnemyNeedToSpawn();
    }

    private void UpdateIndexEnemy()
    {
        amountEnemyNeedToSpawn--;
        if (amountEnemyNeedToSpawn <= 0)
        {
            if (enemyIndex < _enemiesSceneInfo.waves[CurrentWave - 1].enemiesInfo.Count - 1)
            {
                enemyIndex++;
                GetAmountEnemyNeedToSpawn();
            }
        }
    }

    void SpawnEnemy()
    {
        if (PoolingObject.Instance == null)
        {
            Debug.LogError("PoolingObject.Instance is null");
            return;
        }

        if (_enemiesSceneInfo.waves == null || _enemiesSceneInfo.waves.Count < CurrentWave)
        {
            Debug.LogError("Invalid wave configuration or wave index out of bounds");
            return;
        }

        var currentWaveEnemies = _enemiesSceneInfo.waves[CurrentWave - 1].enemiesInfo;
        if (currentWaveEnemies == null || currentWaveEnemies.Count <= enemyIndex)
        {
            Debug.LogError("Invalid enemy index or enemies info is null");
            return;
        }
       
        var enemyPrefab = currentWaveEnemies[enemyIndex].prefab;
        var newEnemy = PoolingObject.Instance.GetObject(enemyPrefab);
       
        if (newEnemy == null)
        {
            Debug.LogError("Failed to get or create a new enemy");
            return;
        }

        newEnemy.SetActive(true);
        enemiesAlive++;
        UpdateIndexEnemy();
    }

    void EnemyBeingDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator WaitBetweenWaves()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        CurrentWave++;
        StartWave();
        UIController.Instance.UpdateWave();
    }
}